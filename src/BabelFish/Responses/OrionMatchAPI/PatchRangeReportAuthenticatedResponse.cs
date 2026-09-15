using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class PatchRangeReportAuthenticatedResponse : Response<RangeReportWrapper> {

        public PatchRangeReportAuthenticatedResponse( PatchRangeReportAuthenticatedRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value.RangeReport.
        /// </summary>
        public RangeReport RangeReport {
            get { return Value.RangeReport; }
        }
    }
}
