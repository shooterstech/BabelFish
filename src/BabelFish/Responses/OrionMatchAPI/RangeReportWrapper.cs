using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {

    /// <summary>
    /// Helper class that creates the added structure needed for deserializing a RangeReport API response.
    /// </summary>
    public class RangeReportWrapper : BaseClass {

        public RangeReport RangeReport { get; set; } = new RangeReport();

        public override string ToString() {
            return $"RangeReport {RangeReport.MatchId} {RangeReport.ResultListName}";
        }
    }
}
