using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class GetRangeReportAuthenticatedResponse : GetRangeReportAbstractResponse {

        public GetRangeReportAuthenticatedResponse( GetRangeReportAuthenticatedRequest request ) : base() {
            this.Request = request;
        }

        public GetRangeReportAuthenticatedResponse( GetLeagueRangeReportAuthenticatedRequest request ) : base() {
            this.Request = request;
        }
    }
}
