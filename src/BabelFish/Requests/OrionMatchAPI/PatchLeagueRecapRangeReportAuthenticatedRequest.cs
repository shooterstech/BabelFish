using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class PatchLeagueRecapRangeReportAuthenticatedRequest : LeagueRecapRangeReportRequest {

        public PatchLeagueRecapRangeReportAuthenticatedRequest(
            UserAuthentication credentials ) : base( "PatchLeagueRecapRangeReport", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            SubDomain = APIClients.APISubDomain.AUTHAPI;
        }

        public PatchLeagueRecapRangeReportAuthenticatedRequest(
            MatchID leagueId,
            DateTime startDate,
            DateTime endDate,
            UserAuthentication credentials ) : this( credentials ) {
            LeagueId = leagueId;
            StartDate = startDate;
            EndDate = endDate;
        }

        public bool? Published { get; set; }

        public string? FormattedHtml { get; set; }

        public override Dictionary<string, List<string>> QueryParameters {
            get {
                var parameterList = BuildLeagueWeekQueryParameters();

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
