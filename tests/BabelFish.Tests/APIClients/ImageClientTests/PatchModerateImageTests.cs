using System.IO;
using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.ScoposData;
using Scopos.BabelFish.Runtime.Authentication;


namespace Scopos.BabelFish.Tests.APIClients.ImageClientTests {
    [TestClass]
    public class PatchModerateImageTests : BaseTestClass {
        private static byte[] LoadImageBytesFromResource( string fileName ) {
            /* 
             * In order for this code to work, the image file must be added to the project as an embedded resource.
             * Check the properties of the image file in Visual Studio and ensure that "Build Action" is set to "Embedded Resource".
             */

            var assembly = typeof( PatchModerateImageTests ).Assembly;
            var resourceName = $"Scopos.BabelFish.Tests.Resources.Images.{fileName}";

            using var stream = assembly.GetManifestResourceStream( resourceName )
                ?? throw new FileNotFoundException(
                    $"Embedded resource not found: {resourceName}" );

            using var memoryStream = new MemoryStream();
            stream.CopyTo( memoryStream );
            return memoryStream.ToArray();
        }

        [TestMethod]
        public async Task PatchModerateImageLocalFileTest() {
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            var request = new UploadImageAuthenticatedRequest( userAuthentication ) {
                ImageBytes = LoadImageBytesFromResource( "Milton Farrow.jpg" ),
                FileType = ImageFileType.JPEG,
                Caption = "A local test image",
                AltText = "Local test image",
                ImageCategory = ImageCategory.CLUB,
                PrimaryKey = "15", // License Number
                SubKey = "", // Should be left empty for CLUB images.
                GroupKey = ImageGroupKeyType.BULK
            };

            var client = new ImageClient();
            var response = await client.PatchModerateImageAuthenticatedAsync( request );

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
