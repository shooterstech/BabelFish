using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.ResultListFormatter {

    public class ResultListIntermediateFormattedChildRow : ResultListIntermediateFormattedRow {

        public ResultListIntermediateFormattedChildRow( ResultListIntermediateFormatted rlf, IRLIFItem re ) : base( rlf, re ) {

            _logger = LogManager.GetCurrentClassLogger();
            IsChildRow = true;
        }

        public override List<string> GetClassList()
        {
            List<string> classSetList = new List<string>();
            foreach (var setObj in _resultListFormatted.DisplayPartitions.Children.ClassSet)
            {
                if (_resultListFormatted.ShowWhenCalculator.Show(setObj.ShowWhen, _item))
                {
                    classSetList.Add(setObj.Name);
                }
            }
            return classSetList;

            //NOTE .RowClass is deprecated
            //return resultListFormatted.DisplayPartitions.Children.ClassList.Concat( resultListFormatted.DisplayPartitions.Children.RowClass ).ToList<string>();
        }

        public ResultListIntermediateFormattedBodyRow ParentRow { get; internal set; }

        /// <inheritdoc/>
		public override bool ShowRowBasedOnShowRanks() {
			//Show this row, because it is a child row, if the parent row is shown.
            return  this.ParentRow.ShowRowBasedOnShowRanks();
		}

		/// <inheritdoc/>
		public override bool ShowRowBasedOnShowNumberOfChildren() {
            //Ask the parent row, if this child row is allowed to be shown
            return this.ParentRow.GetTokenToShowChildRow( this );
		}

		/// <inheritdoc/>
		public override bool ShowRowBasedOnShowZeroScoresWithOFFICIAL() {
			return this.ParentRow.ShowRowBasedOnShowZeroScoresWithOFFICIAL() 
                && base.ShowRowBasedOnShowZeroScoresWithOFFICIAL();
		}

		/// <inheritdoc/>
		public override bool ShowRowBasedOnShownStatus() {
			return this.ParentRow.ShowRowBasedOnShownStatus()
                && base.ShowRowBasedOnShownStatus();
		}

		/// <inheritdoc/>
		public override bool ShowRowBasedOnShowRelay() {
			return this.ParentRow.ShowRowBasedOnShowRelay()
                && base.ShowRowBasedOnShowRelay();
		}
	}
}
