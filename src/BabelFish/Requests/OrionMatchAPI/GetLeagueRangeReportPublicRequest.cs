using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetLeagueRangeReportPublicRequest : GetLeagueRangeReportAbstractRequest {

        public GetLeagueRangeReportPublicRequest(
            MatchID leagueId,
            MatchID matchId,
            string? resultName = null ) : base( "GetLeagueRangeReport" ) {
            LeagueId = leagueId;
            MatchId = matchId;
            ResultName = resultName;
        }
    }
}
