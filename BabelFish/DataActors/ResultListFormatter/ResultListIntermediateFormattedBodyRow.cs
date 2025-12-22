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

            int columnSubRowCount = 1;
            foreach (var column in _resultListFormatted.ResultListFormat.Format.Columns) {
                columnSubRowCount = column.BodyValues.Count;

                if ( column.Spanning is not null 
                    && ! column.Spanning.IsEmpty) {
                    _hasSpanningRow = true;
                }

                if (columnSubRowCount > this.SubRowCount) {
                    SubRowCount = columnSubRowCount;
                }
            }
        }

        public override List<string> GetClassList() {
            List<string> classSetList = new List<string>();
            if (this.IsSpanningRow) {
                foreach (var setObj in _resultListFormatted.DisplayPartitions.Body.ClassSet) {
                    if (_resultListFormatted.ShowWhenCalculator.Show( setObj.ShowWhen, _item )) {
                        classSetList.Add( setObj.Name );
                    }
                }
            }
            else {
                foreach (var setObj in _resultListFormatted.DisplayPartitions.Spanning.ClassSet) {
                    if (_resultListFormatted.ShowWhenCalculator.Show( setObj.ShowWhen, _item )) {
                        classSetList.Add( setObj.Name );
                    }
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

		/// <inheritdoc/>
		public override bool ShowRowBasedOnShowNumberOfChildren() {
            //As this is a Body / Parent row, always show it based on this criteria
            return true;
		}

        private int _numberOfChildRowsLeftToShow = 0;
        internal bool GetTokenToShowChildRow( ResultListIntermediateFormattedChildRow childRow ) {
            _numberOfChildRowsLeftToShow--;
            return _numberOfChildRowsLeftToShow > -1;
		}

        internal override void ResetNumberOfChildRowsLeftToShow() {
            this._numberOfChildRowsLeftToShow = this._resultListFormatted.ShowNumberOfChildRows;
        }

        public override bool ShowRowBasedOnShowNumberOfBodies() {
            return this._resultListFormatted.GetTokenToShowBodyRow( this );
        }

        public override ResultListCellValue GetResultListCellValue( ResultListDisplayColumn column ) {
            
            if ( IsSpanningRow ) {
                if ( column.Spanning is null || column.Spanning.IsEmpty ) {
                    return ResultListCellValue.EMPTY;
                } else {
                    return column.Spanning;
                }
            }

            if ( column.BodyValues.Count > this._SubRowIndex ) {
                return column.BodyValues[this._SubRowIndex];
            }

            return ResultListCellValue.EMPTY;                
        }
    }
}
