using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.DataModel.OrionMatch.Tournament {
    [TestClass]
    public class PatchTournamentTests : BaseTestClass {

        private const string TournamentOwnerId = "OrionAcct000002";

        private static OrionMatchAPIClient CreateClient() => new OrionMatchAPIClient( APIStage.PRODUCTION );

        private static string UniqueName( string prefix ) {
            return $"{prefix} {DateTime.UtcNow:yyyyMMddHHmmssfff}";
        }

        private static MatchID UnknownTournamentId() {
            return new MatchID( $"1.999999.{DateTime.UtcNow.Ticks}.2" );
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
            VisibilityOption visibility ) {

            var request = new CreateTournamentAuthenticatedRequest( credentials ) {
                TournamentName = name,
                OwnerId = TournamentOwnerId,
                Visibility = visibility,
                ShowOnSearch = false,
                MemberPolicy = MemberPolicyOption.INVITE
            };

            var response = await client.CreateTournamentAuthenticatedAsync( request );
            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode, "Tournament setup failed for PatchTournament test." );
            return response.Tournament.TournamentId;
        }

        [TestMethod]
        public async Task PatchTournamentWithRequestUpdatesNameAndVisibility() {
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    UniqueName( "Patch Tournament Request Original" ),
                    VisibilityOption.PRIVATE );

                var updatedName = UniqueName( "Patch Tournament Request Updated" );
                var request = new PatchTournamentAuthenticatedRequest( authorizedUser ) {
                    TournamentId = tournamentId,
                    TournamentName = updatedName,
                    Visibility = VisibilityOption.PUBLIC
                };

                var response = await client.PatchTournamentAuthenticatedAsync( request );

                Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
                Assert.AreEqual( tournamentId, response.Tournament.TournamentId );
                Assert.AreEqual( updatedName, response.Tournament.TournamentName );
                Assert.AreEqual( VisibilityOption.PUBLIC, response.Tournament.Visibility );
                Assert.IsTrue( response.Tournament.Abbreviated );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task PatchTournamentWithConvenienceOverloadUpdatesVisibilityOnly() {
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            MatchID? tournamentId = null;
            try {
                var originalName = UniqueName( "Patch Tournament Overload Original" );
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    originalName,
                    VisibilityOption.PUBLIC );

                var response = await client.PatchTournamentAuthenticatedAsync(
                    tournamentId,
                    tournamentName: null,
                    visibility: VisibilityOption.PRIVATE,
                    credentials: authorizedUser );

                Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
                Assert.AreEqual( tournamentId, response.Tournament.TournamentId );
                Assert.AreEqual( originalName, response.Tournament.TournamentName );
                Assert.AreEqual( VisibilityOption.PRIVATE, response.Tournament.Visibility );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task PatchTournamentReturnsNotFoundForUnknownTournamentId() {
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            var response = await client.PatchTournamentAuthenticatedAsync(
                UnknownTournamentId(),
                UniqueName( "Patch Tournament Missing" ),
                visibility: null,
                credentials: authorizedUser );

            Assert.AreEqual( HttpStatusCode.NotFound, response.RestApiStatusCode );
        }

        [TestMethod]
        public async Task PatchTournamentReturnsUnauthorizedForCallerWithoutEditPermission() {
            var client = CreateClient();
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );
            var unauthorizedUser = await AuthenticateAsync( Constants.TestDev11Credentials );

            MatchID? tournamentId = null;
            try {
                tournamentId = await CreateTournamentAsync(
                    client,
                    authorizedUser,
                    UniqueName( "Patch Tournament Unauthorized Original" ),
                    VisibilityOption.PUBLIC );

                var response = await client.PatchTournamentAuthenticatedAsync(
                    tournamentId,
                    UniqueName( "Patch Tournament Unauthorized Updated" ),
                    visibility: null,
                    credentials: unauthorizedUser );

                Assert.AreEqual( HttpStatusCode.Unauthorized, response.RestApiStatusCode );
            } finally {
                await TryDeleteTournamentAsync( client, tournamentId, authorizedUser );
            }
        }

        [TestMethod]
        public async Task PatchTournamentRequestThrowsWhenNoFieldsAreProvided() {
            var authorizedUser = await AuthenticateAsync( Constants.TestDev7Credentials );

            var request = new PatchTournamentAuthenticatedRequest( authorizedUser ) {
                TournamentId = UnknownTournamentId()
            };

            Assert.Throws<ArgumentException>( () => {
                _ = request.QueryParameters;
            } );
        }
    }
}
