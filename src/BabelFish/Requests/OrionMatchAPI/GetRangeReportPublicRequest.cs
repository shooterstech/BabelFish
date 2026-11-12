using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetRangeReportPublicRequest : GetRangeReportAbstractRequest {

        public GetRangeReportPublicRequest( MatchID matchId, string resultName ) : base( "GetRangeReport", matchId, resultName ) {
        }
    }
}
