using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetLeagueRecapRangeReportPublicRequest : GetLeagueRecapRangeReportAbstractRequest {

        public GetLeagueRecapRangeReportPublicRequest(
            MatchID leagueId,
            DateTime startDate,
            DateTime endDate ) : base( "GetLeagueRecapRangeReport" ) {
            LeagueId = leagueId;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
