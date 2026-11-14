using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Responses.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.DataModel.OrionMatchTests.RangeReporter {
    [TestClass]
    public class LeagueRangeReporterTests : BaseTestClass {

        private static readonly MatchID RegularSeasonLeagueId = new MatchID( "1.1.2025071411032772.3" );
        private static readonly MatchID RegularSeasonGameId = new MatchID( "1.1.2025111411171861.1" );
        private static readonly MatchID PostseasonLeagueId = new MatchID( "1.1.2025111714363286.3" );
        private static readonly MatchID PostseasonGameId = new MatchID( "1.1.2025121511184178.1" );

        private static async Task<UserAuthentication> AuthenticateAsync( BasicUserCredentials credentials ) {
            var userAuthentication = new UserAuthentication( credentials.Username, credentials.Password );
            await userAuthentication.InitializeAsync();
            return userAuthentication;
        }

        private static async Task<GetRangeReportAuthenticatedResponse> WaitForCompletedReportAsync(
            OrionMatchAPIClient client,
            MatchID leagueId,
            MatchID matchId,
            UserAuthentication credentials ) {
            var timeout = DateTime.UtcNow.AddSeconds( 30 );
            GetRangeReportAuthenticatedResponse response;

            do {
                await Task.Delay( 2000 );
                response = await client.GetLeagueRangeReportAuthenticatedAsync(
                    leagueId,
                    matchId,
                    credentials );

                Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            } while (
                response.RangeReport.GenerationStatus is RangeReportStatus.QUEUED or RangeReportStatus.IN_PROGRESS
                && DateTime.UtcNow < timeout );

            return response;
        }

        [TestMethod]
        [TestCategory( "Integration" )]
        [Timeout( 60000 )]
        public async Task GenerateLeagueRangeReportDryRunCreatesReport() {
            var credentials = await AuthenticateAsync( Constants.TestDev7Credentials );
            var client = new OrionMatchAPIClient( APIStage.PRODUCTION );
            var request = new GenerateLeagueRangeReportAuthenticatedRequest(
                PostseasonLeagueId,
                PostseasonGameId,
                credentials ) {
                CourseOfFireId = 1,
                DryRun = true,
                UserContext = new List<string> {
                    "League RangeReporter integration dry-run test."
                }
            };

            var response = await client.GenerateLeagueRangeReportAuthenticatedAsync( request );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            Assert.AreEqual( RangeReportStatus.QUEUED, response.RangeReport.GenerationStatus );
            Assert.AreEqual( RangeReportKind.LEAGUE_GAME, response.RangeReport.ReportKind );
            Assert.AreEqual( PostseasonLeagueId, response.RangeReport.LeagueId );
            Assert.AreEqual( PostseasonGameId.ToString(), response.RangeReport.MatchId );

            var getResponse = await WaitForCompletedReportAsync(
                client,
                PostseasonLeagueId,
                PostseasonGameId,
                credentials );

            Assert.AreEqual(
                RangeReportStatus.COMPLETED,
                getResponse.RangeReport.GenerationStatus,
                getResponse.RangeReport.GenerationError );
            Assert.AreEqual( RangeReportKind.LEAGUE_GAME, getResponse.RangeReport.ReportKind );
            Assert.AreEqual( PostseasonLeagueId, getResponse.RangeReport.LeagueId );
            Assert.AreEqual( PostseasonGameId.ToString(), getResponse.RangeReport.MatchId );
            Assert.AreEqual( 1, getResponse.RangeReport.CourseOfFireId );
            Assert.IsFalse( getResponse.RangeReport.Published );
            Assert.AreEqual( false, getResponse.RangeReport.AiGenerated );
            Assert.IsTrue( getResponse.RangeReport.FormattedHtml?.Contains( "Dry run RangeReporter placeholder" ) );
        }

        [TestMethod]
        [TestCategory( "Integration" )]
        public async Task GetLeagueRangeReportReturnsReport() {
            var credentials = await AuthenticateAsync( Constants.TestDev7Credentials );
            var client = new OrionMatchAPIClient( APIStage.PRODUCTION );

            var response = await client.GetLeagueRangeReportAuthenticatedAsync(
                PostseasonLeagueId,
                PostseasonGameId,
                credentials );

            Assert.AreEqual(
                HttpStatusCode.OK,
                response.RestApiStatusCode,
                string.Join( "; ", response.MessageResponse.Message ) );
            Assert.AreEqual( RangeReportKind.LEAGUE_GAME, response.RangeReport.ReportKind );
            Assert.AreEqual( PostseasonLeagueId, response.RangeReport.LeagueId );
            Assert.AreEqual( PostseasonGameId.ToString(), response.RangeReport.MatchId );
            Assert.IsFalse( string.IsNullOrWhiteSpace( response.RangeReport.ResultListName ) );
        }

        [TestMethod]
        [TestCategory( "Integration" )]
        public async Task GenerateLeagueRangeReportReturnsBadRequestForWrongResultName() {
            var credentials = await AuthenticateAsync( Constants.TestDev7Credentials );
            var client = new OrionMatchAPIClient( APIStage.PRODUCTION );
            var request = new GenerateLeagueRangeReportAuthenticatedRequest(
                PostseasonLeagueId,
                PostseasonGameId,
                credentials ) {
                ResultListName = "Not the configured team result list",
                DryRun = true
            };

            var response = await client.GenerateLeagueRangeReportAuthenticatedAsync( request );

            Assert.AreEqual( HttpStatusCode.BadRequest, response.RestApiStatusCode );
        }

        [TestMethod]
        [TestCategory( "Integration" )]
        public async Task GenerateLeagueRangeReportReturnsNotFoundForGameFromDifferentLeague() {
            var credentials = await AuthenticateAsync( Constants.TestDev7Credentials );
            var client = new OrionMatchAPIClient( APIStage.PRODUCTION );
            var request = new GenerateLeagueRangeReportAuthenticatedRequest(
                PostseasonLeagueId,
                RegularSeasonGameId,
                credentials ) {
                DryRun = true
            };

            var response = await client.GenerateLeagueRangeReportAuthenticatedAsync( request );

            Assert.AreEqual( HttpStatusCode.NotFound, response.RestApiStatusCode );
        }

        [TestMethod]
        [TestCategory( "Integration" )]
        public async Task GetLeagueRangeReportReturnsBadRequestForWrongResultName() {
            var credentials = await AuthenticateAsync( Constants.TestDev7Credentials );
            var client = new OrionMatchAPIClient( APIStage.PRODUCTION );
            var request = new GetLeagueRangeReportAuthenticatedRequest(
                PostseasonLeagueId,
                PostseasonGameId,
                credentials,
                "Not the configured team result list" );

            var response = await client.GetLeagueRangeReportAuthenticatedAsync( request );

            Assert.AreEqual( HttpStatusCode.BadRequest, response.RestApiStatusCode );
        }

        [TestMethod]
        [TestCategory( "Integration" )]
        public async Task GetLeagueRangeReportReturnsNotFoundForGameFromDifferentLeague() {
            var credentials = await AuthenticateAsync( Constants.TestDev7Credentials );
            var client = new OrionMatchAPIClient( APIStage.PRODUCTION );

            var response = await client.GetLeagueRangeReportAuthenticatedAsync(
                PostseasonLeagueId,
                RegularSeasonGameId,
                credentials );

            Assert.AreEqual( HttpStatusCode.NotFound, response.RestApiStatusCode );
        }
    }
}
