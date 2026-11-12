using System.Linq;
using System.Net.Http;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Responses.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.DataModel.OrionMatchTests.RangeReporter {
    [TestClass]
    public class LeagueRecapRangeReporterTests : BaseTestClass {

        private static readonly MatchID LeagueId = new MatchID( "1.1.2025071411032772.3" );
        private static readonly DateTime StartDate = new DateTime( 2025, 11, 3 );
        private static readonly DateTime EndDate = new DateTime( 2025, 11, 9 );

        private static UserAuthentication CreateAuthentication() {
            return new UserAuthentication( "range-reporter@example.com", "password" );
        }

        [TestMethod]
        public void GenerateLeagueRecapRangeReportRequestBuildsExpectedApiContract() {
            var request = new GenerateLeagueRecapRangeReportAuthenticatedRequest(
                LeagueId,
                StartDate,
                EndDate,
                CreateAuthentication() ) {
                UserContext = new List<string> { "Championship week", "Three unbeaten teams" },
                DryRun = true,
                LocalTest = true
            };

            var queryParameters = request.QueryParameters;

            Assert.AreEqual( HttpMethod.Post, request.HttpMethod );
            Assert.AreEqual(
                "/range-reporter/league/1.1.2025071411032772.3/recap",
                request.RelativePath );
            Assert.AreEqual( APISubDomain.AUTHAPI, request.SubDomain );
            Assert.AreEqual( "2025-11-03", queryParameters["start-date"].Single() );
            Assert.AreEqual( "2025-11-09", queryParameters["end-date"].Single() );
            CollectionAssert.AreEqual(
                new List<string> { "Championship week", "Three unbeaten teams" },
                queryParameters["user-context"] );
            Assert.AreEqual( "True", queryParameters["dry-run"].Single() );
            Assert.AreEqual( "True", queryParameters["local-test"].Single() );
        }

        [TestMethod]
        public void GetLeagueRecapRangeReportRequestsBuildExpectedApiContracts() {
            var publicRequest = new GetLeagueRecapRangeReportPublicRequest(
                LeagueId,
                StartDate,
                EndDate );
            var authenticatedRequest = new GetLeagueRecapRangeReportAuthenticatedRequest(
                LeagueId,
                StartDate,
                EndDate,
                CreateAuthentication() );
            var publicFactoryRequest = GetLeagueRecapRangeReportAbstractRequest.Factory(
                LeagueId,
                StartDate,
                EndDate );
            var authenticatedFactoryRequest = GetLeagueRecapRangeReportAbstractRequest.Factory(
                LeagueId,
                StartDate,
                EndDate,
                CreateAuthentication() );

            Assert.AreEqual( HttpMethod.Get, publicRequest.HttpMethod );
            Assert.AreEqual( APISubDomain.API, publicRequest.SubDomain );
            Assert.AreEqual(
                "/range-reporter/league/1.1.2025071411032772.3/recap",
                publicRequest.RelativePath );
            Assert.AreEqual( "2025-11-03", publicRequest.QueryParameters["start-date"].Single() );
            Assert.AreEqual( "2025-11-09", publicRequest.QueryParameters["end-date"].Single() );

            Assert.AreEqual( HttpMethod.Get, authenticatedRequest.HttpMethod );
            Assert.AreEqual( APISubDomain.AUTHAPI, authenticatedRequest.SubDomain );
            Assert.AreEqual( publicRequest.RelativePath, authenticatedRequest.RelativePath );
            Assert.IsInstanceOfType( publicFactoryRequest, typeof( GetLeagueRecapRangeReportPublicRequest ) );
            Assert.IsInstanceOfType(
                authenticatedFactoryRequest,
                typeof( GetLeagueRecapRangeReportAuthenticatedRequest ) );
        }

        [TestMethod]
        public void PatchLeagueRecapRangeReportRequestBuildsExpectedApiContract() {
            var request = new PatchLeagueRecapRangeReportAuthenticatedRequest(
                LeagueId,
                StartDate,
                EndDate,
                CreateAuthentication() ) {
                Published = true,
                FormattedHtml = "<div><h1>Weekly league recap</h1></div>"
            };

            var queryParameters = request.QueryParameters;

            Assert.AreEqual( new HttpMethod( "PATCH" ), request.HttpMethod );
            Assert.AreEqual(
                "/range-reporter/league/1.1.2025071411032772.3/recap",
                request.RelativePath );
            Assert.AreEqual( APISubDomain.AUTHAPI, request.SubDomain );
            Assert.AreEqual( "2025-11-03", queryParameters["start-date"].Single() );
            Assert.AreEqual( "2025-11-09", queryParameters["end-date"].Single() );
            Assert.AreEqual( "True", queryParameters["Published"].Single() );
            Assert.AreEqual(
                "<div><h1>Weekly league recap</h1></div>",
                queryParameters["FormattedHtml"].Single() );
        }

        [TestMethod]
        public void LeagueRecapRangeReportRequestsValidateDatesAndPatchFields() {
            var reversedDates = new GetLeagueRecapRangeReportPublicRequest(
                LeagueId,
                EndDate,
                StartDate );
            var missingDates = new GetLeagueRecapRangeReportAuthenticatedRequest(
                CreateAuthentication() ) {
                LeagueId = LeagueId
            };
            var emptyPatch = new PatchLeagueRecapRangeReportAuthenticatedRequest(
                LeagueId,
                StartDate,
                EndDate,
                CreateAuthentication() );

            Assert.Throws<ArgumentException>( () => {
                _ = reversedDates.QueryParameters;
            } );
            Assert.Throws<ArgumentException>( () => {
                _ = missingDates.QueryParameters;
            } );
            Assert.Throws<ArgumentException>( () => {
                _ = emptyPatch.QueryParameters;
            } );
        }

        [TestMethod]
        public void OrionMatchClientExposesLeagueRecapRangeReporterCalls() {
            Assert.IsNotNull( typeof( OrionMatchAPIClient ).GetMethod(
                nameof( OrionMatchAPIClient.GenerateLeagueRecapRangeReportAuthenticatedAsync ),
                new Type[] { typeof( GenerateLeagueRecapRangeReportAuthenticatedRequest ) } ) );
            Assert.IsNotNull( typeof( OrionMatchAPIClient ).GetMethod(
                nameof( OrionMatchAPIClient.GetLeagueRecapRangeReportPublicAsync ),
                new Type[] { typeof( GetLeagueRecapRangeReportPublicRequest ) } ) );
            Assert.IsNotNull( typeof( OrionMatchAPIClient ).GetMethod(
                nameof( OrionMatchAPIClient.GetLeagueRecapRangeReportAuthenticatedAsync ),
                new Type[] { typeof( GetLeagueRecapRangeReportAuthenticatedRequest ) } ) );
            Assert.IsNotNull( typeof( OrionMatchAPIClient ).GetMethod(
                nameof( OrionMatchAPIClient.GetLeagueRecapRangeReportAsync ),
                new Type[] { typeof( GetLeagueRecapRangeReportAbstractRequest ) } ) );
            Assert.IsNotNull( typeof( OrionMatchAPIClient ).GetMethod(
                nameof( OrionMatchAPIClient.PatchLeagueRecapRangeReportAuthenticatedAsync ),
                new Type[] { typeof( PatchLeagueRecapRangeReportAuthenticatedRequest ) } ) );
        }

        [TestMethod]
        public void LeagueRecapRangeReportResponseDeserializesCurrentApiContract() {
            var json = """
            {
                "RangeReport": {
                    "ReportKind": "LEAGUE_RECAP",
                    "LeagueId": "1.1.2025071411032772.3",
                    "Week": 7,
                    "StartDate": "2025-11-03",
                    "EndDate": "2025-11-09",
                    "Headline": "A dramatic week in the league",
                    "Paragraphs": ["Opening paragraph.", "Closing paragraph."],
                    "Published": true,
                    "LicenseNumber": 1,
                    "UserContext": ["Championship week"],
                    "DryRun": true,
                    "GenerationStatus": "COMPLETED",
                    "AiGenerated": false,
                    "FormattedHtml": "<article>Patched dry-run weekly recap</article>"
                }
            }
            """;

            var wrapper = G_STJ.JsonSerializer.Deserialize<RangeReportWrapper>(
                json,
                SerializerOptions.SystemTextJsonDeserializer );

            Assert.IsNotNull( wrapper );
            Assert.AreEqual( RangeReportKind.LEAGUE_RECAP, wrapper.RangeReport.ReportKind );
            Assert.AreEqual( LeagueId, wrapper.RangeReport.LeagueId );
            Assert.AreEqual( 7, wrapper.RangeReport.Week.GetValueOrDefault() );
            Assert.AreEqual( StartDate, wrapper.RangeReport.StartDate.GetValueOrDefault() );
            Assert.AreEqual( EndDate, wrapper.RangeReport.EndDate.GetValueOrDefault() );
            Assert.AreEqual( RangeReportStatus.COMPLETED, wrapper.RangeReport.GenerationStatus );
            Assert.IsTrue( wrapper.RangeReport.Published );
            Assert.IsFalse( wrapper.RangeReport.AiGenerated.GetValueOrDefault() );
            Assert.IsTrue( wrapper.RangeReport.DryRun.GetValueOrDefault() );
            Assert.AreEqual(
                "<article>Patched dry-run weekly recap</article>",
                wrapper.RangeReport.FormattedHtml );
        }

    }
}
