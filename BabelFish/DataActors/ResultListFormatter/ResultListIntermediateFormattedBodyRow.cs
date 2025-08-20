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

        /// <summary>
        /// A list if child rows associated with this body row. 
        /// </summary>
        public List<ResultListIntermediateFormattedChildRow> ChildRows { get; internal set; } = new List<ResultListIntermediateFormattedChildRow>();

        /// <inheritdoc />
        public override bool ShowRowBasedOnShowRelay() {

            //Always show, if .ShowRelay does not have a filter on it (aka empty string)
            if (this._resultListFormatted.ShowRelay == string.Empty)
                return true;

            //Show the parent row, if there are not any child rows, and its on the same relay.
            if (this.ChildRows.Count == 0)
                return base.ShowRowBasedOnShowRelay();

            //Show the parent rwo, if one or more of the children are on the relay.
            foreach( var childRow in this.ChildRows ) {
                if ( childRow.ShowRowBasedOnShowRelay()) {
                    return true;
                }
            }

            //Else, donw't show
            return false;
        }
	}
}
