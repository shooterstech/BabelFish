using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetLeagueRecapRangeReportAuthenticatedRequest : GetLeagueRecapRangeReportAbstractRequest {

        public GetLeagueRecapRangeReportAuthenticatedRequest(
            UserAuthentication credentials ) : base( "GetLeagueRecapRangeReport", credentials ) {
        }

        public GetLeagueRecapRangeReportAuthenticatedRequest(
            MatchID leagueId,
            DateTime startDate,
            DateTime endDate,
            UserAuthentication credentials ) : this( credentials ) {
            LeagueId = leagueId;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
