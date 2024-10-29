using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// 
    /// </summary>

    [Serializable]
    public class AimingMark : ScoringShapeDimension, ICopy<AimingMark> {

        /// <summary>
        /// Public constructor
        /// </summary>
        public AimingMark() { }

        /// <inheritdoc/>
        public AimingMark Copy() {
            AimingMark copy = new AimingMark();
            copy.Color = this.Color;
            copy.Comment = this.Comment;
            copy.Shape = this.Shape;
            copy.Dimension = this.Dimension;

            return copy;
        }

        /// <summary>
        /// the color of the aiming mark
        /// </summary>
        [JsonConverter( typeof( StringEnumConverter ) )]
        public AimingMarkColor Color { get; set; } = AimingMarkColor.BLACK;
    }
}
