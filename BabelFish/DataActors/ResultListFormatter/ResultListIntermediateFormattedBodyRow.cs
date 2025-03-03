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
            /*
             * TODO: Liam
             * Update to use the new ClassSet property
             */

            //NOTE .RowClass is deprecated
            return resultListFormatted.DisplayPartitions.Body.ClassList.Concat( resultListFormatted.DisplayPartitions.Body.RowClass ).ToList<string>();
        }
    }
}
