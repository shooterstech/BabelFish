using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.DataModel.OrionMatch.VirtualMatches {

    [TestClass]
    public class ListMatchesTests : BaseTestClass {

        private static readonly MatchID ParentMatchId_Invite = new MatchID( "1.900.2026041417335579.1" );
        private static readonly MatchID ParentMatchId_Open = new MatchID( "1.900.2026041417335581.1" );
        private const string OwnerId = "OrionAcct000007";

        private static OrionMatchAPIClient CreateClient() {
            return new OrionMatchAPIClient( APIStage.PRODUCTION ) {
                IgnoreInMemoryCache = true
            };
        }

        private static string UniqueName( string prefix ) {
            return $"{prefix} {DateTime.UtcNow:yyyyMMddHHmmssfff}";
        }

        private static async Task<UserAuthentication> AuthenticateAsync() {
            var userAuthentication = new UserAuthentication(
                Constants.TestDev11Credentials.Username,
                Constants.TestDev11Credentials.Password );

            await userAuthentication.InitializeAsync();
            return userAuthentication;
        }

        private static async Task<MatchChild> CreateChildAsync( OrionMatchAPIClient client, UserAuthentication userAuthentication, string childName ) {
            var response = await client.CreateMatchChildAuthenticatedAsync(
                ParentMatchId_Invite,
                OwnerId,
                childName,
                userAuthentication );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode, "Child match setup failed." );
            Assert.IsNotNull( response.MatchChild );
            return response.MatchChild;
        }

        [TestMethod]
        public async Task ListMatchesAuthenticatedReturnsParentAndCreatedChildForParentFilter() {
            var client = CreateClient();
            var userAuthentication = await AuthenticateAsync();
            var createdChild = await CreateChildAsync( client, userAuthentication, UniqueName( "BabelFish API ListMatches Parent Filter Test" ) );

            var request = new ListMatchesAuthenticatedRequest( userAuthentication ) {
                ParentMatchId = ParentMatchId_Invite,
                OwnerId = OwnerId,
                Limit = 25,
                IgnoreInMemoryCache = true
            };

            var response = await client.ListMatchesAuthenticatedAsync( request );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            Assert.IsNotNull( response.MatchList );
            Assert.IsTrue( response.MatchList.TotalCount >= response.MatchList.Items.Count );
            Assert.IsTrue( response.MatchList.Items.Any( x => x.MatchID.Equals( ParentMatchId_Invite ) ) );
            Assert.IsTrue( response.MatchList.Items.Any( x => x.MatchID.Equals( createdChild.MatchID ) ) );
        }

        [TestMethod]
        public async Task ListMatchesPublicReturnsKnownPublicParentMatch() {
            var client = CreateClient();
            var request = new ListMatchesPublicRequest() {
                ParentMatchId = ParentMatchId_Open,
                MatchTypeFilter = "PARENT",
                Visibility = VisibilityOption.PUBLIC,
                Limit = 10,
                IgnoreInMemoryCache = true
            };

            var response = await client.ListMatchesPublicAsync( request );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            Assert.IsNotNull( response.MatchList );
            Assert.IsTrue( response.MatchList.Items.Count > 0 );
            Assert.IsTrue( response.MatchList.Items.Any( x => x.MatchID.Equals( ParentMatchId_Open ) ) );
        }

        [TestMethod]
        public async Task ListMatchesAuthenticatedChildFilterReturnsCreatedChildAndNotParentMatch() {
            var client = CreateClient();
            var userAuthentication = await AuthenticateAsync();
            var createdChild = await CreateChildAsync( client, userAuthentication, UniqueName( "BabelFish API ListMatches Child Filter Test" ) );

            var request = new ListMatchesAuthenticatedRequest( userAuthentication ) {
                ParentMatchId = ParentMatchId_Invite,
                OwnerId = OwnerId,
                MatchTypeFilter = "child",
                MemberPolicy = MemberPolicyOption.INVITE,
                ApprovalStatus = createdChild.ApprovalStatus,
                Limit = 25,
                IgnoreInMemoryCache = true
            };

            var response = await client.ListMatchesAuthenticatedAsync( request );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            Assert.IsNotNull( response.MatchList );
            Assert.IsTrue( response.MatchList.Items.Count > 0 );
            Assert.IsTrue( response.MatchList.Items.Any( x => x.MatchID.Equals( createdChild.MatchID ) ) );
            Assert.IsFalse( response.MatchList.Items.Any( x => x.MatchID.Equals( ParentMatchId_Invite ) ) );
        }

        [TestMethod]
        public async Task ListMatchesAuthenticatedTokenizedRequestReturnsNextRequest() {
            var client = CreateClient();
            var userAuthentication = await AuthenticateAsync();

            await CreateChildAsync( client, userAuthentication, UniqueName( "BabelFish API ListMatches Token Test A" ) );
            await CreateChildAsync( client, userAuthentication, UniqueName( "BabelFish API ListMatches Token Test B" ) );

            var request = new ListMatchesAuthenticatedRequest( userAuthentication ) {
                ParentMatchId = ParentMatchId_Invite,
                OwnerId = OwnerId,
                MatchTypeFilter = "CHILD",
                Limit = 1,
                IgnoreInMemoryCache = true
            };

            var firstResponse = await client.ListMatchesAuthenticatedAsync( request );

            Assert.AreEqual( HttpStatusCode.OK, firstResponse.RestApiStatusCode );
            Assert.AreEqual( 1, firstResponse.MatchList.Items.Count );
            Assert.IsTrue( firstResponse.HasMoreItems );

            var nextRequest = firstResponse.GetNextRequest();
            Assert.AreEqual( 1, nextRequest.Limit );
            Assert.IsFalse( string.IsNullOrWhiteSpace( nextRequest.Token ) );

            var secondResponse = await client.ListMatchesAuthenticatedAsync( nextRequest );

            Assert.AreEqual( HttpStatusCode.OK, secondResponse.RestApiStatusCode );
            Assert.AreEqual( 1, secondResponse.MatchList.Items.Count );
            Assert.AreNotEqual(
                firstResponse.MatchList.Items[0].MatchID.ToString(),
                secondResponse.MatchList.Items[0].MatchID.ToString() );
        }
    }
}
