using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using NLog;

namespace Scopos.BabelFish.DataActors.ResultListFormatter {

    public class ResultListIntermediateFormattedChildRow : ResultListIntermediateFormattedRow {

        public ResultListIntermediateFormattedChildRow( ResultListIntermediateFormatted rlf, ResultEvent re ) : base( rlf, re ) {

            logger = LogManager.GetCurrentClassLogger();
            IsChildRow = true;
        }

        public override List<string> GetClassList() {
            /*
             * TODO: Liam
             * Update to use the new ClassSet property
             */

            //NOTE .RowClass is deprecated
            return resultListFormatted.DisplayPartitions.Children.ClassList.Concat( resultListFormatted.DisplayPartitions.Children.RowClass ).ToList<string>();
        }
    }
}
