using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.Athena;
using Scopos.BabelFish.Runtime.Authentication;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Tests.AthenaOwner {
    [TestClass]
    public class AthenaOwnerTests : BaseTestClass {
        private AthenaAPIClient athenaOwnerClient;

        [TestMethod]
        public async Task TestGetUserOwnershipValues() {
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            var getRequest = new GetUserOwnershipValuesAuthenticatedRequest( userAuthentication );
            var getResponse = await athenaOwnerClient.GetUserOwnershipValuesAuthenticatedAsync( getRequest );

            Assert.AreEqual( System.Net.HttpStatusCode.OK, getResponse.StatusCode );
            Assert.AreEqual( getResponse.AthenaOwnerValues.OwnerId, "AtHomeAcct000004" );

        }
    }
}
