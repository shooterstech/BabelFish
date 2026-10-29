using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetLeagueRangeReportAuthenticatedRequest : GetLeagueRangeReportAbstractRequest {

        public GetLeagueRangeReportAuthenticatedRequest( UserAuthentication credentials ) : base( "GetLeagueRangeReport", credentials ) {
        }

        public GetLeagueRangeReportAuthenticatedRequest(
            MatchID leagueId,
            MatchID matchId,
            UserAuthentication credentials,
            string? resultName = null ) : this( credentials ) {
            LeagueId = leagueId;
            MatchId = matchId;
            ResultName = resultName;
        }
    }
}
