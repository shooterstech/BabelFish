using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    /// <summary>
    /// Starts RangeReporter generation for a completed league game.
    /// </summary>
    public class GenerateLeagueRangeReportAuthenticatedRequest : RangeReportGenerationAuthenticatedRequest {

        public GenerateLeagueRangeReportAuthenticatedRequest( UserAuthentication credentials ) : base( "GenerateLeagueRangeReport", credentials ) {
        }

        public GenerateLeagueRangeReportAuthenticatedRequest(
            MatchID leagueId,
            MatchID matchId,
            UserAuthentication credentials ) : this( credentials ) {
            LeagueId = leagueId;
            MatchId = matchId;
        }

        public MatchID? LeagueId { get; set; }

        public MatchID? MatchId { get; set; }

        /// <summary>
        /// Optional validation value. When omitted, the service resolves the result list
        /// configured by the league.
        /// </summary>
        public string? ResultListName { get; set; }

        public override string RelativePath {
            get {
                if (LeagueId == null) {
                    throw new ArgumentNullException( nameof( LeagueId ), "The league id must be set to generate a league range report." );
                }

                if (MatchId == null) {
                    throw new ArgumentNullException( nameof( MatchId ), "The match id must be set to generate a league range report." );
                }

                return $"/range-reporter/league/{LeagueId}/{MatchId}";
            }
        }

        public override Dictionary<string, List<string>> QueryParameters {
            get {
                Dictionary<string, List<string>> parameterList = BuildGenerationQueryParameters();
                if (!string.IsNullOrWhiteSpace( ResultListName )) {
                    parameterList.Add( "result-name", new List<string> { ResultListName } );
                }

                return parameterList;
            }
        }
    }
}
