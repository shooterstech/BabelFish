using System.Net;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Tests.DataModel.OrionMatch.VirtualMatches {

    [TestClass]
    public class PostMatchParentTests : BaseTestClass {

        private const string OwnerId = "OrionAcct000007";

        private static OrionMatchAPIClient CreateClient() => new OrionMatchAPIClient( APIStage.PRODUCTION ) {
            IgnoreInMemoryCache = true
        };

        private static UserAuthentication CreateUninitializedAuthentication() {
            return new UserAuthentication(
                Constants.TestDev11Credentials.Username,
                Constants.TestDev11Credentials.Password );
        }

        private static async Task<UserAuthentication> AuthenticateAsync() {
            var userAuthentication = new UserAuthentication(
                Constants.TestDev11Credentials.Username,
                Constants.TestDev11Credentials.Password );

            await userAuthentication.InitializeAsync();
            return userAuthentication;
        }

        private static Match CreateParentMatch( string name ) {
            var today = DateTime.UtcNow.Date;
            var match = new Match {
                MatchID = new MatchID( 1, 900, MatchID.SUBMATCHID_VIRTUAL_PARENT ),
                Name = name,
                OwnerId = OwnerId,
                MemberPolicy = MemberPolicyOption.INVITE,
                Visibility = VisibilityOption.PRIVATE,
                MatchType = CompetitionTypeOptions.LOCAL_MATCH,
                ResultURL = string.Empty,
                SharedKey = string.Empty,
                JSONVersion = Scopos.BabelFish.Helpers.Common.DATA_MODEL_VERSION,
                LastUpdated = DateTime.UtcNow
            };

            match.MatchStructure.CoursesOfFire.Add( new CourseOfFireStructure {
                CourseOfFireId = 1,
                CourseOfFireName = "3x10 Air Rifle",
                CourseOfFireDef = SetName.Parse( "v3.0:ntparc:Three-Position Air Rifle 3x10" ),
                StartDate = today,
                EndDate = today,
                ScoreConfigName = "Integer",
                TargetCollectionName = "10m Air Rifle",
                TypesOfEntries = EntryTypes.INDIVIDUAL_AND_TEAM
            } );

            return match;
        }

        private static string UniqueName( string prefix ) {
            return $"{prefix} {DateTime.UtcNow:yyyyMMddHHmmssfff}";
        }

        [TestMethod]
        public async Task PostMatchParentAuthenticatedReturnsUploadDetails() {
            var client = CreateClient();
            var userAuthentication = await AuthenticateAsync();
            var match = CreateParentMatch( UniqueName( "BabelFish API Parent Post Test" ) );

            var response = await client.PostMatchParentAuthenticatedAsync(
                match,
                userAuthentication );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            Assert.IsNotNull( response.PostMatchParent );
            Assert.AreEqual( match.MatchID.ToString(), response.PostMatchParent.MatchID.ToString() );
            Assert.IsFalse( string.IsNullOrWhiteSpace( response.PostMatchParent.S3Key ) );
            Assert.IsTrue( response.PostMatchParent.S3Key.Contains( match.MatchID.ToString() ) );
        }

        [TestMethod]
        public void PostMatchParentRequestUsesExpectedPathSubDomainAndBody() {
            var userAuthentication = CreateUninitializedAuthentication();
            var match = CreateParentMatch( "BabelFish API Parent Post Request Body Test" );
            var request = new PostMatchParentAuthenticatedRequest( userAuthentication, match );

            var json = request.PostParameters.ReadAsStringAsync().GetAwaiter().GetResult();

            Assert.AreEqual( "/match", request.RelativePath );
            Assert.AreEqual( APISubDomain.AUTHAPI, request.SubDomain );
            Assert.IsTrue( json.Contains( "\"MatchID\"" ) );
            Assert.IsTrue( json.Contains( match.MatchID.ToString() ) );
            Assert.IsTrue( json.Contains( "\"ParentID\"" ) );
            Assert.IsTrue( json.Contains( "\"OwnerId\"" ) );
            Assert.IsTrue( json.Contains( OwnerId ) );
            Assert.IsTrue( json.Contains( "\"CheckSum\"" ) );
        }

        [TestMethod]
        public void PostMatchParentRequestRequiresMatch() {
            var userAuthentication = CreateUninitializedAuthentication();
            Match nullMatch = null!;

            Assert.Throws<ArgumentNullException>( () => {
                _ = new PostMatchParentAuthenticatedRequest( userAuthentication, nullMatch );
            } );
        }

        [TestMethod]
        public void PostMatchParentRequestRequiresParentMatchId() {
            var userAuthentication = CreateUninitializedAuthentication();
            var match = CreateParentMatch( "BabelFish API Parent Post Child Id Test" );
            match.MatchID = new MatchID( "1.900.2026041417335579.1000" );
            var request = new PostMatchParentAuthenticatedRequest( userAuthentication, match );

            Assert.Throws<ArgumentException>( () => {
                _ = request.PostParameters;
            } );
        }

        [TestMethod]
        public void PostMatchParentRequestRequiresOwnerId() {
            var userAuthentication = CreateUninitializedAuthentication();
            var match = CreateParentMatch( "BabelFish API Parent Post Blank Owner Test" );
            match.OwnerId = " ";
            var request = new PostMatchParentAuthenticatedRequest( userAuthentication, match );

            Assert.Throws<ArgumentNullException>( () => {
                _ = request.PostParameters;
            } );
        }
    }
}
