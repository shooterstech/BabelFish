using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.ResultListFormatter {

    public class ResultListIntermediateFormattedChildRow : ResultListIntermediateFormattedRow {

        public ResultListIntermediateFormattedChildRow( ResultListIntermediateFormatted rlf, ResultEvent re ) : base( rlf, re ) {

            logger = LogManager.GetCurrentClassLogger();
            IsChildRow = true;
        }

        public override List<string> GetClassList()
        {
            List<string> classSetList = new List<string>();
            foreach (var setObj in resultListFormatted.DisplayPartitions.Children.ClassSet)
            {
                if (resultListFormatted.ShowWhenCalculator.Show(setObj.ShowWhen, resultEvent))
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
