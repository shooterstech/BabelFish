using System.IO;
using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.ScoposData;
using Scopos.BabelFish.Requests.ImageAPI;
using Scopos.BabelFish.Runtime.Authentication;


namespace Scopos.BabelFish.Tests.APIClients.ImageClientTests {
    [TestClass]
    public class UploadImageTests : BaseTestClass {
        private static FileInfo GetImageFileInfoFromResource( string fileName ) {
            var assembly = typeof( UploadImageTests ).Assembly;
            var resourceName = $"Scopos.BabelFish.Tests.Resources.Images.{fileName}";

            using var stream = assembly.GetManifestResourceStream( resourceName )
                ?? throw new FileNotFoundException( $"Embedded resource not found: {resourceName}" );

            var tempDirectory = Path.Combine( Path.GetTempPath(), "BabelFish.Tests", "Images" );
            Directory.CreateDirectory( tempDirectory );

            var tempFilePath = Path.Combine(
                tempDirectory,
                Path.GetFileName( fileName ) );

            using (var fileStream = new FileStream( tempFilePath, FileMode.Create, FileAccess.Write, FileShare.None )) {
                stream.CopyTo( fileStream );
            }

            return new FileInfo( tempFilePath );
        }

        [TestMethod]
        public async Task PatchModerateImageLocalFileTest() {
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            var request = new UploadImageAuthenticatedRequest( userAuthentication ) {
                Caption = "A local test image",
                AltText = "Local test image",
                ImageCategory = ImageCategory.CLUB,
                PrimaryKey = "15", // License Number
                SubKey = "", // Should be left empty for CLUB images.
                GroupKey = ImageGroupKeyType.BULK,
                ImageFile = GetImageFileInfoFromResource( "Milton Farrow.jpg" )
            };

            var client = new ImageClient();
            var response = await client.UploadImageAuthenticatedAsync( request );

            Assert.IsNotNull( response );
            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode, $"Expecting an OK status code, instead received {response.RestApiStatusCode} with overall status {response.OverallStatusCode}, and message {response.ExceptionMessage}." );
            Assert.IsTrue( response.HasOkStatusCode );
            Assert.IsNotNull( response.Image );
            Assert.IsNotEmpty( response.Image.UrlPath );
            Assert.AreEqual( request.Caption, response.Image.Caption );
            Assert.AreEqual( request.AltText, response.Image.AltText );
            Assert.AreEqual( request.PrimaryKey, response.Image.PrimaryKey );
            Assert.AreEqual( request.SubKey, response.Image.SubKey );
            Assert.AreEqual( request.GroupKey, response.Image.GroupKey );
            Assert.AreEqual( Constants.TestDev7UserId, response.Image.UserId );

            Console.WriteLine( $"Image uploaded successfully. URL: {response.Image.UrlPath}, Status: {response.Image.SafeToShowStatus}" );
        }
    }
}
