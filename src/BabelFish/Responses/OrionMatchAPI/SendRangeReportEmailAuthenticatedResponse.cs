using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class SendRangeReportEmailAuthenticatedResponse : Response<RangeReportEmailWrapper> {

        public SendRangeReportEmailAuthenticatedResponse( SendRangeReportEmailAuthenticatedRequest request ) : base() {
            Request = request;
        }

        /// <summary>
        /// Facade returning the RangeReporter email result.
        /// </summary>
        public RangeReportEmail RangeReportEmail {
            get { return Value; }
        }
    }
}
