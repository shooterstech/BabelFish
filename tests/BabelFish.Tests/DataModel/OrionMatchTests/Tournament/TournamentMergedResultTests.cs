using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.ResultListMerger;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;
using OrionTournament = Scopos.BabelFish.DataModel.OrionMatch.Tournament;

namespace Scopos.BabelFish.Tests.DataModel.OrionMatch.Tournament {
    [TestClass]
    public class TournamentMergedResultTests : BaseTestClass {

        private const string TournamentOwnerId = "OrionAcct000002";
        
        // Additional fixture assumptions for these tests:
        // - TestDev7 is authenticated and has tournament-side permission for OrionAcct000002 to create tournaments,
        //   edit merged result lists, add and approve tournament members, and delete the test tournaments during cleanup.
        // - TestDev11 is authenticated but does not have OrionAcct000002 tournament edit permissions.
        // - MatchID 1.1.1011319229.1 is a public match owned by OrionAcct000001 where TestDev11 has match-side
        //   tournament.join permission and the match exposes at least one named result list.
        // - MatchID 1.1.2021020310584218.1 is a public match owned by OrionAcct000001 that TestDev7 can add to an
        //   OrionAcct000002 tournament and approve when needed, and the match exposes at least one named result list.
        // Requirements for these fixture matches:
        // - Public match owned by OrionAcct000001.
        // - Accessible to the test user needed for that fixture path.
        // - Contains at least one result list with a non-empty ResultName.
        private static readonly MatchID KnownPublicMatchOwner1PatchJoin = new MatchID( "1.1.1011319229.1" );
        private static readonly MatchID KnownPublicMatchOwner1BothSides = new MatchID( "1.1.2021020310584218.1" );

        private static OrionMatchAPIClient CreateClient() => new OrionMatchAPIClient( APIStage.PRODUCTION );

        private static string UniqueName( string prefix ) {
            return $"{prefix} {DateTime.UtcNow:yyyyMMddHHmmssfff}";
        }

        private static MatchID UnknownTournamentId() {
            return new MatchID( $"1.999999.{DateTime.UtcNow.Ticks}.2" );
        }

