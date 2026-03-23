using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.DataModel.OrionMatch.Tournament {
    /*
     * Test data and permission assumptions for these tests:
     * - TestDev7 is authenticated and can create/read/delete tournaments for OrionAcct000002.
     * - TestDev7 can add/remove/patch tournament members for tournaments it creates in OrionAcct000002.
     * - TestDev7 has tournament-side permissions for OrionAcct000001 and match-side permissions for MatchID 1.1.2021020310584218.1.
     * - TestDev11 is authenticated, does not have OrionAcct000002 tournament permissions, and has tournament.join for MatchID 1.1.1011319229.1.
     * - TestDev13 is authenticated but not authorized for OrionAcct or match permissions.
     * - MatchID 1.1.1011318990.1 is a public match owned by OrionAcct000001.
     * - MatchID 1.1.1011319229.1 is a public match owned by OrionAcct000001.
     * - MatchID 1.1.2021020310584218.1 is a public match owned by OrionAcct000001.
     * - MatchID 1.1003.637477891.1 is a public match owned by OrionAcct001003.
     */
    [TestClass]
    public class FullTournamentTests : BaseTestClass {

        private const string TournamentOwnerId = "OrionAcct000002";
        private const string TournamentOwnerIdWithKnownBothSides = "OrionAcct000001";
        private const string MemberOwnerId = "OrionAcct001003";

        private static readonly MatchID KnownPublicMatchOwner1 = new MatchID( "1.1.1011318990.1" );
        private static readonly MatchID KnownPublicMatchOwner1PatchJoin = new MatchID( "1.1.1011319229.1" );
        private static readonly MatchID KnownPublicMatchOwner1BothSides = new MatchID( "1.1.2021020310584218.1" );
        private static readonly MatchID KnownPublicMatchOwner1003 = new MatchID( "1.1003.637477891.1" );

        private static OrionMatchAPIClient CreateClient() => new OrionMatchAPIClient( APIStage.BETA );

        private static string UniqueName( string prefix ) {
            return $"{prefix} {DateTime.UtcNow:yyyyMMddHHmmssfff}";
        }

        private static MatchID UnknownTournamentId() {
            return new MatchID( $"1.999999.{DateTime.UtcNow.Ticks}.2" );
        }

        private static MatchID UnknownMatchId() {
            return new MatchID( $"1.999999.{DateTime.UtcNow.Ticks}.1" );
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
            VisibilityOption visibility,
            bool showOnSearch,
            MemberPolicyOption? memberPolicy = MemberPolicyOption.INVITE,
            string ownerId = TournamentOwnerId ) {

            var request = new CreateTournamentAuthenticatedRequest( credentials ) {
                TournamentName = name,
                OwnerId = ownerId,
                Visibility = visibility,
                ShowOnSearch = showOnSearch,
                MemberPolicy = memberPolicy
            };

            var response = await client.CreateTournamentAuthenticatedAsync( request );
            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode, "Tournament setup failed for test." );
            return response.Tournament.TournamentId;
        }

        private static async Task<List<MatchID>> GetPublicParentMatchCandidatesAsync(
            OrionMatchAPIClient client,
            UserAuthentication credentials,
            int minimumCount,
            string ownerId = "" ) {
            //Match search appears to be broken in beta so just return hard coded list for now
            //var request = new MatchSearchAuthenticatedRequest( credentials ) {
            //    StartDate = DateTime.Today.AddYears( -20 ),
            //    EndDate = DateTime.Today.AddYears( 1 ),
            //    OwnerId = ownerId,
            //    Limit = 100
            //};

            var candidates = new List<MatchID>
            {
                new MatchID("1.1.2021060113580514.1"),
                new MatchID("1.1.2021060114092618.1"),
                new MatchID("1.1.2021060114245608.1"),
                new MatchID("1.1.2021060114263281.1"),
                new MatchID("1.1.2021060114272818.1"),
                new MatchID("1.1.2021071015383280.1"),
                new MatchID("1.1.2021071015435744.1"),
                new MatchID("1.1.2021071015451600.1"),
                new MatchID("1.1.2021071015462684.1"),
                new MatchID("1.1.2021071015474973.1"),
                new MatchID("1.1.2021072313560216.1"),
                new MatchID("1.1.2021080517374319.1"),
                new MatchID("1.1.2021080517385591.1"),
                new MatchID("1.1.2021080517394499.1"),
                new MatchID("1.1.2021080517403669.1"),
                new MatchID("1.1.2021080517411981.1"),
                new MatchID("1.1.2021100915094818.1"),
                new MatchID("1.1.2021100915284158.1"),
                new MatchID("1.1.2021100915303695.1"),
                new MatchID("1.1.2021100915323482.1"),
                new MatchID("1.1.2021110515400157.1"),
                new MatchID("1.1.2021110515442304.1"),
                new MatchID("1.1.2021110515540063.1"),
                new MatchID("1.1.2021110515562519.1"),
                new MatchID("1.1.2021110515580750.1"),
                new MatchID("1.1.2021120317162071.1"),
                new MatchID("1.1.2021120317182866.1"),
                new MatchID("1.1.2021120317215111.1"),
                new MatchID("1.1.2021120317230891.1"),
                new MatchID("1.1.2021120317234410.1"),
                new MatchID("1.1.2022010411082989.1"),
                new MatchID("1.1.2022010411105678.1"),
                new MatchID("1.1.2022010411132118.1"),
                new MatchID("1.1.2022010411172970.1"),
                new MatchID("1.1.2022010411184242.1"),
                new MatchID("1.1.2022013114561710.1"),
                new MatchID("1.1.2022013115022684.1"),
                new MatchID("1.1.2022013115045332.1"),
                new MatchID("1.1.2022013115060721.1"),
                new MatchID("1.1.2022030110090416.1")
            };
            return candidates;

            //var candidateStrings = new HashSet<string>();

            //var maxPages = 20;
            //for (var page = 0; page < maxPages; page++) {
            //    var response = await client.GetMatchSearchAuthenticatedAsync( request );
            //    if (response.RestApiStatusCode != HttpStatusCode.OK) {
            //        break;
            //    }

            //    foreach (var matchAbbr in response.MatchSearchList.Items) {
            //        if (matchAbbr.MatchID == null) {
            //            continue;
            //        }

            //        if (!matchAbbr.MatchID.VirtualMatchParent) {
            //            continue;
            //        }

            //        var idAsString = matchAbbr.MatchID.ToString();
            //        if (candidateStrings.Contains( idAsString )) {
            //            continue;
            //        }

            //        candidateStrings.Add( idAsString );
            //        candidates.Add( matchAbbr.MatchID );
            //    }

            //    if (candidates.Count >= minimumCount || !response.HasMoreItems) {
            //        break;
            //    }

            //    request = (MatchSearchAuthenticatedRequest)response.GetNextRequest();
            //}

            //return candidates;
        }

        [TestMethod]
        public async Task GetTournamentReturnsCreatedPublicTournamentDetails() {
            // Intention: verify GetTournament success path for a public tournament created during test setup.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    UniqueName( "Full Tournament Get Success" ),
                    VisibilityOption.PUBLIC,
                    showOnSearch: true );

                var response = await client.GetTournamentPublicAsync( tournamentId );

                Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
                Assert.AreEqual( tournamentId, response.Tournament.TournamentId );
                Assert.AreEqual( MatchType.TOURNAMENT, response.Tournament.MatchType );
                Assert.AreEqual( TournamentOwnerId, response.Tournament.OwnerId );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task GetTournamentReturnsUnauthorizedForPrivateTournamentWithoutAccess() {
            // Intention: verify GetTournament returns Unauthorized when requesting a private tournament via public endpoint.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    UniqueName( "Full Tournament Get Private Unauthorized" ),
                    VisibilityOption.PRIVATE,
                    showOnSearch: false );

                var response = await client.GetTournamentPublicAsync( tournamentId );

                Assert.AreEqual( HttpStatusCode.Unauthorized, response.RestApiStatusCode );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task GetTournamentReturnsNotFoundForUnknownTournamentId() {
            // Intention: verify GetTournament failure path for a non-existent tournament id.
            var client = CreateClient();

            var response = await client.GetTournamentPublicAsync( UnknownTournamentId() );

            Assert.AreEqual( HttpStatusCode.NotFound, response.RestApiStatusCode );
        }

        [TestMethod]
        public async Task CreateTournamentCreatesTournamentWithProvidedFields() {
            // Intention: verify CreateTournament success path with explicitly provided fields.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                var tournamentName = UniqueName( "Full Tournament Create Explicit" );
                var request = new CreateTournamentAuthenticatedRequest( authorizedUser ) {
                    TournamentName = tournamentName,
                    OwnerId = TournamentOwnerId,
                    Visibility = VisibilityOption.PUBLIC,
                    ShowOnSearch = true,
                    MemberPolicy = MemberPolicyOption.REQUEST
                };

                var response = await client.CreateTournamentAuthenticatedAsync( request );
                tournamentId = response.Tournament.TournamentId;

                Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
                Assert.AreEqual( tournamentName, response.Tournament.TournamentName );
                Assert.AreEqual( TournamentOwnerId, response.Tournament.OwnerId );
                Assert.AreEqual( VisibilityOption.PUBLIC, response.Tournament.Visibility );
                Assert.IsTrue( response.Tournament.IncludeInSearchResults );
                Assert.AreEqual( MemberPolicyOption.REQUEST, response.Tournament.MemberPolicy );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task CreateTournamentUsesDefaultMemberPolicyWhenNotProvided() {
            // Intention: verify CreateTournament edge behavior when member-policy is omitted and defaults are returned.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                var request = new CreateTournamentAuthenticatedRequest( authorizedUser ) {
                    TournamentName = UniqueName( "Full Tournament Create Defaults" ),
                    OwnerId = TournamentOwnerId,
                    Visibility = VisibilityOption.PRIVATE,
                    ShowOnSearch = false,
                    MemberPolicy = null
                };

                var response = await client.CreateTournamentAuthenticatedAsync( request );
                tournamentId = response.Tournament.TournamentId;

                Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
                Assert.AreEqual( MemberPolicyOption.INVITE, response.Tournament.MemberPolicy );
                Assert.IsFalse( response.Tournament.IncludeInSearchResults );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task CreateTournamentRejectsProtectedVisibility() {
            // Intention: verify CreateTournament failure when visibility is Protected, which the lambda disallows for tournaments.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            var request = new CreateTournamentAuthenticatedRequest( authorizedUser ) {
                TournamentName = UniqueName( "Full Tournament Create Protected" ),
                OwnerId = TournamentOwnerId,
                Visibility = VisibilityOption.PROTECTED,
                ShowOnSearch = false,
                MemberPolicy = MemberPolicyOption.INVITE
            };

            var response = await client.CreateTournamentAuthenticatedAsync( request );

            Assert.AreEqual( HttpStatusCode.BadRequest, response.RestApiStatusCode );
        }

        [TestMethod]
        public async Task CreateTournamentRejectsMalformedOwnerId() {
            // Intention: verify CreateTournament failure when owner-id does not use the expected OrionAcct format.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            var request = new CreateTournamentAuthenticatedRequest( authorizedUser ) {
                TournamentName = UniqueName( "Full Tournament Create Bad Owner" ),
                OwnerId = "invalid-owner",
                Visibility = VisibilityOption.PUBLIC,
                ShowOnSearch = false,
                MemberPolicy = MemberPolicyOption.INVITE
            };

            var response = await client.CreateTournamentAuthenticatedAsync( request );

            Assert.AreEqual( HttpStatusCode.BadRequest, response.RestApiStatusCode );
        }

        [TestMethod]
        public async Task CreateTournamentRejectsAtHomeOwnerIdForClubTournamentCreate() {
            // Intention: verify CreateTournament failure when owner-id is AtHome-prefixed and club-only creation is enforced.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            var request = new CreateTournamentAuthenticatedRequest( authorizedUser ) {
                TournamentName = UniqueName( "Full Tournament Create AtHome" ),
                OwnerId = "AtHome000002",
                Visibility = VisibilityOption.PUBLIC,
                ShowOnSearch = false,
                MemberPolicy = MemberPolicyOption.INVITE
            };

            var response = await client.CreateTournamentAuthenticatedAsync( request );

            Assert.AreEqual( HttpStatusCode.Unauthorized, response.RestApiStatusCode );
        }

        [TestMethod]
        public async Task CreateTournamentRejectsCallerWithoutCreatePermission() {
            // Intention: verify CreateTournament failure when caller lacks tournament.create permission on owner OrionAcct000002.
            var client = CreateClient();
            var lowPrivilegeUser = await AuthenticateAsync( Constants.TestDev11Credentials );

            var request = new CreateTournamentAuthenticatedRequest( lowPrivilegeUser ) {
                TournamentName = UniqueName( "Full Tournament Create Unauthorized" ),
                OwnerId = TournamentOwnerId,
                Visibility = VisibilityOption.PUBLIC,
                ShowOnSearch = true,
                MemberPolicy = MemberPolicyOption.INVITE
            };

            var response = await client.CreateTournamentAuthenticatedAsync( request );

            Assert.AreEqual( HttpStatusCode.Unauthorized, response.RestApiStatusCode );
        }

        [TestMethod]
        public async Task DeleteTournamentDeletesPreviouslyCreatedTournament() {
            // Intention: verify DeleteTournament success path and response payload fields.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            var tournamentId = await CreateTournamentAsync(
                client,
                authorizedUser,
                UniqueName( "Full Tournament Delete Success" ),
                VisibilityOption.PUBLIC,
                showOnSearch: false );

            var response = await client.DeleteTournamentAuthenticatedAsync( tournamentId, authorizedUser );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            Assert.AreEqual( tournamentId, response.DeleteTournamentResponse.TournamentId );
            Assert.AreEqual( 2, response.DeleteTournamentResponse.LicenseNumber );
        }

        [TestMethod]
        public async Task DeleteTournamentReturnsNotFoundForUnknownTournamentId() {
            // Intention: verify DeleteTournament failure path for a non-existent tournament id.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            var response = await client.DeleteTournamentAuthenticatedAsync( UnknownTournamentId(), authorizedUser );

            Assert.AreEqual( HttpStatusCode.NotFound, response.RestApiStatusCode );
        }

        [TestMethod]
        public async Task DeleteTournamentReturnsUnauthorizedForCallerWithoutDeletePermission() {
            // Intention: verify DeleteTournament failure when caller lacks tournament.delete permission.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );
            var lowPrivilegeUser = await AuthenticateAsync( Constants.TestDev11Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    UniqueName( "Full Tournament Delete Unauthorized" ),
                    VisibilityOption.PUBLIC,
                    showOnSearch: false );

                var response = await client.DeleteTournamentAuthenticatedAsync( tournamentId, lowPrivilegeUser );

                Assert.AreEqual( HttpStatusCode.Unauthorized, response.RestApiStatusCode );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task DeleteTournamentReturnsNotFoundWhenDeletingSameTournamentTwice() {
            // Intention: verify DeleteTournament succeeds the first time and returns NotFound on a second delete call.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            var tournamentId = await CreateTournamentAsync(
                client,
                authorizedUser,
                UniqueName( "Full Tournament Delete Twice" ),
                VisibilityOption.PUBLIC,
                showOnSearch: false );

            var firstDelete = await client.DeleteTournamentAuthenticatedAsync( tournamentId, authorizedUser );
            Assert.AreEqual( HttpStatusCode.OK, firstDelete.RestApiStatusCode );

            var secondDelete = await client.DeleteTournamentAuthenticatedAsync( tournamentId, authorizedUser );
            Assert.AreEqual( HttpStatusCode.NotFound, secondDelete.RestApiStatusCode );
        }

        [TestMethod]
        public async Task AddTournamentMemberAddsPublicMatchToTournament() {
            // Intention: verify AddTournamentMember success path for a public match into an INVITE tournament.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    UniqueName( "Full Tournament Add Member Success" ),
                    VisibilityOption.PUBLIC,
                    showOnSearch: false,
                    memberPolicy: MemberPolicyOption.INVITE );

                var response = await client.AddTournamentMemberAuthenticatedAsync( tournamentId, KnownPublicMatchOwner1003, authorizedUser );

                Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
                Assert.AreEqual( tournamentId, response.TournamentMember.TournamentId );
                Assert.AreEqual( KnownPublicMatchOwner1003, response.TournamentMember.MatchId );
                Assert.IsTrue(
                    response.TournamentMember.ApprovalStatus == ApprovalStatus.APPROVED
                    || response.TournamentMember.ApprovalStatus == ApprovalStatus.PENDING );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task AddTournamentMemberRejectsTournamentAsItsOwnMember() {
            // Intention: verify AddTournamentMember edge validation that a tournament cannot be added as its own member.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    UniqueName( "Full Tournament Add Member Self" ),
                    VisibilityOption.PUBLIC,
                    showOnSearch: false );

                var response = await client.AddTournamentMemberAuthenticatedAsync( tournamentId, tournamentId, authorizedUser );

                Assert.AreEqual( HttpStatusCode.BadRequest, response.RestApiStatusCode );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task AddTournamentMemberReturnsNotFoundWhenTournamentDoesNotExist() {
            // Intention: verify AddTournamentMember failure path when tournament id does not exist.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            var response = await client.AddTournamentMemberAuthenticatedAsync( UnknownTournamentId(), KnownPublicMatchOwner1003, authorizedUser );

            Assert.AreEqual( HttpStatusCode.NotFound, response.RestApiStatusCode );
        }

        [TestMethod]
        public async Task AddTournamentMemberReturnsNotFoundWhenMatchDoesNotExist() {
            // Intention: verify AddTournamentMember failure path when member match id does not exist.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    UniqueName( "Full Tournament Add Member Missing Match" ),
                    VisibilityOption.PUBLIC,
                    showOnSearch: false );

                var response = await client.AddTournamentMemberAuthenticatedAsync( tournamentId, UnknownMatchId(), authorizedUser );

                Assert.AreEqual( HttpStatusCode.NotFound, response.RestApiStatusCode );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task AddTournamentMemberRejectsDuplicateMembership() {
            // Intention: verify AddTournamentMember failure path when adding the same match twice.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    UniqueName( "Full Tournament Add Member Duplicate" ),
                    VisibilityOption.PUBLIC,
                    showOnSearch: false,
                    memberPolicy: MemberPolicyOption.INVITE );

                var firstAdd = await client.AddTournamentMemberAuthenticatedAsync( tournamentId, KnownPublicMatchOwner1003, authorizedUser );
                Assert.AreEqual( HttpStatusCode.OK, firstAdd.RestApiStatusCode );

                var secondAdd = await client.AddTournamentMemberAuthenticatedAsync( tournamentId, KnownPublicMatchOwner1003, authorizedUser );

                Assert.AreEqual( HttpStatusCode.BadRequest, secondAdd.RestApiStatusCode );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task AddTournamentMemberReturnsUnauthorizedForCallerWithoutPermission() {
            // Intention: verify AddTournamentMember failure when caller lacks invite permission on the tournament.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );
            var lowPrivilegeUser = await AuthenticateAsync( Constants.TestDev11Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    UniqueName( "Full Tournament Add Member Unauthorized" ),
                    VisibilityOption.PUBLIC,
                    showOnSearch: false,
                    memberPolicy: MemberPolicyOption.INVITE );

                var response = await client.AddTournamentMemberAuthenticatedAsync( tournamentId, KnownPublicMatchOwner1003, lowPrivilegeUser );

                Assert.AreEqual( HttpStatusCode.Unauthorized, response.RestApiStatusCode );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task AddTournamentMemberReturnsPendingForRequestPolicyWhenCallerOnlyHasMatchSidePermission() {
            // Intention: verify REQUEST policy returns PENDING when caller has only tournament.join permission on the member match.
            var client = CreateClient();
            var tournamentOwnerUser = await AuthenticateAsync( Constants.TestDev7Credentials );
            var matchSideOnlyUser = await AuthenticateAsync( Constants.TestDev11Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    tournamentOwnerUser,
                    UniqueName( "Full Tournament Add Request Pending MatchSideOnly" ),
                    VisibilityOption.PUBLIC,
                    showOnSearch: false,
                    memberPolicy: MemberPolicyOption.REQUEST,
                    ownerId: TournamentOwnerId );

                var response = await client.AddTournamentMemberAuthenticatedAsync( tournamentId, KnownPublicMatchOwner1PatchJoin, matchSideOnlyUser );

                Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
                Assert.AreEqual( ApprovalStatus.PENDING, response.TournamentMember.ApprovalStatus );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, tournamentOwnerUser );
            }
        }

        [TestMethod]
        public async Task AddTournamentMemberReturnsPendingForInvitePolicyWhenCallerOnlyHasTournamentSidePermission() {
            // Intention: verify INVITE policy returns PENDING when caller can invite to tournament but lacks match-side join permission.
            var client = CreateClient();
            var tournamentSideUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    tournamentSideUser,
                    UniqueName( "Full Tournament Add Invite Pending TournamentSideOnly" ),
                    VisibilityOption.PUBLIC,
                    showOnSearch: false,
                    memberPolicy: MemberPolicyOption.INVITE,
                    ownerId: TournamentOwnerId );

                var response = await client.AddTournamentMemberAuthenticatedAsync( tournamentId, KnownPublicMatchOwner1003, tournamentSideUser );

                Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
                Assert.AreEqual( ApprovalStatus.PENDING, response.TournamentMember.ApprovalStatus );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, tournamentSideUser );
            }
        }

        [TestMethod]
        public async Task AddTournamentMemberReturnsApprovedWhenCallerHasBothTournamentAndMatchSidePermissions() {
            // Intention: verify AddTournamentMember returns APPROVED when caller has permissions on both the tournament and the member match.
            var client = CreateClient();
            var dualPermissionUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    dualPermissionUser,
                    UniqueName( "Full Tournament Add Approved BothSides" ),
                    VisibilityOption.PUBLIC,
                    showOnSearch: false,
                    memberPolicy: MemberPolicyOption.INVITE,
                    ownerId: TournamentOwnerIdWithKnownBothSides );

                var response = await client.AddTournamentMemberAuthenticatedAsync( tournamentId, KnownPublicMatchOwner1BothSides, dualPermissionUser );

                Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
                Assert.AreEqual( ApprovalStatus.APPROVED, response.TournamentMember.ApprovalStatus );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, dualPermissionUser );
            }
        }

        [TestMethod]
        public async Task AddTournamentMemberReturnsConflictWhenAddingTwentyFifthMember() {
            // Intention: verify AddTournamentMember enforces MAX_TOURNAMENT_MEMBERS and returns 409 when adding a 25th unique member.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    UniqueName( "Full Tournament Add Member Max Capacity" ),
                    VisibilityOption.PUBLIC,
                    showOnSearch: false,
                    memberPolicy: MemberPolicyOption.INVITE,
                    ownerId: TournamentOwnerId );

                var candidates = await GetPublicParentMatchCandidatesAsync(
                    client,
                    authorizedUser,
                    minimumCount: 60 );

                var successfulAdds = 0;
                var attempted = 0;
                foreach (var candidateMatchId in candidates) {
                    attempted++;
                    var addResponse = await client.AddTournamentMemberAuthenticatedAsync( tournamentId, candidateMatchId, authorizedUser );

                    if (addResponse.RestApiStatusCode == HttpStatusCode.OK) {
                        successfulAdds++;
                        if (successfulAdds > 24) {
                            Assert.Fail( "Expected a 409 Conflict when adding the 25th member, but a 25th add returned OK." );
                        }
                        continue;
                    }

                    if (addResponse.RestApiStatusCode == HttpStatusCode.Conflict) {
                        Assert.AreEqual( 24, successfulAdds, $"Expected conflict on 25th add, but conflict occurred after {successfulAdds} successful adds." );
                        return;
                    }
                }

                Assert.Inconclusive(
                    $"Could not hit tournament member capacity. Successful adds: {successfulAdds}, attempted candidates: {attempted}. Additional public match fixtures may be needed." );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task PatchTournamentMemberUpdatesApprovalStatusForRequestPolicyTournament() {
            // Intention: verify PatchTournamentMember success path updates approval status for REQUEST-policy tournaments.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    UniqueName( "Full Tournament Patch Member Success" ),
                    VisibilityOption.PUBLIC,
                    showOnSearch: false,
                    memberPolicy: MemberPolicyOption.REQUEST );

                var addResponse = await client.AddTournamentMemberAuthenticatedAsync( tournamentId, KnownPublicMatchOwner1, authorizedUser );
                Assert.AreEqual( HttpStatusCode.OK, addResponse.RestApiStatusCode );

                var patchRequest = new PatchTournamentMemberAuthenticatedRequest( authorizedUser ) {
                    TournamentId = tournamentId,
                    MatchId = KnownPublicMatchOwner1,
                    ApprovalStatus = ApprovalStatus.REJECTED
                };

                var patchResponse = await client.PatchTournamentMemberAuthenticatedAsync( patchRequest );

                Assert.AreEqual( HttpStatusCode.OK, patchResponse.RestApiStatusCode );
                Assert.AreEqual( tournamentId, patchResponse.TournamentMember.TournamentId );
                Assert.AreEqual( KnownPublicMatchOwner1, patchResponse.TournamentMember.MatchId );
                Assert.AreEqual( ApprovalStatus.REJECTED, patchResponse.TournamentMember.ApprovalStatus );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task PatchTournamentMemberUpdatesApprovalStatusForInvitePolicyTournament() {
            // Intention: verify PatchTournamentMember success path for INVITE-policy tournaments when caller has match-side tournament.join permission.
            var client = CreateClient();
            var tournamentOwnerUser = await AuthenticateAsync( Constants.TestDev7Credentials );
            var matchAuthorizedUser = await AuthenticateAsync( Constants.TestDev11Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    tournamentOwnerUser,
                    UniqueName( "Full Tournament Patch Invite Success" ),
                    VisibilityOption.PUBLIC,
                    showOnSearch: false,
                    memberPolicy: MemberPolicyOption.INVITE );

                var addResponse = await client.AddTournamentMemberAuthenticatedAsync( tournamentId, KnownPublicMatchOwner1PatchJoin, tournamentOwnerUser );
                Assert.AreEqual( HttpStatusCode.OK, addResponse.RestApiStatusCode );

                var patchRequest = new PatchTournamentMemberAuthenticatedRequest( matchAuthorizedUser ) {
                    TournamentId = tournamentId,
                    MatchId = KnownPublicMatchOwner1PatchJoin,
                    ApprovalStatus = ApprovalStatus.REJECTED
                };

                var patchResponse = await client.PatchTournamentMemberAuthenticatedAsync( patchRequest );

                Assert.AreEqual( HttpStatusCode.OK, patchResponse.RestApiStatusCode );
                Assert.AreEqual( tournamentId, patchResponse.TournamentMember.TournamentId );
                Assert.AreEqual( KnownPublicMatchOwner1PatchJoin, patchResponse.TournamentMember.MatchId );
                Assert.AreEqual( ApprovalStatus.REJECTED, patchResponse.TournamentMember.ApprovalStatus );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, tournamentOwnerUser );
            }
        }

        [TestMethod]
        public async Task PatchTournamentMemberReturnsUnauthorizedForInvitePolicyWhenCallerLacksMatchJoinPermission() {
            // Intention: verify PatchTournamentMember failure for INVITE-policy tournaments when caller lacks match-side tournament.join permission.
            var client = CreateClient();
            var tournamentOwnerUser = await AuthenticateAsync( Constants.TestDev7Credentials );
            var unauthorizedUser = await AuthenticateAsync( Constants.TestDev13Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    tournamentOwnerUser,
                    UniqueName( "Full Tournament Patch Invite Unauthorized" ),
                    VisibilityOption.PUBLIC,
                    showOnSearch: false,
                    memberPolicy: MemberPolicyOption.INVITE );

                var addResponse = await client.AddTournamentMemberAuthenticatedAsync( tournamentId, KnownPublicMatchOwner1PatchJoin, tournamentOwnerUser );
                Assert.AreEqual( HttpStatusCode.OK, addResponse.RestApiStatusCode );

                var patchRequest = new PatchTournamentMemberAuthenticatedRequest( unauthorizedUser ) {
                    TournamentId = tournamentId,
                    MatchId = KnownPublicMatchOwner1PatchJoin,
                    ApprovalStatus = ApprovalStatus.APPROVED
                };

                var patchResponse = await client.PatchTournamentMemberAuthenticatedAsync( patchRequest );

                Assert.AreEqual( HttpStatusCode.Unauthorized, patchResponse.RestApiStatusCode );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, tournamentOwnerUser );
            }
        }

        [TestMethod]
        public async Task PatchTournamentMemberReturnsNotFoundWhenTournamentDoesNotExist() {
            // Intention: verify PatchTournamentMember failure path for a non-existent tournament id.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            var patchRequest = new PatchTournamentMemberAuthenticatedRequest( authorizedUser ) {
                TournamentId = UnknownTournamentId(),
                MatchId = KnownPublicMatchOwner1,
                ApprovalStatus = ApprovalStatus.APPROVED
            };

            var response = await client.PatchTournamentMemberAuthenticatedAsync( patchRequest );

            Assert.AreEqual( HttpStatusCode.NotFound, response.RestApiStatusCode );
        }

        [TestMethod]
        public async Task PatchTournamentMemberReturnsNotFoundWhenMatchIsNotTournamentMember() {
            // Intention: verify PatchTournamentMember failure path when match is not currently a member of the tournament.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    UniqueName( "Full Tournament Patch Missing Member" ),
                    VisibilityOption.PUBLIC,
                    showOnSearch: false,
                    memberPolicy: MemberPolicyOption.REQUEST );

                var patchRequest = new PatchTournamentMemberAuthenticatedRequest( authorizedUser ) {
                    TournamentId = tournamentId,
                    MatchId = KnownPublicMatchOwner1,
                    ApprovalStatus = ApprovalStatus.APPROVED
                };

                var response = await client.PatchTournamentMemberAuthenticatedAsync( patchRequest );

                Assert.AreEqual( HttpStatusCode.NotFound, response.RestApiStatusCode );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task PatchTournamentMemberReturnsUnauthorizedWhenCallerLacksApprovalPermission() {
            // Intention: verify PatchTournamentMember failure when caller lacks REQUEST-policy approval permissions on the tournament.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );
            var lowPrivilegeUser = await AuthenticateAsync( Constants.TestDev11Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    UniqueName( "Full Tournament Patch Unauthorized" ),
                    VisibilityOption.PUBLIC,
                    showOnSearch: false,
                    memberPolicy: MemberPolicyOption.REQUEST );

                var addResponse = await client.AddTournamentMemberAuthenticatedAsync( tournamentId, KnownPublicMatchOwner1, authorizedUser );
                Assert.AreEqual( HttpStatusCode.OK, addResponse.RestApiStatusCode );

                var patchRequest = new PatchTournamentMemberAuthenticatedRequest( lowPrivilegeUser ) {
                    TournamentId = tournamentId,
                    MatchId = KnownPublicMatchOwner1,
                    ApprovalStatus = ApprovalStatus.APPROVED
                };

                var response = await client.PatchTournamentMemberAuthenticatedAsync( patchRequest );

                Assert.AreEqual( HttpStatusCode.Unauthorized, response.RestApiStatusCode );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task PatchTournamentMemberRejectsOpenPolicyTournaments() {
            // Intention: verify PatchTournamentMember edge failure where OPEN-policy tournaments do not allow approval updates.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    UniqueName( "Full Tournament Patch Open Policy" ),
                    VisibilityOption.PUBLIC,
                    showOnSearch: false,
                    memberPolicy: MemberPolicyOption.OPEN );

                var addResponse = await client.AddTournamentMemberAuthenticatedAsync( tournamentId, KnownPublicMatchOwner1003, authorizedUser );
                Assert.AreEqual( HttpStatusCode.OK, addResponse.RestApiStatusCode );

                var patchRequest = new PatchTournamentMemberAuthenticatedRequest( authorizedUser ) {
                    TournamentId = tournamentId,
                    MatchId = KnownPublicMatchOwner1003,
                    ApprovalStatus = ApprovalStatus.REJECTED
                };

                var response = await client.PatchTournamentMemberAuthenticatedAsync( patchRequest );

                Assert.AreEqual( HttpStatusCode.BadRequest, response.RestApiStatusCode );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task PatchTournamentMemberRequestThrowsForUnknownApprovalStatus() {
            // Intention: verify edge validation in the request model blocks unknown approval-status values before API dispatch.
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            var patchRequest = new PatchTournamentMemberAuthenticatedRequest( authorizedUser ) {
                TournamentId = UnknownTournamentId(),
                MatchId = KnownPublicMatchOwner1,
                ApprovalStatus = ApprovalStatus.UNKNOWN
            };

            Assert.ThrowsException<ArgumentOutOfRangeException>( () => {
                _ = patchRequest.QueryParameters;
            } );
        }

        [TestMethod]
        public async Task DeleteTournamentMemberRemovesExistingTournamentMember() {
            // Intention: verify DeleteTournamentMember success path removes an existing member and returns DELETED status.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    UniqueName( "Full Tournament Delete Member Success" ),
                    VisibilityOption.PUBLIC,
                    showOnSearch: false,
                    memberPolicy: MemberPolicyOption.INVITE );

                var addResponse = await client.AddTournamentMemberAuthenticatedAsync( tournamentId, KnownPublicMatchOwner1003, authorizedUser );
                Assert.AreEqual( HttpStatusCode.OK, addResponse.RestApiStatusCode );

                var deleteResponse = await client.DeleteTournamentMemberAuthenticatedAsync( tournamentId, KnownPublicMatchOwner1003, authorizedUser );

                Assert.AreEqual( HttpStatusCode.OK, deleteResponse.RestApiStatusCode );
                Assert.AreEqual( tournamentId, deleteResponse.TournamentMember.TournamentId );
                Assert.AreEqual( KnownPublicMatchOwner1003, deleteResponse.TournamentMember.MatchId );
                Assert.AreEqual( ApprovalStatus.DELETED, deleteResponse.TournamentMember.ApprovalStatus );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task DeleteTournamentMemberReturnsNotFoundWhenTournamentDoesNotExist() {
            // Intention: verify DeleteTournamentMember failure path for a non-existent tournament id.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            var response = await client.DeleteTournamentMemberAuthenticatedAsync( UnknownTournamentId(), KnownPublicMatchOwner1003, authorizedUser );

            Assert.AreEqual( HttpStatusCode.NotFound, response.RestApiStatusCode );
        }

        [TestMethod]
        public async Task DeleteTournamentMemberReturnsNotFoundWhenMatchDoesNotExist() {
            // Intention: verify DeleteTournamentMember failure path for a non-existent match id.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    UniqueName( "Full Tournament Delete Member Missing Match" ),
                    VisibilityOption.PUBLIC,
                    showOnSearch: false );

                var response = await client.DeleteTournamentMemberAuthenticatedAsync( tournamentId, UnknownMatchId(), authorizedUser );

                Assert.AreEqual( HttpStatusCode.NotFound, response.RestApiStatusCode );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task DeleteTournamentMemberReturnsBadRequestWhenRelationshipDoesNotExist() {
            // Intention: verify DeleteTournamentMember failure path when the match exists but is not a member of the tournament.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    UniqueName( "Full Tournament Delete Member Missing Relationship" ),
                    VisibilityOption.PUBLIC,
                    showOnSearch: false );

                var response = await client.DeleteTournamentMemberAuthenticatedAsync( tournamentId, KnownPublicMatchOwner1003, authorizedUser );

                Assert.AreEqual( HttpStatusCode.BadRequest, response.RestApiStatusCode );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task DeleteTournamentMemberReturnsUnauthorizedForCallerWithoutPermission() {
            // Intention: verify DeleteTournamentMember failure when caller lacks both tournament.remove_member and match.leave permissions.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );
            var lowPrivilegeUser = await AuthenticateAsync( Constants.TestDev11Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    UniqueName( "Full Tournament Delete Member Unauthorized" ),
                    VisibilityOption.PUBLIC,
                    showOnSearch: false,
                    memberPolicy: MemberPolicyOption.INVITE );

                var addResponse = await client.AddTournamentMemberAuthenticatedAsync( tournamentId, KnownPublicMatchOwner1003, authorizedUser );
                Assert.AreEqual( HttpStatusCode.OK, addResponse.RestApiStatusCode );

                var response = await client.DeleteTournamentMemberAuthenticatedAsync( tournamentId, KnownPublicMatchOwner1003, lowPrivilegeUser );

                Assert.AreEqual( HttpStatusCode.Unauthorized, response.RestApiStatusCode );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task TournamentSearchReturnsCreatedTournamentWhenFilteredByNameAndOwner() {
            // Intention: verify TournamentSearch success path returns a newly created tournament using name/owner filters.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                var tournamentName = UniqueName( "Full Tournament Search Name Filter" );
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    tournamentName,
                    VisibilityOption.PUBLIC,
                    showOnSearch: true );

                var request = new TournamentSearchAuthenticatedRequest( authorizedUser ) {
                    Name = tournamentName,
                    OwnerId = TournamentOwnerId,
                    Visibility = VisibilityOption.PUBLIC,
                    Limit = 10
                };

                var response = await client.TournamentSearchAuthenticatedAsync( request );

                Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
                Assert.IsTrue( response.TournamentSearchList.Items.Any( x => x.TournamentId.Equals( tournamentId ) ) );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task TournamentSearchSupportsContinuationTokenPaging() {
            // Intention: verify TournamentSearch edge behavior for paging with NextToken and GetNextRequest().
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            var createdIds = new List<MatchID>();
            try {
                var namePrefix = UniqueName( "Full Tournament Search Pagination" );

                var firstTournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    $"{namePrefix} A",
                    VisibilityOption.PUBLIC,
                    showOnSearch: true );
                createdIds.Add( firstTournamentId );

                var secondTournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    $"{namePrefix} B",
                    VisibilityOption.PUBLIC,
                    showOnSearch: true );
                createdIds.Add( secondTournamentId );

                var request = new TournamentSearchAuthenticatedRequest( authorizedUser ) {
                    Name = namePrefix,
                    OwnerId = TournamentOwnerId,
                    Visibility = VisibilityOption.PUBLIC,
                    Limit = 1
                };

                var firstPage = await client.TournamentSearchAuthenticatedAsync( request );

                Assert.AreEqual( HttpStatusCode.OK, firstPage.RestApiStatusCode );
                Assert.IsTrue( firstPage.TournamentSearchList.Items.Count == 1 );
                Assert.IsTrue( firstPage.HasMoreItems );

                var nextRequest = firstPage.GetNextRequest();
                Assert.IsFalse( string.IsNullOrWhiteSpace( nextRequest.Token ) );

                var secondPage = await client.TournamentSearchAuthenticatedAsync( nextRequest );

                Assert.AreEqual( HttpStatusCode.OK, secondPage.RestApiStatusCode );
                Assert.IsTrue( secondPage.TournamentSearchList.Items.Count >= 1 );

                var returnedIds = new HashSet<string>( firstPage.TournamentSearchList.Items.Select( x => x.TournamentId.ToString() ) );
                foreach (var tournament in secondPage.TournamentSearchList.Items) {
                    returnedIds.Add( tournament.TournamentId.ToString() );
                }

                foreach (var createdId in createdIds) {
                    Assert.IsTrue( returnedIds.Contains( createdId.ToString() ) );
                }
            } finally {
                foreach (var createdId in createdIds) {
                    await TryDeleteTournamentAsync( client, createdId, authorizedUser );
                }
            }
        }

        [TestMethod]
        public async Task TournamentSearchRejectsIncomingInvitesPresetWithoutMemberOwnerFilter() {
            // Intention: verify TournamentSearch validation failure when incoming-invites preset is missing member-has-owner-id.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            var request = new TournamentSearchAuthenticatedRequest( authorizedUser ) {
                Visibility = VisibilityOption.PUBLIC,
                SearchPreset = TournamentSearchPresetOption.INCOMING_INVITES,
                Limit = 10
            };

            var response = await client.TournamentSearchAuthenticatedAsync( request );

            Assert.AreEqual( HttpStatusCode.BadRequest, response.RestApiStatusCode );
        }

        [TestMethod]
        public async Task TournamentSearchFiltersOutPrivateTournamentsWithoutReadPermission() {
            // Intention: verify TournamentSearch does not return private owner tournaments to callers without tournament.read on that owner.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );
            var lowPrivilegeUser = await AuthenticateAsync( Constants.TestDev11Credentials );

            MatchID? tournamentId = null;
            try {
                var tournamentName = UniqueName( "Full Tournament Search Private Visibility" );
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    tournamentName,
                    VisibilityOption.PRIVATE,
                    showOnSearch: true );

                var request = new TournamentSearchAuthenticatedRequest( lowPrivilegeUser ) {
                    Name = tournamentName,
                    OwnerId = TournamentOwnerId,
                    Visibility = VisibilityOption.PRIVATE,
                    Limit = 10
                };

                var response = await client.TournamentSearchAuthenticatedAsync( request );

                Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
                Assert.AreEqual( 0, response.TournamentSearchList.Items.Count );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task TournamentSearchIncludesFilteredTournamentMembersWhenMemberFiltersAreProvided() {
            // Intention: verify TournamentSearch edge behavior where member filters return abbreviated tournaments with matching TournamentMembers populated.
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                var tournamentName = UniqueName( "Full Tournament Search Member Filter" );
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    tournamentName,
                    VisibilityOption.PUBLIC,
                    showOnSearch: true,
                    memberPolicy: MemberPolicyOption.INVITE );

                var addResponse = await client.AddTournamentMemberAuthenticatedAsync( tournamentId, KnownPublicMatchOwner1003, authorizedUser );
                Assert.AreEqual( HttpStatusCode.OK, addResponse.RestApiStatusCode );

                var request = new TournamentSearchAuthenticatedRequest( authorizedUser ) {
                    Name = tournamentName,
                    Visibility = VisibilityOption.PUBLIC,
                    MemberHasOwnerId = MemberOwnerId,
                    Limit = 10
                };

                var response = await client.TournamentSearchAuthenticatedAsync( request );

                Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );

                var tournament = response.TournamentSearchList.Items.FirstOrDefault( x => x.TournamentId.Equals( tournamentId ) );
                Assert.IsNotNull( tournament );
                Assert.IsTrue( tournament!.TournamentMembers.Count >= 1 );
                Assert.IsTrue( tournament.TournamentMembers.Any( x => x.MatchId.Equals( KnownPublicMatchOwner1003 ) ) );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }
    }
}
