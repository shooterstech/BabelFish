using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class GetRangeReportPublicResponse : GetRangeReportAbstractResponse {

        public GetRangeReportPublicResponse( GetRangeReportPublicRequest request ) : base() {
            this.Request = request;
        }
    }
}
