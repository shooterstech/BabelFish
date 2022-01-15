using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class ScoreAverage {

        public ScoreAverage() {
            Average = 0;
            TieBreaker1 = 0;
            TieBreaker2 = 0;
        }

        public float Average { get; set; }

        public float TieBreaker1 { get; set; }

        public float TieBreaker2 { get; set; }
    }
}
