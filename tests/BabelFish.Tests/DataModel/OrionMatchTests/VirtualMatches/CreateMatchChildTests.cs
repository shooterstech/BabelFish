using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Responses.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.DataModel.OrionMatch.VirtualMatches {

    [TestClass]
    public class CreateMatchChildTests : BaseTestClass {

        //TestDev11 is Admin for OrionAcct000007 and OrionAcct000001
        //TestDev13 has no permissions for any accounts
        //TestDev9 is Admin ONLY for OrionAcct000002


        private static readonly MatchID ParentMatchId_Invite = new MatchID("1.900.2026041417335579.1");
        private static readonly MatchID ParentMatchId_Request = new MatchID( "1.900.2026041417335580.1" ); // ownerid OrionAcct000007
        private static readonly MatchID ParentMatchId_Open = new MatchID( "1.900.2026041417335581.1" ); //  ownerid OrionAcct000007

        // The below parent matches should be owned by OrionAcct000002, so TestDev9 has parent match permission
        // and TestDev11 has child owner-id permission only when the child owner-id is OrionAcct000007.
        private static readonly MatchID ParentMatchId_Invite_ParentOwnedByOrionAcct000002 = new MatchID( "1.900.2026041417335582.1" ); // MemberPolicy INVITE
        private static readonly MatchID ParentMatchId_Request_ParentOwnedByOrionAcct000002 = new MatchID( "1.900.2026041417335583.1" ); // MemberPolicy REQUEST
        private static readonly MatchID ParentMatchId_Open_ParentOwnedByOrionAcct000002 = new MatchID( "1.900.2026041417335584.1" ); // MemberPolicy OPEN

        private const string OwnerId = "OrionAcct000007";

        private static OrionMatchAPIClient CreateClient() => new OrionMatchAPIClient(APIStage.PRODUCTION);

        private static UserAuthentication CreateUninitializedAuthentication() {
            return new UserAuthentication(
                Constants.TestDev11Credentials.Username,
                Constants.TestDev11Credentials.Password );
        }

        private static string UniqueName(string prefix) {
            return $"{prefix} {DateTime.UtcNow:yyyyMMddHHmmssfff}";
        }

        private static async Task<UserAuthentication> AuthenticateAsync() { //user with permissions for OrionAcct000007
            var userAuthentication = new UserAuthentication(
                Constants.TestDev11Credentials.Username,
                Constants.TestDev11Credentials.Password);

            await userAuthentication.InitializeAsync();
            return userAuthentication;
        }

        private static async Task<UserAuthentication> AuthenticateAsyncParentOnly() { //user with permissions for OrionAcct000002 only
            var userAuthentication = new UserAuthentication(
                Constants.TestDev9Credentials.Username,
                Constants.TestDev9Credentials.Password );

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

        private static async Task<CreateMatchChildAuthenticatedResponse> CreateChildAsync(
            OrionMatchAPIClient client,
            UserAuthentication userAuthentication,
            MatchID parentMatchId,
            string ownerId,
            string childName ) {

            var request = new CreateMatchChildAuthenticatedRequest(
                userAuthentication,
                parentMatchId,
                ownerId,
                childName );

            return await client.CreateMatchChildAuthenticatedAsync( request );
        }

        private static void AssertSuccessfulChildMatch(
            CreateMatchChildAuthenticatedResponse response,
            MatchID expectedParentMatchId,
            string expectedOwnerId,
            string expectedChildName ) {

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            Assert.IsNotNull( response.MatchChild );
            Assert.AreEqual( expectedParentMatchId.ToString(), response.MatchChild.ParentID.ToString() );
            Assert.AreEqual( expectedOwnerId, response.MatchChild.OwnerId );
            Assert.AreEqual( expectedOwnerId, response.MatchChild.AccountNumber );
            Assert.AreEqual( expectedChildName, response.MatchChild.Name );
            Assert.AreEqual( expectedChildName, response.MatchChild.MatchName );
            Assert.AreEqual( response.MatchChild.MatchID.ToString(), response.MatchChild.MATCH_MatchID.ToString() );
            Assert.AreEqual( $"{response.MatchChild.MatchID}:M", response.MatchChild.UniqueID );
            Assert.IsTrue( response.MatchChild.MatchID.VirtualMatchChild );
            Assert.IsTrue(
                response.MatchChild.ApprovalStatus == ApprovalStatus.APPROVED
                || response.MatchChild.ApprovalStatus == ApprovalStatus.PENDING );
        }

        [TestMethod]
        public async Task CreateMatchChildForInvitePolicyWhenCallerHasParentAndOwnerPermissionsReturnsChildMatch() {
            // Intention: verify INVITE policy succeeds and returns child match details when caller has both parent and owner-id permissions.
            var client = CreateClient();
            var userAuthentication = await AuthenticateAsync();
            var childName = UniqueName("BabelFish API Child Invite Both Permissions Test");

            var response = await CreateChildAsync(
                client,
                userAuthentication,
                ParentMatchId_Invite,
                OwnerId,
                childName );

            AssertSuccessfulChildMatch( response, ParentMatchId_Invite, OwnerId, childName );
        }

        [TestMethod]
        public async Task CreateMatchChildForInvitePolicyWhenCallerHasOwnerPermissionOnlyReturnsForbidden() {
            // Intention: verify INVITE policy rejects a caller who lacks parent match permission even when they have owner-id permission.
            var client = CreateClient();
            var userAuthentication = await AuthenticateAsync();
            var childName = UniqueName( "BabelFish API Child Invite Owner Only Test" );

            var response = await CreateChildAsync(
                client,
                userAuthentication,
                ParentMatchId_Invite_ParentOwnedByOrionAcct000002,
                OwnerId,
                childName );

            Assert.AreEqual( HttpStatusCode.Unauthorized, response.RestApiStatusCode );
        }

        [TestMethod]
        public async Task CreateMatchChildForInvitePolicyWhenCallerHasParentPermissionOnlyReturnsChildMatch() {
            // Intention: verify INVITE policy succeeds and returns child match details when caller has parent match permission but lacks owner-id permission.
            var client = CreateClient();
            var userAuthentication = await AuthenticateAsyncParentOnly();
            var childName = UniqueName( "BabelFish API Child Invite Parent Only Test" );

            var response = await CreateChildAsync(
                client,
                userAuthentication,
                ParentMatchId_Invite_ParentOwnedByOrionAcct000002,
                OwnerId,
                childName );

            AssertSuccessfulChildMatch( response, ParentMatchId_Invite_ParentOwnedByOrionAcct000002, OwnerId, childName );
        }

        [TestMethod]
        public async Task CreateMatchChildForRequestPolicyWhenCallerHasParentPermissionOnlyReturnsChildMatch() {
            // Intention: verify REQUEST policy succeeds and returns child match details when caller has parent match permission but lacks owner-id permission.
            var client = CreateClient();
            var userAuthentication = await AuthenticateAsyncParentOnly();
            var childName = UniqueName( "BabelFish API Child Request Parent Only Test" );

            var response = await CreateChildAsync(
                client,
                userAuthentication,
                ParentMatchId_Request_ParentOwnedByOrionAcct000002,
                OwnerId,
                childName );

            AssertSuccessfulChildMatch( response, ParentMatchId_Request_ParentOwnedByOrionAcct000002, OwnerId, childName );
        }

        [TestMethod]
        public async Task CreateMatchChildForRequestPolicyWhenCallerHasOwnerPermissionOnlyReturnsChildMatch() {
            // Intention: verify REQUEST policy succeeds and returns child match details when caller lacks parent match permission but has owner-id permission.
            var client = CreateClient();
            var userAuthentication = await AuthenticateAsync();
            var childName = UniqueName( "BabelFish API Child Request Owner Only Test" );

            var response = await CreateChildAsync(
                client,
                userAuthentication,
                ParentMatchId_Request_ParentOwnedByOrionAcct000002,
                OwnerId,
                childName );

            AssertSuccessfulChildMatch( response, ParentMatchId_Request_ParentOwnedByOrionAcct000002, OwnerId, childName );
        }

        [TestMethod]
        public async Task CreateMatchChildForRequestPolicyWhenCallerHasNoPermissionsReturnsForbidden() {
            // Intention: verify REQUEST policy rejects a caller who lacks both parent match permission and owner-id permission.
            var client = CreateClient();
            var userAuthentication = await AuthenticateAsyncNoPermissions();
            var childName = UniqueName( "BabelFish API Child Request No Permissions Test" );

            var response = await CreateChildAsync(
                client,
                userAuthentication,
                ParentMatchId_Request,
                OwnerId,
                childName );

            Assert.AreEqual( HttpStatusCode.Unauthorized, response.RestApiStatusCode );
        }

        [TestMethod]
        public async Task CreateMatchChildForOpenPolicyWhenCallerHasNoPermissionsReturnsForbidden() {
            // Intention: verify OPEN policy rejects a caller who lacks both parent match permission and owner-id permission.
            var client = CreateClient();
            var userAuthentication = await AuthenticateAsyncNoPermissions();
            var childName = UniqueName( "BabelFish API Child Open No Permissions Test" );

            var response = await CreateChildAsync(
                client,
                userAuthentication,
                ParentMatchId_Open,
                OwnerId,
                childName );

            Assert.AreEqual( HttpStatusCode.Unauthorized, response.RestApiStatusCode );
        }

        [TestMethod]
        public async Task CreateMatchChildForOpenPolicyWhenCallerHasOwnerPermissionOnlyReturnsChildMatch() {
            // Intention: verify OPEN policy succeeds and returns child match details when caller lacks parent match permission but has owner-id permission.
            var client = CreateClient();
            var userAuthentication = await AuthenticateAsync();
            var childName = UniqueName( "BabelFish API Child Open Owner Only Test" );

            var response = await CreateChildAsync(
                client,
                userAuthentication,
                ParentMatchId_Open_ParentOwnedByOrionAcct000002,
                OwnerId,
                childName );

            AssertSuccessfulChildMatch( response, ParentMatchId_Open_ParentOwnedByOrionAcct000002, OwnerId, childName );
        }

        /*
         * Additional request validation tests.
         */

        [TestMethod]
        public void CreateMatchChildRequestRequiresParentMatchId() {
            // Intention: verify request construction rejects a missing parent match id before an API call is attempted.
            var userAuthentication = CreateUninitializedAuthentication();
            MatchID nullParentMatchId = null!;

            Assert.ThrowsException<ArgumentNullException>( () => {
                _ = new CreateMatchChildAuthenticatedRequest(
                    userAuthentication,
                    nullParentMatchId,
                    OwnerId,
                    UniqueName( "BabelFish API Child Null Parent Test" ) );
            } );
        }

        [TestMethod]
        public void CreateMatchChildRequestRequiresOwnerId() {
            // Intention: verify request query parameter generation rejects a blank owner id before an API call is attempted.
            var userAuthentication = CreateUninitializedAuthentication();

            var request = new CreateMatchChildAuthenticatedRequest(
                userAuthentication,
                ParentMatchId_Invite,
                " ",
                UniqueName( "BabelFish API Child Blank Owner Test" ) );

            Assert.ThrowsException<ArgumentNullException>( () => {
                _ = request.QueryParameters;
            } );
        }

        [TestMethod]
        public void CreateMatchChildRequestRequiresName() {
            // Intention: verify request query parameter generation rejects a blank child match name before an API call is attempted.
            var userAuthentication = CreateUninitializedAuthentication();

            var request = new CreateMatchChildAuthenticatedRequest(
                userAuthentication,
                ParentMatchId_Invite,
                OwnerId,
                " " );

            Assert.ThrowsException<ArgumentNullException>( () => {
                _ = request.QueryParameters;
            } );
        }

    }

}
