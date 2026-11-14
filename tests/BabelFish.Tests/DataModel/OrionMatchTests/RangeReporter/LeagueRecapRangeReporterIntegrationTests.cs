using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Responses.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.DataModel.OrionMatchTests.RangeReporter {
    [TestClass]
    [DoNotParallelize]
    public class LeagueRecapRangeReporterIntegrationTests : BaseTestClass {

        private static readonly MatchID LeagueId = new MatchID( "1.1.2025071411032772.3" );
        private static readonly DateTime StartDate = new DateTime( 2025, 11, 3 );
        private static readonly DateTime EndDate = new DateTime( 2025, 11, 9 );

        private UserAuthentication Credentials = null!;
        private OrionMatchAPIClient Client = null!;
        private GenerateRangeReportAuthenticatedResponse GenerateResponse = null!;
        private GetRangeReportAuthenticatedResponse CompletedResponse = null!;

        private async Task InitializeRecapAsync() {
            Credentials = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await Credentials.InitializeAsync();
            Client = new OrionMatchAPIClient( APIStage.PRODUCTION );

            var request = new GenerateLeagueRecapRangeReportAuthenticatedRequest(
                LeagueId,
                StartDate,
                EndDate,
                Credentials ) {
                DryRun = true,
                UserContext = new List<string> {
                    "BabelFish live integration dry-run test."
                }
            };

            GenerateResponse = await Client.GenerateLeagueRecapRangeReportAuthenticatedAsync( request );

            Assert.AreEqual(
                HttpStatusCode.OK,
                GenerateResponse.RestApiStatusCode,
                string.Join( "; ", GenerateResponse.MessageResponse.Message ) );
            CompletedResponse = await WaitForCompletedRecapAsync();
        }

        private async Task<GetRangeReportAuthenticatedResponse> GetRecapAsync() {
            var request = new GetLeagueRecapRangeReportAuthenticatedRequest(
                LeagueId,
                StartDate,
                EndDate,
                Credentials ) {
                IgnoreInMemoryCache = true
            };

            return await Client.GetLeagueRecapRangeReportAuthenticatedAsync( request );
        }

        private async Task<GetRangeReportPublicResponse> GetPublicRecapAsync() {
            var request = new GetLeagueRecapRangeReportPublicRequest(
                LeagueId,
                StartDate,
                EndDate ) {
                IgnoreInMemoryCache = true
            };

            return await Client.GetLeagueRecapRangeReportPublicAsync( request );
        }

        private async Task SetPublishedAsync( bool published ) {
            var request = new PatchLeagueRecapRangeReportAuthenticatedRequest(
                LeagueId,
                StartDate,
                EndDate,
                Credentials ) {
                Published = published
            };
            var response = await Client.PatchLeagueRecapRangeReportAuthenticatedAsync( request );

            Assert.AreEqual(
                HttpStatusCode.OK,
                response.RestApiStatusCode,
                string.Join( "; ", response.MessageResponse.Message ) );
            Assert.AreEqual( published, response.RangeReport.Published );
        }

        private async Task<GetRangeReportAuthenticatedResponse> WaitForCompletedRecapAsync() {
            var timeout = DateTime.UtcNow.AddSeconds( 90 );
            GetRangeReportAuthenticatedResponse response;

            do {
                response = await GetRecapAsync();

                Assert.AreEqual(
                    HttpStatusCode.OK,
                    response.RestApiStatusCode,
                    string.Join( "; ", response.MessageResponse.Message ) );

                if (response.RangeReport.GenerationStatus is RangeReportStatus.QUEUED or RangeReportStatus.IN_PROGRESS) {
                    await Task.Delay( 2000 );
                }
            } while (
                response.RangeReport.GenerationStatus is RangeReportStatus.QUEUED or RangeReportStatus.IN_PROGRESS
                && DateTime.UtcNow < timeout );

            Assert.AreEqual(
                RangeReportStatus.COMPLETED,
                response.RangeReport.GenerationStatus,
                response.RangeReport.GenerationError );
            return response;
        }

        [TestMethod]
        [TestCategory( "Integration" )]
        public async Task GenerateLeagueRecapRangeReportDryRunQueuesAndCompletes() {
            await InitializeRecapAsync();

            Assert.AreEqual( RangeReportKind.LEAGUE_RECAP, GenerateResponse.RangeReport.ReportKind );
            Assert.AreEqual( LeagueId, GenerateResponse.RangeReport.LeagueId );
            Assert.AreEqual( StartDate, GenerateResponse.RangeReport.StartDate.GetValueOrDefault() );
            Assert.AreEqual( EndDate, GenerateResponse.RangeReport.EndDate.GetValueOrDefault() );
            Assert.AreEqual( RangeReportStatus.QUEUED, GenerateResponse.RangeReport.GenerationStatus );
            Assert.IsTrue( GenerateResponse.RangeReport.DryRun.GetValueOrDefault() );

            Assert.AreEqual( RangeReportStatus.COMPLETED, CompletedResponse.RangeReport.GenerationStatus );
            Assert.IsFalse( CompletedResponse.RangeReport.AiGenerated.GetValueOrDefault() );
            Assert.IsTrue( CompletedResponse.RangeReport.DryRun.GetValueOrDefault() );
        }

        [TestMethod]
        [TestCategory( "Integration" )]
        public async Task GetLeagueRecapRangeReportReturnsCompletedDryRun() {
            await InitializeRecapAsync();

            var response = await GetRecapAsync();

            Assert.AreEqual(
                HttpStatusCode.OK,
                response.RestApiStatusCode,
                string.Join( "; ", response.MessageResponse.Message ) );
            Assert.AreEqual( RangeReportKind.LEAGUE_RECAP, response.RangeReport.ReportKind );
            Assert.AreEqual( LeagueId, response.RangeReport.LeagueId );
            Assert.AreEqual( StartDate, response.RangeReport.StartDate.GetValueOrDefault() );
            Assert.AreEqual( EndDate, response.RangeReport.EndDate.GetValueOrDefault() );
            Assert.AreEqual( RangeReportStatus.COMPLETED, response.RangeReport.GenerationStatus );
            Assert.IsFalse( response.RangeReport.AiGenerated.GetValueOrDefault() );
            Assert.IsTrue( response.RangeReport.DryRun.GetValueOrDefault() );
            Assert.IsFalse( response.RangeReport.Published );
            Assert.IsFalse( string.IsNullOrWhiteSpace( response.RangeReport.FormattedHtml ) );
        }

        [TestMethod]
        [TestCategory( "Integration" )]
        public async Task GetLeagueRecapRangeReportPublicRejectsUnpublishedRecap() {
            await InitializeRecapAsync();

            var originalPublished = (await GetRecapAsync()).RangeReport.Published;

            try {
                await SetPublishedAsync( false );

                var response = await GetPublicRecapAsync();

                Assert.AreEqual( HttpStatusCode.Unauthorized, response.RestApiStatusCode );
            } finally {
                await SetPublishedAsync( originalPublished );
            }
        }

        [TestMethod]
        [TestCategory( "Integration" )]
        public async Task GetLeagueRecapRangeReportPublicReturnsPublishedRecap() {
            await InitializeRecapAsync();

            var originalPublished = (await GetRecapAsync()).RangeReport.Published;

            try {
                await SetPublishedAsync( true );

                var response = await GetPublicRecapAsync();

                Assert.AreEqual(
                    HttpStatusCode.OK,
                    response.RestApiStatusCode,
                    string.Join( "; ", response.MessageResponse.Message ) );
                Assert.IsTrue( response.RangeReport.Published );
                Assert.AreEqual( RangeReportKind.LEAGUE_RECAP, response.RangeReport.ReportKind );
                Assert.AreEqual( LeagueId, response.RangeReport.LeagueId );
                Assert.AreEqual( StartDate, response.RangeReport.StartDate.GetValueOrDefault() );
                Assert.AreEqual( EndDate, response.RangeReport.EndDate.GetValueOrDefault() );
                Assert.AreEqual( RangeReportStatus.COMPLETED, response.RangeReport.GenerationStatus );
                Assert.IsFalse( response.RangeReport.AiGenerated.GetValueOrDefault() );
            } finally {
                await SetPublishedAsync( originalPublished );
            }
        }

        [TestMethod]
        [TestCategory( "Integration" )]
        public async Task PatchLeagueRecapRangeReportUpdatesCompletedDryRun() {
            await InitializeRecapAsync();

            var beforePatchResponse = await GetRecapAsync();
            Assert.AreEqual( HttpStatusCode.OK, beforePatchResponse.RestApiStatusCode );
            Assert.AreEqual( RangeReportStatus.COMPLETED, beforePatchResponse.RangeReport.GenerationStatus );
            Assert.IsFalse( beforePatchResponse.RangeReport.AiGenerated.GetValueOrDefault() );
            Assert.IsFalse( string.IsNullOrWhiteSpace( beforePatchResponse.RangeReport.FormattedHtml ) );

            var originalPublished = beforePatchResponse.RangeReport.Published;
            var originalFormattedHtml = beforePatchResponse.RangeReport.FormattedHtml!;
            var patchedFormattedHtml = $"<article>BabelFish integration patch test {Guid.NewGuid()}</article>";
            var request = new PatchLeagueRecapRangeReportAuthenticatedRequest(
                LeagueId,
                StartDate,
                EndDate,
                Credentials ) {
                Published = true,
                FormattedHtml = patchedFormattedHtml
            };

            try {
                var response = await Client.PatchLeagueRecapRangeReportAuthenticatedAsync( request );

                Assert.AreEqual(
                    HttpStatusCode.OK,
                    response.RestApiStatusCode,
                    string.Join( "; ", response.MessageResponse.Message ) );
                Assert.IsTrue( response.RangeReport.Published );
                Assert.AreEqual( patchedFormattedHtml, response.RangeReport.FormattedHtml );
                Assert.AreEqual( RangeReportStatus.COMPLETED, response.RangeReport.GenerationStatus );
                Assert.IsFalse( response.RangeReport.AiGenerated.GetValueOrDefault() );

                var afterPatchResponse = await GetRecapAsync();

                Assert.AreEqual( HttpStatusCode.OK, afterPatchResponse.RestApiStatusCode );
                Assert.IsTrue( afterPatchResponse.RangeReport.Published );
                Assert.AreEqual( patchedFormattedHtml, afterPatchResponse.RangeReport.FormattedHtml );
                Assert.AreEqual( RangeReportStatus.COMPLETED, afterPatchResponse.RangeReport.GenerationStatus );
                Assert.IsFalse( afterPatchResponse.RangeReport.AiGenerated.GetValueOrDefault() );
            } finally {
                var restoreRequest = new PatchLeagueRecapRangeReportAuthenticatedRequest(
                    LeagueId,
                    StartDate,
                    EndDate,
                    Credentials ) {
                    Published = originalPublished,
                    FormattedHtml = originalFormattedHtml
                };
                var restoreResponse = await Client.PatchLeagueRecapRangeReportAuthenticatedAsync(
                    restoreRequest );

                Assert.AreEqual( HttpStatusCode.OK, restoreResponse.RestApiStatusCode );
                Assert.AreEqual( originalPublished, restoreResponse.RangeReport.Published );
                Assert.AreEqual( originalFormattedHtml, restoreResponse.RangeReport.FormattedHtml );

                var restoredGetResponse = await GetRecapAsync();
                Assert.AreEqual( HttpStatusCode.OK, restoredGetResponse.RestApiStatusCode );
                Assert.AreEqual( originalPublished, restoredGetResponse.RangeReport.Published );
                Assert.AreEqual( originalFormattedHtml, restoredGetResponse.RangeReport.FormattedHtml );
            }
        }
    }
}
