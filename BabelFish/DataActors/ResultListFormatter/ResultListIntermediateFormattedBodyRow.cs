using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using NLog;
using System.Diagnostics;

namespace Scopos.BabelFish.DataActors.ResultListFormatter {
    public class ResultListIntermediateFormattedBodyRow : ResultListIntermediateFormattedRow {

        public ResultListIntermediateFormattedBodyRow( ResultListIntermediateFormatted rlf, IRLIFItem re ) : base (rlf, re) {

            _logger = LogManager.GetCurrentClassLogger();
            IsChildRow = false;
        }

        public override List<string> GetClassList() {
            List<string> classSetList = new List<string>();
            foreach ( var setObj in _resultListFormatted.DisplayPartitions.Body.ClassSet ){
                if (_resultListFormatted.ShowWhenCalculator.Show(setObj.ShowWhen, _item))
                {
                    classSetList.Add(setObj.Name);
                }
            }
            return classSetList;

            //NOTE .RowClass is deprecated
            //return resultListFormatted.DisplayPartitions.Body.ClassList.Concat( resultListFormatted.DisplayPartitions.Body.RowClass ).ToList<string>();
        }
    }
}
