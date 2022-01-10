using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.Definitions {
    [Serializable]
    public abstract class ScoringShape {

        public const string SHAPE_SQUARE = "SQUARE";
        public const string SHAPE_CIRCLE = "CIRCLE";

        public ScoringShape() {
            Shape = SHAPE_CIRCLE;
        }

        public string Shape { get; set; }

        /// <summary>
        /// CIRCLE -> Dimension is diameter
        /// SQUARE -> Dimension is length of a side
        /// </summary>
        public float Dimension { get; set; }
    }
}
