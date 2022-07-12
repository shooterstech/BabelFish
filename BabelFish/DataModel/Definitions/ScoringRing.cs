using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Definitions {
    [Serializable]
    public class ScoringRing : ScoringShape {

        public ScoringRing() { }

        public ScoringRing(int value, float diameter) {
            this.Value = value;
            this.Dimension = diameter;
            this.Shape = SHAPE_CIRCLE;
        }

        public int Value { get; set; } = 0;
    }
}
