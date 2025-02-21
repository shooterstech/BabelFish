using System.Text.Json;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {


    /// <summary>
    /// An abstract class that helps define both an aiming mark (what an athlete aims at) or a scoring ring
    /// </summary>
    [Serializable]
    public abstract class ScoringShapeDimension: IReconfigurableRulebookObject {

        /// <summary>
        /// Public constructor
        /// </summary>
        public ScoringShapeDimension() { }

        /// <summary>
        /// THe shape of this ScoringShape.
        /// </summary>
        
        [DefaultValue( ScoringShape.CIRCLE )]
        public ScoringShape Shape { get; set; } = ScoringShape.CIRCLE;

        /// <summary>
        /// The size of this scoring shape.
        /// If the Shape is a Circle -> Dimension is diameter
        /// If the Shape is a Square -> Dimension is length of a side
        /// </summary>
        public float Dimension { get; set; } = 0;


        /// <inheritdoc/>
        [JsonPropertyOrder ( 99 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;
    }
}
