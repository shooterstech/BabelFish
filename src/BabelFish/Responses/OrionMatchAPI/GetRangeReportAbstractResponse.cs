using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public abstract class GetRangeReportAbstractResponse : Response<RangeReportWrapper> {

        public GetRangeReportAbstractResponse() : base() {
        }

        /// <summary>
        /// Facade function that returns the same as this.Value.RangeReport.
        /// </summary>
        public RangeReport RangeReport {
            get { return Value.RangeReport; }
        }
    }
}
