using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Image;
using Scopos.BabelFish.DataModel.ScoposData;
using Scopos.BabelFish.Requests.ImageAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.APIClients.ImageClientTests {
    /// <summary>
    /// Creates only new test_dev_7 User/Bulk images; never deletes pre-existing images.
    /// </summary>
    [TestClass]
    [TestCategory( "Live" )]
    [DoNotParallelize]
    public class DeleteImageLiveTests {
        private ImageClient _client = null!;
        private UserAuthentication _credentials = null!;
        private readonly HashSet<string> _pendingCleanup = new();
        private readonly List<string> _localFiles = new();

        [TestInitialize]
        public async Task InitializeAsync() {
            // Gate before authentication, initialization, or any remote writes.
            var apiKey = Environment.GetEnvironmentVariable( "ScoposXApiKey" );
            Assert.IsFalse( string.IsNullOrWhiteSpace( apiKey ), "ScoposXApiKey must be configured for live tests." );
            Initializer.Initialize( apiKey!, false );
            _credentials = new UserAuthentication( Constants.TestDev7Credentials.Username, Constants.TestDev7Credentials.Password );
            await _credentials.InitializeAsync();
            Assert.AreEqual( Constants.TestDev7UserId, await _credentials.GetUserIdAsync() );
            _client = new ImageClient( APIStage.BETA ) { IgnoreInMemoryCache = true, IgnoreFileSystemCache = true };
        }

        [TestMethod]
        public async Task DeleteUploadedImageReturnsImageAndRemovesItFromListAsync() {
            var uploaded = await UploadDedicatedImageAsync();
            var listed = await FindListedImageAsync( uploaded.ImageId );
            Assert.IsNotNull( listed, "The newly uploaded approved image should be listed before deletion." );
            Assert.IsTrue( listed.IsValid );
            Assert.AreEqual( uploaded.Caption, listed.Caption );
            Assert.AreEqual( uploaded.AltText, listed.AltText );
            Assert.AreEqual( uploaded.Url, listed.Url );

            var response = await _client.DeleteImageAuthenticatedAsync(
                new DeleteImageAuthenticatedRequest( uploaded.ImageId, _credentials ) );
            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            Assert.IsTrue( response.HasOkStatusCode );
            _pendingCleanup.Remove( uploaded.ImageId );
            AssertImage( listed, response.DeletedImage );
            Assert.IsNull( await FindListedImageAsync( uploaded.ImageId ), "The deleted image must not appear in an uncached image list." );
        }

        [TestMethod]
        public async Task DeletingSameUploadedImageAgainReturnsSameImageAsync() {
            var uploaded = await UploadDedicatedImageAsync();
            var listed = await FindListedImageAsync( uploaded.ImageId );
            Assert.IsNotNull( listed );
            for (int attempt = 0; attempt < 2; attempt++) {
                // Retries must work for this test's already-deleted row, without deleting another image.
                var response = await _client.DeleteImageAuthenticatedAsync(
                    new DeleteImageAuthenticatedRequest( uploaded.ImageId, _credentials ) );
                Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode, $"Deletion attempt {attempt + 1} failed." );
                Assert.IsTrue( response.HasOkStatusCode );
                _pendingCleanup.Remove( uploaded.ImageId );
                AssertImage( listed, response.DeletedImage );
            }
            Assert.IsNull( await FindListedImageAsync( uploaded.ImageId ) );
        }

        private async Task<ScoposImage> UploadDedicatedImageAsync() {
            string marker = $"BabelFish delete-image live test {Guid.NewGuid():N}";
            string fileName = Path.Combine( Path.GetTempPath(), $"babelfish-delete-image-{Guid.NewGuid():N}.jpg" );
            _localFiles.Add( fileName );
            using (var resource = typeof( DeleteImageLiveTests ).Assembly.GetManifestResourceStream(
                "Scopos.BabelFish.Tests.Resources.Images.Scopos_Owl_Transparent.jpg" )) {
                Assert.IsNotNull( resource );
                using var file = File.Create( fileName );
                await resource.CopyToAsync( file );
            }
            var request = new UploadImageAuthenticatedRequest( _credentials ) {
                ImageCategory = ImageCategory.USER,
                PrimaryKey = Constants.TestDev7UserId,
                GroupKey = ImageGroupKeyType.BULK,
                Caption = marker,
                AltText = marker,
                ImageFile = new FileInfo( fileName )
            };
            var response = await _client.UploadImageAuthenticatedAsync( request );
            var image = response.Image;
            // Register cleanup before assertions, but only for an ID confirmed as this run's upload.
            if (image != null && Guid.TryParse( image.ImageId, out _ ) &&
                image.Caption == marker && image.UserId == Constants.TestDev7UserId)
                _pendingCleanup.Add( image.ImageId );
            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode, "Beta upload failed." );
            Assert.IsTrue( response.HasOkStatusCode );
            Assert.IsNotNull( image );
            Assert.IsTrue( _pendingCleanup.Contains( image.ImageId ), "Upload did not identify this test's image; no unrelated ID will be deleted." );
            Assert.AreEqual( Constants.TestDev7UserId, image.PrimaryKey );
            Assert.AreEqual( ImageGroupKeyType.BULK, image.GroupKey );
            Assert.AreEqual( ImageSafeToShowStatus.APPROVED, image.SafeToShowStatus );
            Assert.IsFalse( string.IsNullOrWhiteSpace( image.Url ), "Upload must return the current Url field." );
            // Compare DELETE's timestamp with GET's stored timestamp, rather than upload's
            // separately generated response timestamp.
            return image;
        }

        private async Task<ScoposImageAbbr?> FindListedImageAsync( string imageId ) {
            var request = new GetImagesRequest( Constants.TestDev7UserId ) {
                IgnoreInMemoryCache = true, IgnoreFileSystemCache = true
            };
            while (true) {
                var response = await _client.GetImagesPublicAsync( request );
                Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
                Assert.IsTrue( response.HasOkStatusCode );
                var image = response.ImageList.Items.FirstOrDefault( image => image.ImageId == imageId );
                if (image != null)
                    return image;
                if (!response.HasMoreItems)
                    return null;
                request = response.GetNextRequest();
                request.IgnoreInMemoryCache = true;
                request.IgnoreFileSystemCache = true;
            }
        }

        private static void AssertImage( ScoposImageAbbr uploaded, ScoposImageAbbr? deleted ) {
            Assert.IsNotNull( deleted );
            Assert.AreEqual( uploaded.ImageId, deleted.ImageId );
            Assert.AreEqual( uploaded.Caption, deleted.Caption );
            Assert.AreEqual( uploaded.AltText, deleted.AltText );
            Assert.AreEqual( uploaded.Url, deleted.Url );
            Assert.AreEqual( uploaded.CreatedAt, deleted.CreatedAt );
        }

        [TestCleanup]
        public async Task CleanupAsync() {
            var failures = new List<string>();
            foreach (string imageId in _pendingCleanup.ToArray()) {
                try {
                    var response = await _client.DeleteImageAuthenticatedAsync(
                        new DeleteImageAuthenticatedRequest( imageId, _credentials ) );
                    if (response.HasOkStatusCode && response.RestApiStatusCode == HttpStatusCode.OK)
                        _pendingCleanup.Remove( imageId );
                    else
                        failures.Add( $"{imageId}: HTTP {(int)response.RestApiStatusCode}" );
                } catch (Exception ex) {
                    failures.Add( $"{imageId}: {ex.GetType().Name}" );
                }
            }
            foreach (string fileName in _localFiles)
                File.Delete( fileName );
            Assert.IsEmpty( failures, "Cleanup failed for test-created image(s): " + string.Join( ", ", failures ) );
        }
    }
}
