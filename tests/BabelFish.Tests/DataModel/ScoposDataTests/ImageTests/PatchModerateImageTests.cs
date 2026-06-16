using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.ScoposData;
using Scopos.BabelFish.Responses.ScoposData;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.DataModel.ScoposDataTests.ImageTests {
    [TestClass]
    public class PatchModerateImageTests : BaseTestClass {

        [TestMethod]
        public void PatchModerateImageAuthenticatedRequest_BuildsExpectedInternalPatchCall() {
            var request = new PatchModerateImageAuthenticatedRequest(
                CreateOfflineUserAuthentication(),
                "images/OrionAcct000001/swa.jpg" );

            Assert.AreEqual( "PatchModerateImage", request.OperationId );
            Assert.AreEqual( new HttpMethod( "PATCH" ), request.HttpMethod );
            Assert.AreEqual( APISubDomain.INTERNAL, request.SubDomain );
            Assert.IsTrue( request.RequiresCredentials );
            Assert.AreEqual( "/image/moderate", request.RelativePath );
            Assert.AreEqual( "s3-key=images%2fOrionAcct000001%2fswa.jpg", request.QueryString );
        }


        private static UserAuthentication CreateOfflineUserAuthentication() {
            return new UserAuthentication(
                "unit-test@example.com",
                "unit-test-user",
                "refresh-token",
                "access-token",
                "id-token",
                DateTime.UtcNow.AddHours( 1 ),
                DateTime.UtcNow );
        }
    }
}