        private static string UnknownMergedId() {
            return Guid.NewGuid().ToString();
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

        private static async Task<MatchID> CreateTournamentAsync(
            OrionMatchAPIClient client,
            UserAuthentication credentials,
            string name,
            MemberPolicyOption? memberPolicy = MemberPolicyOption.INVITE ) {

            var request = new CreateTournamentAuthenticatedRequest( credentials ) {
                TournamentName = name,
                OwnerId = TournamentOwnerId,
                Visibility = VisibilityOption.PUBLIC,
                ShowOnSearch = false,
                MemberPolicy = memberPolicy
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

        private static async Task<ResultListMember> CreateResultListMemberFromMatchAsync( OrionMatchAPIClient client, MatchID matchId ) {
            var matchResponse = await client.GetMatchPublicAsync( matchId );
            Assert.AreEqual( HttpStatusCode.OK, matchResponse.RestApiStatusCode, "Known member match lookup failed for merged result list test." );

            var resultList = matchResponse.Match.MatchStructure.CoursesOfFire
                .SelectMany( cof => cof.ResultLists.Select( rl => new { cof.CourseOfFireId, ResultList = rl } ) )
                .FirstOrDefault( x => !string.IsNullOrWhiteSpace( x.ResultList.ResultName ) );

            Assert.IsNotNull( resultList, "No result list metadata was available from the known member match." );

            return new ResultListMember() {
                MatchId = matchId,
                CourseOfFireId = resultList!.CourseOfFireId,
                ResultName = resultList.ResultList.ResultName,
                HeaderName = string.IsNullOrWhiteSpace( resultList.ResultList.EventName ) ? resultList.ResultList.ResultName : resultList.ResultList.EventName
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

            return await CreateResultListMemberFromMatchAsync( client, KnownPublicMatchOwner1BothSides );
        }

        private static async Task<OrionTournament> GetTournamentAsync( OrionMatchAPIClient client, MatchID tournamentId, UserAuthentication credentials ) {
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
        public async Task CreateMergedResultListRejectsDuplicateResultNameWithinTournament() {
            // Intention: verify CreateMergedResultList returns BadRequest when a tournament already contains the same ResultName.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync( client, authorizedUser, UniqueName( "Merged Result Create Duplicate" ) );
                var duplicateName = UniqueName( "Merged Result Duplicate Name" );

                var firstResponse = await client.CreateMergedResultListAuthenticatedAsync(
                    tournamentId,
                    CreateMergedResultListModel( duplicateName ),
                    authorizedUser );
                var secondResponse = await client.CreateMergedResultListAuthenticatedAsync(
                    tournamentId,
                    CreateMergedResultListModel( duplicateName ),
                    authorizedUser );

                Assert.AreEqual( HttpStatusCode.OK, firstResponse.RestApiStatusCode );
                Assert.AreEqual( HttpStatusCode.BadRequest, secondResponse.RestApiStatusCode );

                var tournament = await GetTournamentAsync( client, tournamentId, authorizedUser );
                Assert.AreEqual( 1, tournament.MergedResultLists.Count( x => x.ResultName == duplicateName ) );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task CreateMergedResultListReturnsNotFoundWhenTournamentDoesNotExist() {
            // Intention: verify CreateMergedResultList returns NotFound for a tournament id that does not exist.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            var response = await client.CreateMergedResultListAuthenticatedAsync(
                UnknownTournamentId(),
                CreateMergedResultListModel( UniqueName( "Merged Result Unknown Tournament" ) ),
                authorizedUser );

            Assert.AreEqual( HttpStatusCode.NotFound, response.RestApiStatusCode );
        }

        [TestMethod]
        public async Task CreateMergedResultListReturnsUnauthorizedForCallerWithoutPermission() {
            // Intention: verify CreateMergedResultList returns Unauthorized when the caller lacks tournament-side permissions.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );
            var lowPrivilegeUser = await AuthenticateAsync( Constants.TestDev11Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync( client, authorizedUser, UniqueName( "Merged Result Create Unauthorized" ) );

                var response = await client.CreateMergedResultListAuthenticatedAsync(
                    tournamentId,
                    CreateMergedResultListModel( UniqueName( "Merged Result Unauthorized" ) ),
                    lowPrivilegeUser );

                Assert.AreEqual( HttpStatusCode.Unauthorized, response.RestApiStatusCode );
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
        public async Task AddMergedResultListMemberRejectsDuplicateMemberForMergedResultList() {
            // Intention: verify AddMergedResultListMember returns BadRequest when the same member is added twice to one merged result list.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync( client, authorizedUser, UniqueName( "Merged Result Add Duplicate Member" ) );
                var createResponse = await client.CreateMergedResultListAuthenticatedAsync(
                    tournamentId,
                    CreateMergedResultListModel( UniqueName( "Merged Duplicate Member" ) ),
                    authorizedUser );
                var member = await CreateKnownApprovedResultListMemberAsync( client, tournamentId, authorizedUser );

                var firstAdd = await client.AddMergedResultListMemberAuthenticatedAsync( tournamentId, createResponse.MergedResultList.MergedId, member, authorizedUser );
                var secondAdd = await client.AddMergedResultListMemberAuthenticatedAsync( tournamentId, createResponse.MergedResultList.MergedId, member, authorizedUser );

                Assert.AreEqual( HttpStatusCode.OK, firstAdd.RestApiStatusCode );
                Assert.AreEqual( HttpStatusCode.BadRequest, secondAdd.RestApiStatusCode );

                var tournament = await GetTournamentAsync( client, tournamentId, authorizedUser );
                var mergedResultList = tournament.MergedResultLists.First( x => x.MergedId == createResponse.MergedResultList.MergedId );
                Assert.AreEqual( 1, mergedResultList.ResultListMembers.Count( x =>
                    x.MatchId.Equals( member.MatchId )
                    && x.CourseOfFireId == member.CourseOfFireId
                    && x.ResultName == member.ResultName
                    && x.HeaderName == member.HeaderName ) );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task AddMergedResultListMemberReturnsNotFoundWhenMergedResultListDoesNotExist() {
            // Intention: verify AddMergedResultListMember returns NotFound when the merged-id does not exist for the tournament.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync( client, authorizedUser, UniqueName( "Merged Result Add Missing Merged Id" ) );
                var member = await CreateKnownApprovedResultListMemberAsync( client, tournamentId, authorizedUser );

                var response = await client.AddMergedResultListMemberAuthenticatedAsync( tournamentId, UnknownMergedId(), member, authorizedUser );

                Assert.AreEqual( HttpStatusCode.NotFound, response.RestApiStatusCode );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task AddMergedResultListMemberReturnsUnauthorizedWhenTournamentMemberIsPendingApproval() {
            // Intention: verify AddMergedResultListMember returns Unauthorized when the target tournament member exists but is not APPROVED.
            var client = CreateClient();
            var tournamentOwnerUser = await AuthenticateAsync( Constants.TestDev7Credentials );
            var matchSideOnlyUser = await AuthenticateAsync( Constants.TestDev11Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    tournamentOwnerUser,
                    UniqueName( "Merged Result Add Pending Member" ),
                    MemberPolicyOption.REQUEST );

                var createResponse = await client.CreateMergedResultListAuthenticatedAsync(
                    tournamentId,
                    CreateMergedResultListModel( UniqueName( "Merged Pending Member" ) ),
                    tournamentOwnerUser );

                var addTournamentMemberResponse = await client.AddTournamentMemberAuthenticatedAsync( tournamentId, KnownPublicMatchOwner1PatchJoin, matchSideOnlyUser );
                Assert.AreEqual( HttpStatusCode.OK, addTournamentMemberResponse.RestApiStatusCode, "Tournament member setup failed for pending merged result list test." );
                Assert.AreEqual( ApprovalStatus.PENDING, addTournamentMemberResponse.TournamentMember.ApprovalStatus, "Expected the fixture match to remain pending for this test." );

                var member = await CreateResultListMemberFromMatchAsync( client, KnownPublicMatchOwner1PatchJoin );

                var response = await client.AddMergedResultListMemberAuthenticatedAsync(
                    tournamentId,
                    createResponse.MergedResultList.MergedId,
                    member,
                    tournamentOwnerUser );

                Assert.AreEqual( HttpStatusCode.Unauthorized, response.RestApiStatusCode );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, tournamentOwnerUser );
            }
        }

        [TestMethod]
        public async Task AddMergedResultListMemberReturnsNotFoundWhenCourseOfFireDoesNotExistForMemberMatch() {
            // Intention: verify AddMergedResultListMember returns NotFound when the member match exists but the supplied CourseOfFireId does not.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync( client, authorizedUser, UniqueName( "Merged Result Add Missing Cof" ) );
                var createResponse = await client.CreateMergedResultListAuthenticatedAsync(
                    tournamentId,
                    CreateMergedResultListModel( UniqueName( "Merged Missing Cof" ) ),
                    authorizedUser );
                var member = await CreateKnownApprovedResultListMemberAsync( client, tournamentId, authorizedUser );

                var invalidCourseOfFireMember = new ResultListMember() {
                    MatchId = member.MatchId,
                    CourseOfFireId = int.MaxValue,
                    ResultName = member.ResultName,
                    HeaderName = member.HeaderName
                };

                var response = await client.AddMergedResultListMemberAuthenticatedAsync(
                    tournamentId,
                    createResponse.MergedResultList.MergedId,
                    invalidCourseOfFireMember,
                    authorizedUser );

                Assert.AreEqual( HttpStatusCode.NotFound, response.RestApiStatusCode );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task RemoveMergedResultListMemberReturnsNotFoundWhenMemberWasNeverAdded() {
            // Intention: verify RemoveMergedResultListMember returns NotFound when the merged result list exists but the requested member row does not.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync( client, authorizedUser, UniqueName( "Merged Result Remove Missing Member" ) );
                var createResponse = await client.CreateMergedResultListAuthenticatedAsync(
                    tournamentId,
                    CreateMergedResultListModel( UniqueName( "Merged Remove Missing Member" ) ),
                    authorizedUser );
                var member = await CreateKnownApprovedResultListMemberAsync( client, tournamentId, authorizedUser );

                var response = await client.RemoveMergedResultListMemberAuthenticatedAsync(
                    tournamentId,
                    createResponse.MergedResultList.MergedId,
                    member,
                    authorizedUser );

                Assert.AreEqual( HttpStatusCode.NotFound, response.RestApiStatusCode );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task RemoveMergedResultListMemberReturnsNotFoundWhenMergedResultListDoesNotExist() {
            // Intention: verify RemoveMergedResultListMember returns NotFound when the merged-id does not exist for the tournament but does exist for another tournament.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            MatchID? otherTournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync( client, authorizedUser, UniqueName( "Merged Result Remove Missing Merged Id" ) );
                otherTournamentId = await CreateTournamentAsync( client, authorizedUser, UniqueName( "Merged Result Remove Other Tournament" ) );
                var otherTournamentMergedResult = await client.CreateMergedResultListAuthenticatedAsync(
                    otherTournamentId,
                    CreateMergedResultListModel( UniqueName( "Merged Remove Other Tournament" ) ),
                    authorizedUser );
                var member = await CreateKnownApprovedResultListMemberAsync( client, tournamentId, authorizedUser );

                var response = await client.RemoveMergedResultListMemberAuthenticatedAsync(
                    tournamentId,
                    otherTournamentMergedResult.MergedResultList.MergedId,
                    member,
                    authorizedUser );

                Assert.AreEqual( HttpStatusCode.NotFound, response.RestApiStatusCode );
            } finally {
                await TryDeleteTournamentAsync( client, otherTournamentId, authorizedUser );
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task RemoveMergedResultListMemberReturnsUnauthorizedForCallerWithoutPermission() {
            // Intention: verify RemoveMergedResultListMember returns Unauthorized when the caller lacks permission to edit the tournament.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );
            var lowPrivilegeUser = await AuthenticateAsync( Constants.TestDev11Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync( client, authorizedUser, UniqueName( "Merged Result Remove Unauthorized" ) );
                var createResponse = await client.CreateMergedResultListAuthenticatedAsync(
                    tournamentId,
                    CreateMergedResultListModel( UniqueName( "Merged Remove Unauthorized" ) ),
                    authorizedUser );
                var member = await CreateKnownApprovedResultListMemberAsync( client, tournamentId, authorizedUser );
                var addResponse = await client.AddMergedResultListMemberAuthenticatedAsync(
                    tournamentId,
                    createResponse.MergedResultList.MergedId,
                    member,
                    authorizedUser );
                Assert.AreEqual( HttpStatusCode.OK, addResponse.RestApiStatusCode );

                var response = await client.RemoveMergedResultListMemberAuthenticatedAsync(
                    tournamentId,
                    createResponse.MergedResultList.MergedId,
                    member,
                    lowPrivilegeUser );

                Assert.AreEqual( HttpStatusCode.Unauthorized, response.RestApiStatusCode );
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

        [TestMethod]
        public async Task DeleteMergedResultListReturnsNotFoundWhenMergedResultListDoesNotExist() {
            // Intention: verify DeleteMergedResultList returns NotFound when the requested merged-id is missing from the tournament.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync( client, authorizedUser, UniqueName( "Merged Result Delete Missing Merged Id" ) );

                var response = await client.DeleteMergedResultListAuthenticatedAsync( tournamentId, UnknownMergedId(), authorizedUser );

                Assert.AreEqual( HttpStatusCode.NotFound, response.RestApiStatusCode );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task DeleteMergedResultListReturnsUnauthorizedForCallerWithoutPermission() {
            // Intention: verify DeleteMergedResultList returns Unauthorized when the caller lacks permission to edit the tournament.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );
            var lowPrivilegeUser = await AuthenticateAsync( Constants.TestDev11Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync( client, authorizedUser, UniqueName( "Merged Result Delete Unauthorized" ) );
                var createResponse = await client.CreateMergedResultListAuthenticatedAsync(
                    tournamentId,
                    CreateMergedResultListModel( UniqueName( "Merged Delete Unauthorized" ) ),
                    authorizedUser );

                var response = await client.DeleteMergedResultListAuthenticatedAsync(
                    tournamentId,
                    createResponse.MergedResultList.MergedId,
                    lowPrivilegeUser );

                Assert.AreEqual( HttpStatusCode.Unauthorized, response.RestApiStatusCode );

                var tournament = await GetTournamentAsync( client, tournamentId, authorizedUser );
                Assert.IsTrue( tournament.MergedResultLists.Any( x => x.MergedId == createResponse.MergedResultList.MergedId ) );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }
    }
}
