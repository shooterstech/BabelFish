using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetRangeReportAuthenticatedRequest : GetRangeReportAbstractRequest {

        public GetRangeReportAuthenticatedRequest( UserAuthentication credentials ) : base( "GetRangeReport", credentials ) {
        }

        public GetRangeReportAuthenticatedRequest(
            MatchID matchId,
            string resultName,
            UserAuthentication credentials ) : base( "GetRangeReport", matchId, resultName, credentials ) {
        }
    }
}
