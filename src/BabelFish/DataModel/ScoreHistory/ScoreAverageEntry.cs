using Scopos.BabelFish.DataModel.Athena;

namespace Scopos.BabelFish.DataModel.ScoreHistory
{
    public class ScoreAverageEntry : ScoreAverageBase {

        public ScoreAverageEntry() : base() {
        }

        public AveragedScore ScoreAverage { get; set; }
    }
}
