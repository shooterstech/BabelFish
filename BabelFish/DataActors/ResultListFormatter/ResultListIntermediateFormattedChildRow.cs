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
    }
}
