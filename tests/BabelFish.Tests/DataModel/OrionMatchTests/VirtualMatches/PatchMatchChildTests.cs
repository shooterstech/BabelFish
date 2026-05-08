using System.Net;
using System.Threading.Tasks;
using System.Linq;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Responses.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.DataModel.OrionMatch.VirtualMatches {

    [TestClass]
    public class PatchMatchChildTests : BaseTestClass {

        //TestDev11 is Admin for OrionAcct000007 and has child owner-id permissions in these tests.
        //TestDev9 is Admin ONLY for OrionAcct000002 and has parent match permissions for the OrionAcct000002 parent.

        private static readonly MatchID ParentMatchId_Request_ParentOwnedByOrionAcct000002 = new MatchID( "1.900.2026041417335583.1" );
        private static readonly MatchID ParentMatchId_Invite_ParentOwnedByOrionAcct000002 = new MatchID( "1.900.2026041417335582.1" );
        private const string OwnerId = "OrionAcct000007";

        private static OrionMatchAPIClient CreateClient() => new OrionMatchAPIClient( APIStage.PRODUCTION ) {
            IgnoreInMemoryCache = true
        };

        private static UserAuthentication CreateUninitializedAuthentication() {
            return new UserAuthentication(
                Constants.TestDev11Credentials.Username,
                Constants.TestDev11Credentials.Password );
        }

        private static string UniqueName( string prefix ) {
            return $"{prefix} {DateTime.UtcNow:yyyyMMddHHmmssfff}";
        }

        private static async Task<UserAuthentication> AuthenticateAsyncChildOwner() {
            var userAuthentication = new UserAuthentication(
                Constants.TestDev11Credentials.Username,
                Constants.TestDev11Credentials.Password );

            await userAuthentication.InitializeAsync();
            return userAuthentication;
        }

        private static async Task<UserAuthentication> AuthenticateAsyncParentOwner() {
            var userAuthentication = new UserAuthentication(
                Constants.TestDev9Credentials.Username,
                Constants.TestDev9Credentials.Password );

            await userAuthentication.InitializeAsync();
            return userAuthentication;
        }

        private static async Task<MatchChild> CreateChildAsync(
            OrionMatchAPIClient client,
            UserAuthentication userAuthentication,
            MatchID parentMatchId,
            string childName ) {

            var response = await client.CreateMatchChildAuthenticatedAsync(
                parentMatchId,
                OwnerId,
                childName,
                userAuthentication );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode, "Child match setup failed." );
            return response.MatchChild;
        }

        private static async Task<PatchMatchChildAuthenticatedResponse> PatchChildAsync(
            OrionMatchAPIClient client,
            UserAuthentication userAuthentication,
            MatchChild matchChild ) {

            var request = new PatchMatchChildAuthenticatedRequest(
                userAuthentication,
                matchChild );

            return await client.PatchMatchChildAuthenticatedAsync( request );
        }

        [TestMethod]
        public async Task PatchMatchChildWhenCallerHasChildPermissionUpdatesName() {
            var client = CreateClient();
            var userAuthentication = await AuthenticateAsyncChildOwner();
            var child = await CreateChildAsync(
                client,
                userAuthentication,
                ParentMatchId_Request_ParentOwnedByOrionAcct000002,
                UniqueName( "BabelFish API Child Patch Name Setup" ) );
            var updatedName = UniqueName( "BabelFish API Child Patch Name Updated" );

            child.Name = updatedName;

            var response = await client.PatchMatchChildAuthenticatedAsync(
                child,
                userAuthentication );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            Assert.IsNotNull( response.MatchChild );
            Assert.AreEqual( updatedName, response.MatchChild.Name );
            Assert.AreEqual( updatedName, response.MatchChild.MatchName );
            Assert.AreEqual( child.MatchID.ToString(), response.MatchChild.MatchID.ToString() );
            Assert.AreEqual( ParentMatchId_Request_ParentOwnedByOrionAcct000002.ToString(), response.MatchChild.ParentID.ToString() );
        }

        [TestMethod]
        public async Task PatchMatchChildWhenCallerHasParentPermissionUpdatesApprovalStatus() {
            var client = CreateClient();
            var parentAuthentication = await AuthenticateAsyncParentOwner();
            var child = await CreateChildAsync(
                client,
                parentAuthentication,
                ParentMatchId_Invite_ParentOwnedByOrionAcct000002,
                UniqueName( "BabelFish API Child Patch Approval Setup" ) );

            child.ApprovalStatus = ApprovalStatus.REJECTED;

            var response = await PatchChildAsync(
                client,
                parentAuthentication,
                child );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            Assert.IsNotNull( response.MatchChild );
            Assert.AreEqual( ApprovalStatus.REJECTED, response.MatchChild.ApprovalStatus );
            Assert.AreEqual( child.MatchID.ToString(), response.MatchChild.MatchID.ToString() );
            Assert.AreEqual( ParentMatchId_Invite_ParentOwnedByOrionAcct000002.ToString(), response.MatchChild.ParentID.ToString() );
        }

        [TestMethod]
        public async Task PatchMatchChildWhenCallerLacksFieldPermissionReturnsWarningAndIgnoresChange() {
            var client = CreateClient();
            var parentAuthentication = await AuthenticateAsyncParentOwner();
            var child = await CreateChildAsync(
                client,
                parentAuthentication,
                ParentMatchId_Invite_ParentOwnedByOrionAcct000002,
                UniqueName( "BabelFish API Child Patch Unauthorized Setup" ) );
            var originalName = child.Name;
            var requestedName = UniqueName( "BabelFish API Child Patch Unauthorized Updated" );

            child.Name = requestedName;

            var response = await PatchChildAsync(
                client,
                parentAuthentication,
                child );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            Assert.IsNotNull( response.MatchChild );
            Assert.AreEqual( originalName, response.MatchChild.Name );
            Assert.AreNotEqual( requestedName, response.MatchChild.Name );
            Assert.IsTrue( response.MessageResponse.Message.Any( x => x.Contains( "Warning: User does not have permissions to edit field Name" ) ) );
        }

        [TestMethod]
        public async Task PatchMatchChildWhenCallerHasChildPermissionOnlyCannotUpdateApprovalStatus() {
            var client = CreateClient();
            var childAuthentication = await AuthenticateAsyncChildOwner();
            var child = await CreateChildAsync(
                client,
                childAuthentication,
                ParentMatchId_Request_ParentOwnedByOrionAcct000002,
                UniqueName( "BabelFish API Child Patch Approval Unauthorized Setup" ) );
            var originalApprovalStatus = child.ApprovalStatus;
            var requestedApprovalStatus = originalApprovalStatus == ApprovalStatus.REJECTED
                ? ApprovalStatus.APPROVED
                : ApprovalStatus.REJECTED;

            child.ApprovalStatus = requestedApprovalStatus;

            var response = await PatchChildAsync(
                client,
                childAuthentication,
                child );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            Assert.IsNotNull( response.MatchChild );
            Assert.AreEqual( originalApprovalStatus, response.MatchChild.ApprovalStatus );
            Assert.AreNotEqual( requestedApprovalStatus, response.MatchChild.ApprovalStatus );
            Assert.IsTrue( response.MessageResponse.Message.Any( x => x.Contains( "Warning: User does not have permissions to edit field ApprovalStatus" ) ) );
        }

        [TestMethod]
        public void PatchMatchChildRequestRequiresParentMatchId() {
            var userAuthentication = CreateUninitializedAuthentication();
            var childMatchId = new MatchID( "1.900.2026041417335583.2" );
            var matchChild = new MatchChild {
                MatchID = childMatchId
            };

            var request = new PatchMatchChildAuthenticatedRequest( userAuthentication, matchChild );

            Assert.Throws<ArgumentNullException>( () => {
                _ = request.RelativePath;
            } );
        }

        [TestMethod]
        public void PatchMatchChildRequestRequiresChildMatchId() {
            var userAuthentication = CreateUninitializedAuthentication();
            var matchChild = new MatchChild {
                ParentID = ParentMatchId_Request_ParentOwnedByOrionAcct000002
            };

            var request = new PatchMatchChildAuthenticatedRequest( userAuthentication, matchChild );

            Assert.Throws<ArgumentNullException>( () => {
                _ = request.RelativePath;
            } );
        }

        [TestMethod]
        public void PatchMatchChildRequestUsesExpectedPathAndBody() {
            var userAuthentication = CreateUninitializedAuthentication();
            var childMatchId = new MatchID( "1.900.2026041417335583.2" );
            var matchChild = new MatchChild {
                MatchID = childMatchId,
                ParentID = ParentMatchId_Request_ParentOwnedByOrionAcct000002,
                OwnerId = OwnerId,
                AccountNumber = OwnerId,
                Name = "BabelFish API Child Patch Request Body Test",
                ApprovalStatus = ApprovalStatus.APPROVED
            };

            var request = new PatchMatchChildAuthenticatedRequest( userAuthentication );
            request.MatchChild = matchChild;
            var json = request.PostParameters.ReadAsStringAsync().GetAwaiter().GetResult();

            Assert.AreEqual(
                $"/match/{ParentMatchId_Request_ParentOwnedByOrionAcct000002}/children/{childMatchId}",
                request.RelativePath );
            Assert.AreEqual( APISubDomain.AUTHAPI, request.SubDomain );
            Assert.IsTrue( json.Contains( "\"MatchID\"" ) );
            Assert.IsTrue( json.Contains( childMatchId.ToString() ) );
            Assert.IsTrue( json.Contains( "\"Name\"" ) );
            Assert.IsTrue( json.Contains( matchChild.Name ) );
        }
    }
}
