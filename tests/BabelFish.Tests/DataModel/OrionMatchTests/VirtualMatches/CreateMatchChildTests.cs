using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Responses.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.OrionMatch.VirtualMatches {

    [TestClass]
    public class CreateMatchChildTests : BaseTestClass {

        private static readonly MatchID ParentMatchId = new MatchID("1.900.2026041417335579.1");
        private const string OwnerId = "OrionAcct000007";

        private static OrionMatchAPIClient CreateClient() => new OrionMatchAPIClient(APIStage.PRODUCTION);

        private static string UniqueName(string prefix) {
            return $"{prefix} {DateTime.UtcNow:yyyyMMddHHmmssfff}";
        }

        private static async Task<UserAuthentication> AuthenticateAsync() {
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password);

            await userAuthentication.InitializeAsync();
            return userAuthentication;
        }

        [TestMethod]
        public async Task CreateMatchChildWithRequestCreatesChildMatch() {
            var client = CreateClient();
            var userAuthentication = await AuthenticateAsync();
            var childName = UniqueName("BabelFish API Child Request Test");

            var request = new CreateMatchChildAuthenticatedRequest(
                userAuthentication,
                ParentMatchId,
                OwnerId,
                childName);

            var response = await client.CreateMatchChildAuthenticatedAsync(request);

            Assert.AreEqual(HttpStatusCode.OK, response.RestApiStatusCode);
            Assert.IsNotNull(response.MatchChild);
            Assert.AreEqual(ParentMatchId.ToString(), response.MatchChild.ParentID.ToString());
            Assert.AreEqual(OwnerId, response.MatchChild.OwnerId);
            Assert.AreEqual(OwnerId, response.MatchChild.AccountNumber);
            Assert.AreEqual(childName, response.MatchChild.Name);
            Assert.AreEqual(childName, response.MatchChild.MatchName);
            Assert.AreEqual(response.MatchChild.MatchID.ToString(), response.MatchChild.MATCH_MatchID.ToString());
            Assert.AreEqual($"{response.MatchChild.MatchID}:M", response.MatchChild.UniqueID);
            Assert.IsTrue(response.MatchChild.MatchID.VirtualMatchChild);
            Assert.IsTrue(
                response.MatchChild.ApprovalStatus == ApprovalStatus.APPROVED
                || response.MatchChild.ApprovalStatus == ApprovalStatus.PENDING);
        }
    }

}
