using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class PatchLeagueRangeReportAuthenticatedRequest : Request {

        public PatchLeagueRangeReportAuthenticatedRequest( UserAuthentication credentials ) : base( "PatchLeagueRangeReport", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            SubDomain = APIClients.APISubDomain.AUTHAPI;
        }

        public PatchLeagueRangeReportAuthenticatedRequest(
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

        public bool? Published { get; set; }

        public string? FormattedHtml { get; set; }

        public override string RelativePath {
            get {
                if (LeagueId == null) {
                    throw new ArgumentNullException( nameof( LeagueId ), "The league id must be set to patch a league range report." );
                }

                if (MatchId == null) {
                    throw new ArgumentNullException( nameof( MatchId ), "The match id must be set to patch a league range report." );
                }

                return $"/range-reporter/league/{LeagueId}/{MatchId}";
            }
        }

        public override Dictionary<string, List<string>> QueryParameters {
            get {
                var parameterList = new Dictionary<string, List<string>>();

                if (!string.IsNullOrWhiteSpace( ResultName )) {
                    parameterList.Add( "result-name", new List<string> { ResultName } );
                }

                if (Published.HasValue) {
                    parameterList.Add( "Published", new List<string> { Published.Value.ToString() } );
                }

                if (FormattedHtml != null) {
                    parameterList.Add( "FormattedHtml", new List<string> { FormattedHtml } );
                }

                if (!Published.HasValue && FormattedHtml == null) {
                    throw new ArgumentException( "At least one of published or formatted HTML must be provided." );
                }

                return parameterList;
            }
        }
    }
}
