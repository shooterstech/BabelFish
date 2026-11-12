using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Summary of a RangeReporter email operation.
    /// </summary>
    public class RangeReportEmail : BaseClass {

        public int EmailsSent { get; set; }

        public int EmailsSkipped { get; set; }

        public string EmailHtml { get; set; } = string.Empty;
    }
}
