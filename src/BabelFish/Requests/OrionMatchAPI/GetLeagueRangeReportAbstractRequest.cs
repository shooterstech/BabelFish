using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public abstract class GetLeagueRangeReportAbstractRequest : Request {

        protected GetLeagueRangeReportAbstractRequest( string operationId ) : base( operationId ) {
        }

        protected GetLeagueRangeReportAbstractRequest( string operationId, UserAuthentication credentials ) : base( operationId, credentials ) {
        }

        public static GetLeagueRangeReportAbstractRequest Factory(
            MatchID leagueId,
            MatchID matchId,
            UserAuthentication? credentials = null,
            string? resultName = null ) {
            if (credentials == null) {
                return new GetLeagueRangeReportPublicRequest( leagueId, matchId, resultName );
            } else {
                return new GetLeagueRangeReportAuthenticatedRequest( leagueId, matchId, credentials, resultName );
            }
        }

        public MatchID? LeagueId { get; set; }

        public MatchID? MatchId { get; set; }

        /// <summary>
        /// Optional validation value. When omitted, the service resolves the result list
        /// configured by the league.
        /// </summary>
        public string? ResultName { get; set; }

        public override string RelativePath {
            get {
                if (LeagueId == null) {
                    throw new ArgumentNullException( nameof( LeagueId ), "The league id must be set to get a league range report." );
                }

                if (MatchId == null) {
                    throw new ArgumentNullException( nameof( MatchId ), "The match id must be set to get a league range report." );
                }

                return $"/range-reporter/league/{LeagueId}/{MatchId}";
            }
        }

        public override Dictionary<string, List<string>> QueryParameters {
            get {
                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();
                if (!string.IsNullOrWhiteSpace( ResultName )) {
                    parameterList.Add( "result-name", new List<string> { ResultName } );
                }

                return parameterList;
            }
        }
    }
}
