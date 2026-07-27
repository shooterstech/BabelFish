using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Responses.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.DataModel.OrionMatchTests.RangeReporter {
    [TestClass]
    public class RangeReporterTests : BaseTestClass {

        private static readonly MatchID KnownRangeReporterMatchId = new MatchID("1.1.2021020310584218.1");
        private const string KnownRangeReporterResultName = "Individual - All";

        private static UserAuthentication CreateAuthentication() {
            return new UserAuthentication( "range-reporter@example.com", "password" );
        }

        private static async Task<UserAuthentication> AuthenticateAsync( BasicUserCredentials credentials ) {
            var userAuthentication = new UserAuthentication( credentials.Username, credentials.Password );
            await userAuthentication.InitializeAsync();
            return userAuthentication;
        }

        [TestMethod]
        public void GenerateRangeReportRequestBuildsExpectedApiContract() {
            var request = new GenerateRangeReportAuthenticatedRequest(
                new MatchID( "1.2063.2026043009084183.0" ),
                "Individual - Precision",
                CreateAuthentication() ) {
                CourseOfFireId = 1,
                MilestoneStrategy = "numshots",
                ShotMilestoneCounts = new List<int> { 10, 20, 30 },
                ExpectedShots = 60,
                SnapshotOrderBy = "Event",
                UserContext = new List<string> { "State championship", "Personal best" },
                DryRun = true
            };

            var queryParameters = request.QueryParameters;

            Assert.AreEqual( HttpMethod.Post, request.HttpMethod );
            Assert.AreEqual( "/range-reporter/1.2063.2026043009084183.0", request.RelativePath );
            Assert.AreEqual( APISubDomain.AUTHAPI, request.SubDomain );
            Assert.IsFalse( queryParameters.ContainsKey( "match-id" ) );
            Assert.AreEqual( "Individual - Precision", queryParameters["result-name"].Single() );
            Assert.AreEqual( "1", queryParameters["course-of-fire-id"].Single() );
            Assert.AreEqual( "numshots", queryParameters["milestone-strategy"].Single() );
            Assert.AreEqual( "10,20,30", queryParameters["shot-milestones"].Single() );
            Assert.AreEqual( "60", queryParameters["expected-shots"].Single() );
            Assert.AreEqual( "Event", queryParameters["snapshot-order-by"].Single() );
            CollectionAssert.AreEqual( new List<string> { "State championship", "Personal best" }, queryParameters["user-context"] );
            Assert.AreEqual( "True", queryParameters["dry-run"].Single() );
        }

        [TestMethod]
        public void GetRangeReportRequestBuildsExpectedApiContract() {
            var request = new GetRangeReportAuthenticatedRequest(
                new MatchID( "1.2063.2026043009084183.0" ),
                "Individual - Precision",
                CreateAuthentication() );

            var queryParameters = request.QueryParameters;

            Assert.AreEqual( HttpMethod.Get, request.HttpMethod );
            Assert.AreEqual( "/range-reporter/1.2063.2026043009084183.0", request.RelativePath );
            Assert.AreEqual( APISubDomain.AUTHAPI, request.SubDomain );
            Assert.AreEqual( "Individual - Precision", queryParameters["result-name"].Single() );
        }

        [TestMethod]
        public void GetRangeReportPublicRequestBuildsExpectedApiContract() {
            var request = new GetRangeReportPublicRequest(
                new MatchID( "1.2063.2026043009084183.0" ),
                "Individual - Precision" );

            var queryParameters = request.QueryParameters;
            var factoryRequest = GetRangeReportAbstractRequest.Factory(
                new MatchID( "1.2063.2026043009084183.0" ),
                "Individual - Precision" );

            Assert.AreEqual( HttpMethod.Get, request.HttpMethod );
            Assert.AreEqual( "/range-reporter/1.2063.2026043009084183.0", request.RelativePath );
            Assert.AreEqual( APISubDomain.API, request.SubDomain );
            Assert.AreEqual( "Individual - Precision", queryParameters["result-name"].Single() );
            Assert.IsInstanceOfType( factoryRequest, typeof( GetRangeReportPublicRequest ) );
        }

        [TestMethod]
        public void PatchRangeReportRequestBuildsExpectedApiContract() {
            var request = new PatchRangeReportAuthenticatedRequest(
                new MatchID( "1.2063.2026043009084183.0" ),
                "Individual - Precision",
                CreateAuthentication() ) {
                Published = false,
                FormattedHtml = "<div><h1>Updated</h1></div>"
            };

            var queryParameters = request.QueryParameters;

            Assert.AreEqual( new HttpMethod( "PATCH" ), request.HttpMethod );
            Assert.AreEqual( "/range-reporter/1.2063.2026043009084183.0", request.RelativePath );
            Assert.AreEqual( APISubDomain.AUTHAPI, request.SubDomain );
            Assert.AreEqual( "Individual - Precision", queryParameters["result-name"].Single() );
            Assert.AreEqual( "False", queryParameters["Published"].Single() );
            Assert.AreEqual( "<div><h1>Updated</h1></div>", queryParameters["FormattedHtml"].Single() );
        }

        [TestMethod]
        public void PatchRangeReportRequestRequiresPatchField() {
            var request = new PatchRangeReportAuthenticatedRequest(
                new MatchID( "1.2063.2026043009084183.0" ),
                "Individual - Precision",
                CreateAuthentication() );

            Assert.Throws<ArgumentException>( () => {
                _ = request.QueryParameters;
            } );
        }

        [TestMethod]
        public void RangeReportWrapperDeserializesApiResponse() {
            var json = """
            {
                "RangeReport": {
                    "Headline": "Patched RangeReporter Press Release",
                    "Paragraphs": [
                        "Updated by PatchRangeReport local test."
                    ],
                    "Published": true,
                    "MatchId": "1.2063.2026043009084183.0",
                    "LicenseNumber": 2063,
                    "ResultListName": "Individual - Precision",
                    "CourseOfFireId": 1,
                    "MilestoneStrategy": "numshots",
                    "ShotMilestoneCounts": [10, 20, 30],
                    "ExpectedShots": 60,
                    "SnapshotOrderBy": "Event",
                    "UserContext": [
                        "State championship"
                    ],
                    "S3Uri": "s3://cdn.scopos.tech/matches/1.2063.2026043009084183.0/range_reporter/Individual - Precision/range_reporter_press_release.json",
                    "Creator": "26f32227-d428-41f6-b224-beed7b6e8850",
                    "TokensRemaining": 4,
                    "AiGenerated": true,
                    "FormattedHtml": "<div><h1>Patched RangeReporter Press Release</h1></div>"
                }
            }
            """;

            var wrapper = G_STJ.JsonSerializer.Deserialize<RangeReportWrapper>( json, SerializerOptions.SystemTextJsonDeserializer );

            Assert.IsNotNull( wrapper );
            Assert.AreEqual( "Patched RangeReporter Press Release", wrapper.RangeReport.Headline );
            Assert.IsTrue( wrapper.RangeReport.Published );
            Assert.AreEqual( "1.2063.2026043009084183.0", wrapper.RangeReport.MatchId );
            Assert.AreEqual( 2063, wrapper.RangeReport.LicenseNumber );
            Assert.AreEqual( "Individual - Precision", wrapper.RangeReport.ResultListName );
            Assert.AreEqual( 1, wrapper.RangeReport.CourseOfFireId );
            Assert.AreEqual( "numshots", wrapper.RangeReport.MilestoneStrategy );
            CollectionAssert.AreEqual( new List<int> { 10, 20, 30 }, wrapper.RangeReport.ShotMilestoneCounts );
            Assert.AreEqual( 60, wrapper.RangeReport.ExpectedShots );
            Assert.AreEqual( "Event", wrapper.RangeReport.SnapshotOrderBy );
            CollectionAssert.AreEqual( new List<string> { "State championship" }, wrapper.RangeReport.UserContext );
            Assert.AreEqual( 4, wrapper.RangeReport.TokensRemaining );
            Assert.AreEqual( true, wrapper.RangeReport.AiGenerated );
            Assert.AreEqual( "<div><h1>Patched RangeReporter Press Release</h1></div>", wrapper.RangeReport.FormattedHtml );
        }

        [TestMethod]
        public async Task GenerateRangeReportDryRunCreatesReport() {
            var credentials = await AuthenticateAsync( Constants.TestDev7Credentials );
            var client = new OrionMatchAPIClient();

            var request = new GenerateRangeReportAuthenticatedRequest(
                KnownRangeReporterMatchId,
                KnownRangeReporterResultName,
                credentials ) {
                CourseOfFireId = 1,
                DryRun = true,
                UserContext = new List<string> {
                    "RangeReporter integration dry-run test."
                }
            };

            var response = await client.GenerateRangeReportAuthenticatedAsync( request );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            Assert.AreEqual( RangeReportStatus.QUEUED, response.RangeReport.GenerationStatus );

            //sleep 3 seconds
            await Task.Delay( 7000 );

            //GetRangeReport the report to verify it was created
            var getResponse = await client.GetRangeReportAuthenticatedAsync(
                KnownRangeReporterMatchId,
                KnownRangeReporterResultName,
                credentials );

            Assert.AreEqual( HttpStatusCode.OK, getResponse.RestApiStatusCode );
            Assert.AreEqual( RangeReportStatus.COMPLETED, getResponse.RangeReport.GenerationStatus );
            Assert.AreEqual( KnownRangeReporterMatchId.ToString(), getResponse.RangeReport.MatchId );
            Assert.AreEqual( KnownRangeReporterResultName, getResponse.RangeReport.ResultListName );
            Assert.AreEqual( 1, getResponse.RangeReport.CourseOfFireId );
            Assert.IsFalse( getResponse.RangeReport.Published );
            Assert.AreEqual( false, getResponse.RangeReport.AiGenerated );
            Assert.IsTrue( getResponse.RangeReport.FormattedHtml.Contains( "Dry run RangeReporter placeholder" ) );
        }

        [TestMethod]
        public async Task GetRangeReportReturnsReport() {
            var credentials = await AuthenticateAsync( Constants.TestDev7Credentials );
            var client = new OrionMatchAPIClient();

            var response = await client.GetRangeReportAuthenticatedAsync(
                KnownRangeReporterMatchId,
                KnownRangeReporterResultName,
                credentials );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            Assert.AreEqual( KnownRangeReporterMatchId.ToString(), response.RangeReport.MatchId );
            Assert.AreEqual( KnownRangeReporterResultName, response.RangeReport.ResultListName );
            Assert.IsNotNull( response.RangeReport.Paragraphs );
            Assert.IsFalse( string.IsNullOrWhiteSpace( response.RangeReport.FormattedHtml ) );
        }

        [TestMethod]
        public async Task GetRangeReportPublicReturnsReport() {
            var client = new OrionMatchAPIClient();
            var request = new GetRangeReportPublicRequest(
                KnownRangeReporterMatchId,
                KnownRangeReporterResultName );

            var response = await client.GetRangeReportPublicAsync( request );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            Assert.AreEqual( KnownRangeReporterMatchId.ToString(), response.RangeReport.MatchId );
            Assert.AreEqual( KnownRangeReporterResultName, response.RangeReport.ResultListName );
            Assert.IsNotNull( response.RangeReport.Paragraphs );
            Assert.IsFalse( string.IsNullOrWhiteSpace( response.RangeReport.FormattedHtml ) );
        }

        [TestMethod]
        public async Task PatchRangeReportUpdatesPublishedAndFormattedHtml() {
            var credentials = await AuthenticateAsync( Constants.TestDev7Credentials );
            var client = new OrionMatchAPIClient();
            var formattedHtml = $"<div><h1>Patched RangeReporter Test</h1><p>{DateTime.UtcNow:O}</p></div>";

            await client.GetRangeReportAuthenticatedAsync(
                KnownRangeReporterMatchId,
                KnownRangeReporterResultName,
                credentials );

            var response = await client.PatchRangeReportAuthenticatedAsync(
                KnownRangeReporterMatchId,
                KnownRangeReporterResultName,
                published: false,
                formattedHtml: formattedHtml,
                credentials: credentials );

            Assert.AreEqual( HttpStatusCode.OK, response.RestApiStatusCode );
            Assert.AreEqual( KnownRangeReporterMatchId.ToString(), response.RangeReport.MatchId );
            Assert.AreEqual( KnownRangeReporterResultName, response.RangeReport.ResultListName );
            Assert.IsFalse( response.RangeReport.Published );
            Assert.AreEqual( formattedHtml, response.RangeReport.FormattedHtml );
        }
    }
}
