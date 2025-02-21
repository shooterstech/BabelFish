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
        [DefaultValue( ShowWhenBoolean.AND )]
        [JsonInclude]
        public ShowWhenBoolean Boolean { get; set; } = ShowWhenBoolean.AND;

        public List<ShowWhenBase> Arguments { get; set; } = new List<ShowWhenBase>();

        /// <inheritdoc/>
        public override string ToString() {
            return $"{Boolean} {Arguments.Count} arguments";
        }

    }
}
