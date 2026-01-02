using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class ShowWhenEquation : ShowWhenBase {

        /// <summary>
        /// Public constructor.
        /// </summary>
        public ShowWhenEquation() {
            Operation = ShowWhenOperation.EQUATION;
        }

        /// <summary>
        /// The type of boolean operation that should be applied to all of the Arguments.
        /// </summary>
        [G_NS.JsonProperty( Order = 2, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        [DefaultValue( ShowWhenBoolean.AND )]        
        public ShowWhenBoolean Boolean { get; set; } = ShowWhenBoolean.AND;

        /// <summary>
        /// A list of ShowWhen arguments for this ShowWhenEquation
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        public List<ShowWhenBase> Arguments { get; set; } = new List<ShowWhenBase>();

        /// <inheritdoc/>
        public override string ToString() {
            return $"{Boolean} {Arguments.Count} arguments";
        }

    }
}
