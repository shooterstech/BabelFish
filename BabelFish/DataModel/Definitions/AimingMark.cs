using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BabelFish.DataModel.Definitions {
    [Serializable]
    public class AimingMark : ScoringShape {

        //TODO turn these string consts into Enum
        public const string COLOR_BLACK = "BLACK";
        public const string COLOR_WHITE = "WHITE";

        public AimingMark() {
            Color = COLOR_BLACK;
            Shape = SHAPE_CIRCLE;
        }

        public AimingMark(float dimension, string shape, string color) {
            this.Dimension = dimension;
            this.Shape = shape;
            this.Color = color;
        }

        public string Color { get; set; }
    }
}
