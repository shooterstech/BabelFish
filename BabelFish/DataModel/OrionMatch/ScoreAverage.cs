using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class ScoreAverage {

        public ScoreAverage() { }

        public float Average { get; set; } = 0;

        public float TieBreaker1 { get; set; } = 0;

        public float TieBreaker2 { get; set; } = 0;
    }
}
