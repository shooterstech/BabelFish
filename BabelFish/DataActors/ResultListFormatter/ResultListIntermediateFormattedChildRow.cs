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
            List<string> classSetList = new List<string>();
            foreach (var setObj in resultListFormatted.DisplayPartitions.Body.ClassSet)
            {
                if (resultListFormatted.ShowWhenCalculator.Show(setObj.ShowWhen))
                {
                    classSetList.Add(setObj.Name);
                }
            }
            return classSetList;

            //NOTE .RowClass is deprecated
            //return resultListFormatted.DisplayPartitions.Children.ClassList.Concat( resultListFormatted.DisplayPartitions.Children.RowClass ).ToList<string>();
        }
    }
}
