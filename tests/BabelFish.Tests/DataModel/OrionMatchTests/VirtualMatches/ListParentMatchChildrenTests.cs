using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.OrionMatch.VirtualMatches {

    [TestClass]
    public class ListParentMatchChildrenTests : BaseTestClass {

        private static readonly MatchID ParentMatchId_Invite = new MatchID( "1.900.2026041417335579.1" ); //ownerid OrionAcct000007
        private static readonly MatchID ParentMatchId_Request = new MatchID( "1.900.2026041417335580.1" ); //ownerid OrionAcct000007
        private static readonly MatchID ParentMatchId_Open = new MatchID( "1.900.2026041417335581.1" ); //ownerid OrionAcct000007
        private const string OwnerId = "OrionAcct000007";

        private static OrionMatchAPIClient CreateClient() {
            return new OrionMatchAPIClient( APIStage.PRODUCTION ) {
                IgnoreInMemoryCache = true
            };
        }

        private static string UniqueName( string prefix ) {
            return $"{prefix} {DateTime.UtcNow:yyyyMMddHHmmssfff}";
        }

        private static async Task<UserAuthentication> AuthenticateAsync() { //user with permissions for OrionAcct000007
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );

            await userAuthentication.InitializeAsync();
            return userAuthentication;
        }

        private static async Task<UserAuthentication> AuthenticateAsyncNoPermissions() { //user with no permissions
            var userAuthentication = new UserAuthentication(
                Constants.TestDev13Credentials.Username,
                Constants.TestDev13Credentials.Password );

            await userAuthentication.InitializeAsync();
            return userAuthentication;
        }

        private static async Task<MatchChild> CreateChildAsync( OrionMatchAPIClient client, UserAuthentication userAuthentication, string name ) {
            var response = await client.CreateMatchChildAuthenticatedAsync(
                ParentMatchId_Invite,
                OwnerId,
                name,
                userAuthentication );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode, "Child match setup failed." );
            return response.MatchChild;
        }

        [TestMethod]
        public async Task ListParentMatchChildrenWithConvenienceOverloadReturnsChildren() {
            var client = CreateClient();
            var userAuthentication = await AuthenticateAsync();
            var createdChild = await CreateChildAsync(
                client,
                userAuthentication,
                UniqueName( "BabelFish API Child List Overload Test" ) );

            var response = await client.ListParentMatchChildrenAuthenticatedAsync( ParentMatchId_Invite, userAuthentication );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            Assert.IsNotNull( response.MatchChildList );
            Assert.IsTrue( response.MatchChildList.Items.Count > 0 );
            Assert.IsTrue( response.MatchChildList.Items.Any( x => x.MatchID.Equals( createdChild.MatchID ) ) );
            Assert.IsTrue( response.MatchChildList.Items.All( x => x.ParentID.Equals( ParentMatchId_Invite ) ) );
        }

        [TestMethod]
        public async Task ListParentMatchChildrenTokenizedRequestReturnsNextRequest() {
            var client = CreateClient();
            var userAuthentication = await AuthenticateAsync();

            await CreateChildAsync( client, userAuthentication, UniqueName( "BabelFish API Child Token Test A" ) );
            await CreateChildAsync( client, userAuthentication, UniqueName( "BabelFish API Child Token Test B" ) );

            var request = new ListParentMatchChildrenAuthenticatedRequest( userAuthentication, ParentMatchId_Invite ) {
                Limit = 1,
                IgnoreInMemoryCache = true
            };

            var firstResponse = await client.ListParentMatchChildrenAuthenticatedAsync( request );

            Assert.AreEqual( HttpStatusCode.OK, firstResponse.RestApiStatusCode );
            Assert.AreEqual( 1, firstResponse.MatchChildList.Items.Count );
            Assert.IsTrue( firstResponse.HasMoreItems );

            var nextRequest = firstResponse.GetNextRequest();
            Assert.AreEqual( 1, nextRequest.Limit );
            Assert.IsFalse( string.IsNullOrWhiteSpace( nextRequest.Token ) );

            var secondResponse = await client.ListParentMatchChildrenAuthenticatedAsync( nextRequest );

            Assert.AreEqual( HttpStatusCode.OK, secondResponse.RestApiStatusCode );
            Assert.AreEqual( 1, secondResponse.MatchChildList.Items.Count );
            Assert.AreNotEqual(
                firstResponse.MatchChildList.Items[0].MatchID.ToString(),
                secondResponse.MatchChildList.Items[0].MatchID.ToString() );
        }
    }
}
