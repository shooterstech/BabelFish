using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    /// <summary>
    /// Starts RangeReporter generation for one week of a league.
    /// </summary>
    public class GenerateLeagueRecapRangeReportAuthenticatedRequest : LeagueRecapRangeReportRequest {

        public GenerateLeagueRecapRangeReportAuthenticatedRequest(
            UserAuthentication credentials ) : base( "GenerateLeagueRecapRangeReport", credentials ) {
            HttpMethod = HttpMethod.Post;
            SubDomain = APIClients.APISubDomain.AUTHAPI;
            Timeout = 3 * 60;
        }

        public GenerateLeagueRecapRangeReportAuthenticatedRequest(
            MatchID leagueId,
            DateTime startDate,
            DateTime endDate,
            UserAuthentication credentials ) : this( credentials ) {
            LeagueId = leagueId;
            StartDate = startDate;
            EndDate = endDate;
        }

        public List<string> UserContext { get; set; } = new List<string>();

        public bool DryRun { get; set; } = false;

        /// <summary>
        /// Development option that queues the recap without invoking the generation worker.
        /// </summary>
        public bool LocalTest { get; set; } = false;

        public override Dictionary<string, List<string>> QueryParameters {
            get {
                var parameterList = BuildLeagueWeekQueryParameters();

                if (UserContext != null && UserContext.Count > 0) {
                    if (UserContext.Any( string.IsNullOrWhiteSpace )) {
                        throw new ArgumentException( "User context entries must be non-empty strings.", nameof( UserContext ) );
                    }

                    parameterList.Add( "user-context", UserContext );
                }

                if (DryRun) {
                    parameterList.Add( "dry-run", new List<string> { DryRun.ToString() } );
                }

                if (LocalTest) {
                    parameterList.Add( "local-test", new List<string> { LocalTest.ToString() } );
                }

                return parameterList;
            }
        }
    }
}
