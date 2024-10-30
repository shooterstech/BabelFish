using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class ShowWhenEquation : ShowWhenBase {

        /// <summary>
        /// Public constructor.
        /// </summary>
        public ShowWhenEquation() {
            Operation = ShowWhenOperation.EQUATION;
        }

        /// <inheritdoc/>
        public override ShowWhenBase Copy() {
            ShowWhenEquation copy = new ShowWhenEquation();
            copy.Comment = this.Comment;
            copy.Not = this.Not;
            copy.Boolean = this.Boolean;
            foreach( var arg in Arguments)
                copy.Arguments.Add(arg.Copy());

            return copy;
        }

        /// <summary>
        /// The type of boolean operatino that should be applied to all of the Arguments.
        /// </summary>
        [JsonConverter( typeof( StringEnumConverter ) )]
        public ShowWhenBoolean Boolean { get; set; }

        public List<ShowWhenBase> Arguments { get; set; } = new List<ShowWhenBase>();

        public override string ToString() {
            var not = Not ? "NOT " : "";
            return $"{not}{Boolean} {Arguments.Count} arguments";
        }

    }
}
