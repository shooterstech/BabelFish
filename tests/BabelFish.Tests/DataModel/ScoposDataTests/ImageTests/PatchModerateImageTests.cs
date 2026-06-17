using System.IO;
using System.Net;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.ScoposData;
using Scopos.BabelFish.Runtime.Authentication;
using System.Threading.Tasks;


namespace Scopos.BabelFish.Tests.DataModel.ScoposDataTests.ImageTests {
    [TestClass]
    public class PatchModerateImageTests : BaseTestClass {

        [TestMethod]
        public async Task PatchModerateImageLocalFileTest() {
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            var imagePath = Path.Combine(
                AppContext.BaseDirectory,
                "..",
                "..",
                "..",
                "..",
                "..",
                "tests",
                "BabelFish.Tests",
                "DataModel",
                "ScoposDataTests",
                "ImageTests",
                "walkingdead.jpg" );

            var request = new PatchModerateImageAuthenticatedRequest( userAuthentication ) {
                ImageBytes = File.ReadAllBytes( imagePath ),
                Caption = "A local test image",
                FileName = "walkingdead.jpg",
                AltText = "Local test image",
                Key = "LOCAL_TEST_PRIMARY_KEY",
                SubKey = "LOCAL_TEST_SUB_KEY",
                GroupKey = "LOCAL_TEST_GROUP_KEY"
            };

            var client = new ScoposDataClient( APIStage.PRODUCTION );
            var response = await client.PatchModerateImageAuthenticatedAsync( request );

            Assert.IsNotNull( response );
            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode, $"Expecting an OK status code, instead received {response.RestApiStatusCode} with overall status {response.OverallStatusCode}, and message {response.ExceptionMessage}." );
            Assert.IsTrue( response.HasOkStatusCode );
            Assert.IsNotNull( response.Image );
            Assert.AreEqual( "images/OrionAcct000001/walkingdead.jpg", response.Image.S3Key );
            Assert.AreEqual( request.Caption, response.Image.Caption );
            Assert.AreEqual( request.AltText, response.Image.AltText );
            Assert.AreEqual( request.Key, response.Image.Key );
            Assert.AreEqual( request.SubKey, response.Image.SubKey );
            Assert.AreEqual( request.GroupKey, response.Image.GroupKey );
            Assert.AreEqual( Constants.TestDev7UserId, response.Image.UserId );
        }
    }
}
