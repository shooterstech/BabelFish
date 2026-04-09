using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.ResultListMerger;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.DataModel.OrionMatch.Tournament {
    [TestClass]
    public class TournamentMergedResultTests : BaseTestClass {

        private const string TournamentOwnerId = "OrionAcct000002";
        private static readonly MatchID KnownPublicMatchOwner1BothSides = new MatchID( "1.1.2021020310584218.1" );

        private static OrionMatchAPIClient CreateClient() => new OrionMatchAPIClient( APIStage.PRODUCTION );

        private static string UniqueName( string prefix ) {
            return $"{prefix} {DateTime.UtcNow:yyyyMMddHHmmssfff}";
        }

        private static async Task<UserAuthentication> AuthenticateAsync( BasicUserCredentials credentials ) {
            var userAuthentication = new UserAuthentication( credentials.Username, credentials.Password );
            await userAuthentication.InitializeAsync();
            return userAuthentication;
        }

        private static async Task TryDeleteTournamentAsync( OrionMatchAPIClient client, MatchID? tournamentId, UserAuthentication credentials ) {
            if (tournamentId == null) {
                return;
            }

            try {
                await client.DeleteTournamentAuthenticatedAsync( tournamentId, credentials );
            } catch {
                // Best-effort cleanup only.
            }
        }

        private static async Task<MatchID> CreateTournamentAsync( OrionMatchAPIClient client, UserAuthentication credentials, string name ) {
            var request = new CreateTournamentAuthenticatedRequest( credentials ) {
                TournamentName = name,
                OwnerId = TournamentOwnerId,
                Visibility = VisibilityOption.PUBLIC,
                ShowOnSearch = false,
                MemberPolicy = MemberPolicyOption.INVITE
            };

            var response = await client.CreateTournamentAuthenticatedAsync( request );
            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode, "Tournament setup failed for merged result list test." );
            return response.Tournament.TournamentId;
        }

        private static MergedResultList CreateMergedResultListModel( string resultName ) {
            return new MergedResultList() {
                ResultName = resultName,
                Method = MergeMethodType.SUM,
                Configuration = new SumMethodConfiguration()
            };
        }

        private static async Task<ResultListMember> CreateKnownApprovedResultListMemberAsync( OrionMatchAPIClient client, MatchID tournamentId, UserAuthentication credentials ) {
            var addTournamentMemberResponse = await client.AddTournamentMemberAuthenticatedAsync( tournamentId, KnownPublicMatchOwner1BothSides, credentials );
            Assert.AreEqual( HttpStatusCode.OK, addTournamentMemberResponse.RestApiStatusCode, "Tournament member setup failed for merged result list test." );

            if (addTournamentMemberResponse.TournamentMember.ApprovalStatus != ApprovalStatus.APPROVED) {
                var patchRequest = new PatchTournamentMemberAuthenticatedRequest( credentials ) {
                    TournamentId = tournamentId,
                    MatchId = KnownPublicMatchOwner1BothSides,
                    ApprovalStatus = ApprovalStatus.APPROVED
                };

                var patchResponse = await client.PatchTournamentMemberAuthenticatedAsync( patchRequest );
                Assert.AreEqual( HttpStatusCode.OK, patchResponse.RestApiStatusCode, "Tournament member approval setup failed for merged result list test." );
                Assert.AreEqual( ApprovalStatus.APPROVED, patchResponse.TournamentMember.ApprovalStatus );
            }

            var matchResponse = await client.GetMatchPublicAsync( KnownPublicMatchOwner1BothSides );
            Assert.AreEqual( HttpStatusCode.OK, matchResponse.RestApiStatusCode, "Known member match lookup failed for merged result list test." );

            var resultList = matchResponse.Match.MatchStructure.CoursesOfFire
                .SelectMany( cof => cof.ResultLists.Select( rl => new { cof.CourseOfFireId, ResultList = rl } ) )
                .FirstOrDefault( x => !string.IsNullOrWhiteSpace( x.ResultList.ResultName ) );

            Assert.IsNotNull( resultList, "No result list metadata was available from the known member match." );

            return new ResultListMember() {
                MatchId = KnownPublicMatchOwner1BothSides,
                CourseOfFireId = resultList!.CourseOfFireId,
                ResultName = resultList.ResultList.ResultName,
                HeaderName = string.IsNullOrWhiteSpace( resultList.ResultList.EventName ) ? resultList.ResultList.ResultName : resultList.ResultList.EventName
            };
        }

        private static async Task<Tournament> GetTournamentAsync( OrionMatchAPIClient client, MatchID tournamentId, UserAuthentication credentials ) {
            var request = new GetTournamentAuthenticatedRequest( tournamentId, credentials ) {
                IgnoreInMemoryCache = true,
                IgnoreFileSystemCache = true
            };

            var response = await client.GetTournamentAuthenticatedAsync( request );
            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode, "Tournament verification lookup failed." );
            return response.Tournament;
        }

        [TestMethod]
        public async Task CreateMergedResultListWithRequestAddsMergedResultListToTournament() {
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync( client, authorizedUser, UniqueName( "Merged Result Create Request" ) );
                var expectedMergedResultList = CreateMergedResultListModel( UniqueName( "Merged Result Request" ) );

                var request = new CreateMergedResultListAuthenticatedRequest( authorizedUser, tournamentId, expectedMergedResultList );
                var response = await client.CreateMergedResultListAuthenticatedAsync( request );

                Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
                Assert.AreEqual( expectedMergedResultList.ResultName, response.MergedResultList.ResultName );
                Assert.AreEqual( expectedMergedResultList.Method, response.MergedResultList.Method );
                Assert.IsFalse( string.IsNullOrWhiteSpace( response.MergedResultList.MergedId ) );

                var tournament = await GetTournamentAsync( client, tournamentId, authorizedUser );
                Assert.IsTrue( tournament.MergedResultLists.Any( x => x.MergedId == response.MergedResultList.MergedId ) );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task CreateMergedResultListWithConvenienceOverloadAddsMergedResultListToTournament() {
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync( client, authorizedUser, UniqueName( "Merged Result Create Overload" ) );
                var expectedMergedResultList = CreateMergedResultListModel( UniqueName( "Merged Result Overload" ) );

                var response = await client.CreateMergedResultListAuthenticatedAsync( tournamentId, expectedMergedResultList, authorizedUser );

                Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
                Assert.AreEqual( expectedMergedResultList.ResultName, response.MergedResultList.ResultName );
                Assert.AreEqual( expectedMergedResultList.Method, response.MergedResultList.Method );
                Assert.IsFalse( string.IsNullOrWhiteSpace( response.MergedResultList.MergedId ) );

                var tournament = await GetTournamentAsync( client, tournamentId, authorizedUser );
                Assert.IsTrue( tournament.MergedResultLists.Any( x => x.MergedId == response.MergedResultList.MergedId ) );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task AddMergedResultListMemberWithRequestAndRemoveWithOverloadRoundTripsThroughTournament() {
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync( client, authorizedUser, UniqueName( "Merged Result Member Request" ) );
                var createResponse = await client.CreateMergedResultListAuthenticatedAsync(
                    tournamentId,
                    CreateMergedResultListModel( UniqueName( "Merged Member Request" ) ),
                    authorizedUser );
                var member = await CreateKnownApprovedResultListMemberAsync( client, tournamentId, authorizedUser );

                var addRequest = new AddMergedResultListMemberAuthenticatedRequest( authorizedUser, tournamentId, createResponse.MergedResultList.MergedId, member );
                var addResponse = await client.AddMergedResultListMemberAuthenticatedAsync( addRequest );

                Assert.AreEqual( HttpStatusCode.OK, addResponse.RestApiStatusCode );
                Assert.AreEqual( member.MatchId, addResponse.ResultListMember.MatchId );
                Assert.AreEqual( member.CourseOfFireId, addResponse.ResultListMember.CourseOfFireId );
                Assert.AreEqual( member.ResultName, addResponse.ResultListMember.ResultName );

                var tournamentAfterAdd = await GetTournamentAsync( client, tournamentId, authorizedUser );
                var mergedResultListAfterAdd = tournamentAfterAdd.MergedResultLists.First( x => x.MergedId == createResponse.MergedResultList.MergedId );
                Assert.IsTrue( mergedResultListAfterAdd.ResultListMembers.Any( x =>
                    x.MatchId.Equals( member.MatchId )
                    && x.CourseOfFireId == member.CourseOfFireId
                    && x.ResultName == member.ResultName
                    && x.HeaderName == member.HeaderName ) );

                var removeResponse = await client.RemoveMergedResultListMemberAuthenticatedAsync( tournamentId, createResponse.MergedResultList.MergedId, member, authorizedUser );

                Assert.AreEqual( HttpStatusCode.OK, removeResponse.RestApiStatusCode );
                Assert.AreEqual( member.MatchId, removeResponse.ResultListMember.MatchId );
                Assert.AreEqual( member.ResultName, removeResponse.ResultListMember.ResultName );

                var tournamentAfterRemove = await GetTournamentAsync( client, tournamentId, authorizedUser );
                var mergedResultListAfterRemove = tournamentAfterRemove.MergedResultLists.First( x => x.MergedId == createResponse.MergedResultList.MergedId );
                Assert.IsFalse( mergedResultListAfterRemove.ResultListMembers.Any( x =>
                    x.MatchId.Equals( member.MatchId )
                    && x.CourseOfFireId == member.CourseOfFireId
                    && x.ResultName == member.ResultName
                    && x.HeaderName == member.HeaderName ) );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task AddMergedResultListMemberWithOverloadAndRemoveWithRequestRoundTripsThroughTournament() {
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync( client, authorizedUser, UniqueName( "Merged Result Member Overload" ) );
                var createResponse = await client.CreateMergedResultListAuthenticatedAsync(
                    tournamentId,
                    CreateMergedResultListModel( UniqueName( "Merged Member Overload" ) ),
                    authorizedUser );
                var member = await CreateKnownApprovedResultListMemberAsync( client, tournamentId, authorizedUser );

                var addResponse = await client.AddMergedResultListMemberAuthenticatedAsync( tournamentId, createResponse.MergedResultList.MergedId, member, authorizedUser );

                Assert.AreEqual( HttpStatusCode.OK, addResponse.RestApiStatusCode );
                Assert.AreEqual( member.MatchId, addResponse.ResultListMember.MatchId );
                Assert.AreEqual( member.HeaderName, addResponse.ResultListMember.HeaderName );

                var tournamentAfterAdd = await GetTournamentAsync( client, tournamentId, authorizedUser );
                var mergedResultListAfterAdd = tournamentAfterAdd.MergedResultLists.First( x => x.MergedId == createResponse.MergedResultList.MergedId );
                Assert.IsTrue( mergedResultListAfterAdd.ResultListMembers.Any( x =>
                    x.MatchId.Equals( member.MatchId )
                    && x.CourseOfFireId == member.CourseOfFireId
                    && x.ResultName == member.ResultName
                    && x.HeaderName == member.HeaderName ) );

                var removeRequest = new RemoveMergedResultListMemberAuthenticatedRequest( authorizedUser, tournamentId, createResponse.MergedResultList.MergedId, member );
                var removeResponse = await client.RemoveMergedResultListMemberAuthenticatedAsync( removeRequest );

                Assert.AreEqual( HttpStatusCode.OK, removeResponse.RestApiStatusCode );
                Assert.AreEqual( member.MatchId, removeResponse.ResultListMember.MatchId );
                Assert.AreEqual( member.HeaderName, removeResponse.ResultListMember.HeaderName );

                var tournamentAfterRemove = await GetTournamentAsync( client, tournamentId, authorizedUser );
                var mergedResultListAfterRemove = tournamentAfterRemove.MergedResultLists.First( x => x.MergedId == createResponse.MergedResultList.MergedId );
                Assert.IsFalse( mergedResultListAfterRemove.ResultListMembers.Any( x =>
                    x.MatchId.Equals( member.MatchId )
                    && x.CourseOfFireId == member.CourseOfFireId
                    && x.ResultName == member.ResultName
                    && x.HeaderName == member.HeaderName ) );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task DeleteMergedResultListWithRequestRemovesMergedResultListFromTournament() {
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync( client, authorizedUser, UniqueName( "Merged Result Delete Request" ) );
                var createResponse = await client.CreateMergedResultListAuthenticatedAsync(
                    tournamentId,
                    CreateMergedResultListModel( UniqueName( "Merged Delete Request" ) ),
                    authorizedUser );

                var deleteRequest = new DeleteMergedResultListAuthenticatedRequest( authorizedUser, tournamentId, createResponse.MergedResultList.MergedId );
                var deleteResponse = await client.DeleteMergedResultListAuthenticatedAsync( deleteRequest );

                Assert.AreEqual( HttpStatusCode.OK, deleteResponse.RestApiStatusCode );
                Assert.AreEqual( tournamentId, deleteResponse.DeleteMergedResultListResponse.MatchId );
                Assert.AreEqual( createResponse.MergedResultList.MergedId, deleteResponse.DeleteMergedResultListResponse.MergedId );

                var tournamentAfterDelete = await GetTournamentAsync( client, tournamentId, authorizedUser );
                Assert.IsFalse( tournamentAfterDelete.MergedResultLists.Any( x => x.MergedId == createResponse.MergedResultList.MergedId ) );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task DeleteMergedResultListWithConvenienceOverloadRemovesMergedResultListFromTournament() {
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync( client, authorizedUser, UniqueName( "Merged Result Delete Overload" ) );
                var createResponse = await client.CreateMergedResultListAuthenticatedAsync(
                    tournamentId,
                    CreateMergedResultListModel( UniqueName( "Merged Delete Overload" ) ),
                    authorizedUser );

                var deleteResponse = await client.DeleteMergedResultListAuthenticatedAsync( tournamentId, createResponse.MergedResultList.MergedId, authorizedUser );

                Assert.AreEqual( HttpStatusCode.OK, deleteResponse.RestApiStatusCode );
                Assert.AreEqual( tournamentId, deleteResponse.DeleteMergedResultListResponse.MatchId );
                Assert.AreEqual( createResponse.MergedResultList.MergedId, deleteResponse.DeleteMergedResultListResponse.MergedId );

                var tournamentAfterDelete = await GetTournamentAsync( client, tournamentId, authorizedUser );
                Assert.IsFalse( tournamentAfterDelete.MergedResultLists.Any( x => x.MergedId == createResponse.MergedResultList.MergedId ) );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }
    }
}
