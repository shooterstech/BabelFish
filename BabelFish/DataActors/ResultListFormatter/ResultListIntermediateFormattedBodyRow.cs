using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using NLog;

namespace Scopos.BabelFish.DataActors.ResultListFormatter {
    public class ResultListIntermediateFormattedBodyRow : ResultListIntermediateFormattedRow {

        public ResultListIntermediateFormattedBodyRow( ResultListIntermediateFormatted rlf, ResultEvent re ) : base (rlf, re) {

            logger = LogManager.GetCurrentClassLogger();
            IsChildRow = false;
        }

        public override List<string> GetClassList() {
            return resultListFormatted.DisplayPartitions.Body.RowClass;
        }

        public override List<LinkToOption> GetLinkToList() {
            return resultListFormatted.DisplayPartitions.Body.RowLinkTo;
        }
    }
}
