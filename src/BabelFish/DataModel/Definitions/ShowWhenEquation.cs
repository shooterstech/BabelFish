using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// ShowWhen operations describe logic for when a <seealso cref="ResultListFormat">RESULT LIST FORMAT</seealso>
    /// <seealso cref="ResultListDisplayColumn"/>, <seealso cref="ClassSet"/>, or SpanningText is included and displayed.
    /// <para>A ShowWhenEquation is a concrete class that combines multiple ShowWhenBase evaluations in the form
    /// of <seealso cref="Arguments"/></para>
    /// </summary>
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
