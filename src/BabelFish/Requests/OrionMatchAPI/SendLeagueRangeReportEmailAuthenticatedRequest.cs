using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class SendLeagueRangeReportEmailAuthenticatedRequest : Request {

        public SendLeagueRangeReportEmailAuthenticatedRequest( UserAuthentication credentials ) : base( "SendLeagueRangeReportEmail", credentials ) {
            HttpMethod = HttpMethod.Post;
            SubDomain = APIClients.APISubDomain.AUTHAPI;
        }

        public SendLeagueRangeReportEmailAuthenticatedRequest(
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
        public string? ResultName { get; set; }

        /// <summary>
        /// If true, the API builds the email and reports its recipients without sending it.
        /// </summary>
        public bool DryRun { get; set; }

        public override string RelativePath {
            get {
                if (LeagueId == null) {
                    throw new ArgumentNullException( nameof( LeagueId ), "The league id must be set to send a league range report email." );
                }

                if (MatchId == null) {
                    throw new ArgumentNullException( nameof( MatchId ), "The match id must be set to send a league range report email." );
                }

                return $"/range-reporter/league/{LeagueId}/{MatchId}/email";
            }
        }

        public override Dictionary<string, List<string>> QueryParameters {
            get {
                var parameterList = new Dictionary<string, List<string>>();

                if (!string.IsNullOrWhiteSpace( ResultName )) {
                    parameterList.Add( "result-name", new List<string> { ResultName } );
                }

                if (DryRun) {
                    parameterList.Add( "dry-run", new List<string> { DryRun.ToString() } );
                }

                return parameterList;
            }
        }
    }
}
