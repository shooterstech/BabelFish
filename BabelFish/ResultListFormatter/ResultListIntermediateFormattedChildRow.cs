using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using NLog;

namespace Scopos.BabelFish.ResultListFormatter {

    public class ResultListIntermediateFormattedChildRow : ResultListIntermediateFormattedRow {

        public ResultListIntermediateFormattedChildRow( ResultListIntermediateFormatted rlf, ResultEvent re ) : base( rlf, re ) {

            logger = LogManager.GetCurrentClassLogger();
            IsChildRow = true;
        }

        public override List<string> GetClassList() {
            return resultListFormatted.DisplayPartitions.Children.RowClass;
        }

        public override List<LinkToOption> GetLinkToList() {
            return resultListFormatted.DisplayPartitions.Children.RowLinkTo;
        }
    }
}
