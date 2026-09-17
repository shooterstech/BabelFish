using System.Linq;
using System.Net.Http;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Responses.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.DataModel.OrionMatchTests.RangeReporter {
    [TestClass]
    public class LeagueRangeReporterContractTests : BaseTestClass {

        private static readonly MatchID LeagueId = new MatchID( "1.1.2025071411032772.3" );
        private static readonly MatchID MatchId = new MatchID( "1.1.2025111411171861.1" );
        private static readonly MatchID PostseasonLeagueId = new MatchID( "1.1.2025111714363286.3" );
        private static readonly MatchID PostseasonMatchId = new MatchID( "1.1.2025121511184178.1" );

        private static UserAuthentication CreateAuthentication() {
            return new UserAuthentication( "range-reporter@example.com", "password" );
        }

        [TestMethod]
        public void GenerateLeagueRangeReportRequestBuildsExpectedApiContract() {
            var request = new GenerateLeagueRangeReportAuthenticatedRequest(
                LeagueId,
                MatchId,
                CreateAuthentication() ) {
                CourseOfFireId = 1,
                MilestoneCount = 3,
                UserContext = new List<string> { "Championship match" },
                DryRun = true
            };

            var queryParameters = request.QueryParameters;

            Assert.AreEqual( HttpMethod.Post, request.HttpMethod );
            Assert.AreEqual(
                "/range-reporter/league/1.1.2025071411032772.3/1.1.2025111411171861.1",
                request.RelativePath );
            Assert.AreEqual( APISubDomain.AUTHAPI, request.SubDomain );
            Assert.AreEqual( "1", queryParameters["course-of-fire-id"].Single() );
            Assert.AreEqual( "3", queryParameters["milestone-count"].Single() );
            Assert.AreEqual( "Championship match", queryParameters["user-context"].Single() );
            Assert.AreEqual( "True", queryParameters["dry-run"].Single() );
            Assert.IsFalse( queryParameters.ContainsKey( "result-name" ) );
        }

        [TestMethod]
        public void GenerateLeagueRangeReportAllowsConfiguredResultNameValidation() {
            var request = new GenerateLeagueRangeReportAuthenticatedRequest(
                LeagueId,
                MatchId,
                CreateAuthentication() ) {
                ResultListName = "Team - All",
                ShotMilestoneCounts = new List<int> { 20, 40, 60 }
            };

            var queryParameters = request.QueryParameters;

            Assert.AreEqual( "Team - All", queryParameters["result-name"].Single() );
            Assert.AreEqual( "20,40,60", queryParameters["shot-milestones"].Single() );
        }

        [TestMethod]
        public void GenerateLeagueRangeReportRejectsConflictingMilestoneOptions() {
            var request = new GenerateLeagueRangeReportAuthenticatedRequest(
                LeagueId,
                MatchId,
                CreateAuthentication() ) {
                MilestoneCount = 3,
                ShotMilestoneCounts = new List<int> { 20, 40, 60 }
            };

            Assert.Throws<ArgumentException>( () => {
                _ = request.QueryParameters;
            } );
        }

        [TestMethod]
        public void GetLeagueRangeReportRequestsBuildExpectedApiContracts() {
            var publicRequest = new GetLeagueRangeReportPublicRequest( LeagueId, MatchId );
            var authenticatedRequest = new GetLeagueRangeReportAuthenticatedRequest(
                PostseasonLeagueId,
                PostseasonMatchId,
                CreateAuthentication(),
                "Team - All" );
            var factoryRequest = GetLeagueRangeReportAbstractRequest.Factory( LeagueId, MatchId );

            Assert.AreEqual( HttpMethod.Get, publicRequest.HttpMethod );
            Assert.AreEqual( APISubDomain.API, publicRequest.SubDomain );
            Assert.AreEqual(
                "/range-reporter/league/1.1.2025071411032772.3/1.1.2025111411171861.1",
                publicRequest.RelativePath );
            Assert.AreEqual( 0, publicRequest.QueryParameters.Count );

            Assert.AreEqual( HttpMethod.Get, authenticatedRequest.HttpMethod );
            Assert.AreEqual( APISubDomain.AUTHAPI, authenticatedRequest.SubDomain );
            Assert.AreEqual(
                "/range-reporter/league/1.1.2025111714363286.3/1.1.2025121511184178.1",
                authenticatedRequest.RelativePath );
            Assert.AreEqual( "Team - All", authenticatedRequest.QueryParameters["result-name"].Single() );
            Assert.IsInstanceOfType( factoryRequest, typeof( GetLeagueRangeReportPublicRequest ) );
        }

        [TestMethod]
        public void PatchLeagueRangeReportRequestBuildsExpectedApiContract() {
            var request = new PatchLeagueRangeReportAuthenticatedRequest(
                LeagueId,
                MatchId,
                CreateAuthentication() ) {
                Published = true,
                FormattedHtml = "<div><h1>League recap</h1></div>"
            };

            var queryParameters = request.QueryParameters;

            Assert.AreEqual( new HttpMethod( "PATCH" ), request.HttpMethod );
            Assert.AreEqual(
                "/range-reporter/league/1.1.2025071411032772.3/1.1.2025111411171861.1",
                request.RelativePath );
            Assert.AreEqual( APISubDomain.AUTHAPI, request.SubDomain );
            Assert.AreEqual( "True", queryParameters["Published"].Single() );
            Assert.AreEqual(
                "<div><h1>League recap</h1></div>",
                queryParameters["FormattedHtml"].Single() );
            Assert.IsFalse( queryParameters.ContainsKey( "result-name" ) );
        }

        [TestMethod]
        public void SendLeagueRangeReportEmailRequestIsDryRunAndDoesNotCallApi() {
            // This is a pure request-contract test. It deliberately does not instantiate
            // OrionMatchAPIClient or execute CallAPIAsync, so no email can be sent.
            var request = new SendLeagueRangeReportEmailAuthenticatedRequest(
                LeagueId,
                MatchId,
                CreateAuthentication() ) {
                DryRun = true
            };

            var queryParameters = request.QueryParameters;

            Assert.AreEqual( HttpMethod.Post, request.HttpMethod );
            Assert.AreEqual(
                "/range-reporter/league/1.1.2025071411032772.3/1.1.2025111411171861.1/email",
                request.RelativePath );
            Assert.AreEqual( APISubDomain.AUTHAPI, request.SubDomain );
            Assert.AreEqual( "True", queryParameters["dry-run"].Single() );
            Assert.IsFalse( queryParameters.ContainsKey( "result-name" ) );
        }

        [TestMethod]
        public void OrionMatchClientExposesLeagueRangeReporterCalls() {
            Assert.IsNotNull( typeof( OrionMatchAPIClient ).GetMethod(
                nameof( OrionMatchAPIClient.GenerateLeagueRangeReportAuthenticatedAsync ),
                new Type[] { typeof( GenerateLeagueRangeReportAuthenticatedRequest ) } ) );
            Assert.IsNotNull( typeof( OrionMatchAPIClient ).GetMethod(
                nameof( OrionMatchAPIClient.GetLeagueRangeReportPublicAsync ),
                new Type[] { typeof( GetLeagueRangeReportPublicRequest ) } ) );
            Assert.IsNotNull( typeof( OrionMatchAPIClient ).GetMethod(
                nameof( OrionMatchAPIClient.GetLeagueRangeReportAuthenticatedAsync ),
                new Type[] { typeof( GetLeagueRangeReportAuthenticatedRequest ) } ) );
            Assert.IsNotNull( typeof( OrionMatchAPIClient ).GetMethod(
                nameof( OrionMatchAPIClient.PatchLeagueRangeReportAuthenticatedAsync ),
                new Type[] { typeof( PatchLeagueRangeReportAuthenticatedRequest ) } ) );
            Assert.IsNotNull( typeof( OrionMatchAPIClient ).GetMethod(
                nameof( OrionMatchAPIClient.SendLeagueRangeReportEmailAuthenticatedAsync ),
                new Type[] { typeof( SendLeagueRangeReportEmailAuthenticatedRequest ) } ) );
        }
        [TestMethod]
        public void LeagueRangeReportResponseDeserializesCurrentApiContract() {
            var json = """
            {
                "RangeReport": {
                    "ReportKind": "LEAGUE_GAME",
                    "LeagueId": "1.1.2025111714363286.3",
                    "CanonicalLeagueId": "1.1.2025071411032772.3",
                    "LeagueName": "2025 National Precision Air Rifle League",
                    "LeagueTeamNames": ["Everett Sportsman Junior Rifle Club", "Robinson Rifle Team"],
                    "Headline": "Everett sets league record",
                    "Paragraphs": ["Everett won the regular-season finale."],
                    "Published": false,
                    "MatchId": "1.1.2025121511184178.1",
                    "LicenseNumber": 1,
                    "ResultListName": "Team - All",
                    "CourseOfFireId": 1,
                    "ShotMilestoneCounts": [10, 20, 30, 40, 50, 60],
                    "MilestoneCount": 6,
                    "ExpectedShots": 60,
                    "ScoreComponent": "D",
                    "UserContext": [],
                    "GenerationStatus": "COMPLETED",
                    "AiGenerated": true
                }
            }
            """;

            var wrapper = G_STJ.JsonSerializer.Deserialize<RangeReportWrapper>(
                json,
                SerializerOptions.SystemTextJsonDeserializer );

            Assert.IsNotNull( wrapper );
            Assert.AreEqual( RangeReportKind.LEAGUE_GAME, wrapper.RangeReport.ReportKind );
            Assert.AreEqual( PostseasonLeagueId.ToString(), wrapper.RangeReport.LeagueId?.ToString() );
            Assert.AreEqual( LeagueId.ToString(), wrapper.RangeReport.CanonicalLeagueId?.ToString() );
            Assert.AreEqual( PostseasonMatchId.ToString(), wrapper.RangeReport.MatchId );
            Assert.AreEqual( "2025 National Precision Air Rifle League", wrapper.RangeReport.LeagueName );
            Assert.AreEqual( 2, wrapper.RangeReport.LeagueTeamNames.Count );
            Assert.AreEqual( 6, wrapper.RangeReport.MilestoneCount );
            Assert.AreEqual( ScoreComponent.D, wrapper.RangeReport.ScoreComponent );
            Assert.AreEqual( RangeReportStatus.COMPLETED, wrapper.RangeReport.GenerationStatus );
        }
    }
}
