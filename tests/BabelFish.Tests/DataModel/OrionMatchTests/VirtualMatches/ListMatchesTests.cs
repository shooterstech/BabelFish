using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Responses.OrionMatchAPI;
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

        private static void AssertEveryMatchHasMatchingCourseOfFire( ListMatchesAbstractResponse response, Func<CourseOfFireStructureAbbr, bool> predicate ) {
            Assert.IsNotNull( response.MatchList );
            Assert.IsTrue( response.MatchList.Items.Count > 0 );
            Assert.IsTrue(
                response.MatchList.Items.All( x => x.CoursesOfFire.Any( predicate ) ),
                "Expected every returned match to include at least one matching course of fire." );
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
                Limit = 200,
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
        public async Task ListMatchesPublicDisciplineFilterReturnsOnlyMatchingCoursesOfFire() {
            var client = CreateClient();
            var request = new ListMatchesPublicRequest() {
                Discipline = DisciplineType.RIFLE,
                Limit = 10,
                IgnoreInMemoryCache = true
            };

            var response = await client.ListMatchesPublicAsync( request );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            AssertEveryMatchHasMatchingCourseOfFire( response, cof => cof.Discipline == DisciplineType.RIFLE );
        }

        [TestMethod]
        public async Task ListMatchesPublicCourseOfFireDefinitionFilterReturnsOnlyMatchingCoursesOfFire() {
            var client = CreateClient();
            var courseOfFireDefinition = SetName.Parse( "v1.0:usas:Air Pistol 60 Shots", true );
            var request = new ListMatchesPublicRequest() {
                MatchTypeFilter = "PARENT",
                CourseOfFireDefinition = courseOfFireDefinition,
                Limit = 10,
                IgnoreInMemoryCache = true
            };

            var response = await client.ListMatchesPublicAsync( request );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            AssertEveryMatchHasMatchingCourseOfFire( response, cof => cof.CourseOfFireDef.Equals( courseOfFireDefinition ) );
        }

        [TestMethod]
        public async Task ListMatchesPublicTargetCollectionFilterReturnsOnlyMatchingCoursesOfFire() {
            var client = CreateClient();
            var request = new ListMatchesPublicRequest() {
                MatchTypeFilter = "PARENT",
                TargetCollectionName = "10m Air Rifle",
                Limit = 10,
                IgnoreInMemoryCache = true
            };

            var response = await client.ListMatchesPublicAsync( request );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            AssertEveryMatchHasMatchingCourseOfFire( response, cof => cof.TargetCollectionName == "10m Air Rifle" );
        }

        [TestMethod]
        public async Task ListMatchesPublicHistoricalDateWindowReturnsMatchesWithinRequestedWindow() {
            var client = CreateClient();
            var startDate = new DateTime( 2022, 1, 1 );
            var endDate = new DateTime( 2022, 12, 31 );
            var request = new ListMatchesPublicRequest() {
                StartDate = startDate,
                EndDate = endDate,
                Limit = 10,
                IgnoreInMemoryCache = true
            };

            var response = await client.ListMatchesPublicAsync( request );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            Assert.IsNotNull( response.MatchList );
            Assert.IsTrue( response.MatchList.Items.Count > 0 );
            Assert.IsTrue( response.MatchList.Items.All( x => x.StartDate >= startDate ) );
            Assert.IsTrue( response.MatchList.Items.All( x => x.EndDate <= endDate ) );
        }

        [TestMethod]
        public async Task ListMatchesPublicInvalidMatchTypeReturnsBadRequest() {
            var client = CreateClient();
            var request = new ListMatchesPublicRequest() {
                MatchTypeFilter = "BOTH",
                Limit = 10,
                IgnoreInMemoryCache = true
            };

            var response = await client.ListMatchesPublicAsync( request );

            Assert.AreEqual( HttpStatusCode.BadRequest, response.RestApiStatusCode );
            Assert.IsTrue( response.MessageResponse.Message.Any( x => x.Contains( "Invalid match-type", StringComparison.OrdinalIgnoreCase ) ) );
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
                Limit = 200,
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
