using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// An AttributeFilterEquation is a concrete class implementation for AttributeFilter. An Equation
    /// specifies the boolean logic of how multiple AttributeFilter instances are combined.
    ///
    /// </summary>
    public class AttributeFilterEquation : AttributeFilter, IEquatable<AttributeFilterEquation>, IEqualityComparer<AttributeFilterEquation>, IFinishInitializationAsync {

        /// <summary>
        /// Constructor
        /// </summary>
        public AttributeFilterEquation() {
            this.Operation = AttributeFilterOperation.EQUATION;
        }

        /// <summary>
        /// The boolean operation to apply.
        /// </summary>
        /// <remarks>Using the existing enum ShowWhenBoolean, which has all of the boolean operations defined.
        /// Choosing not to rename it (for now) to be more generic, as that would be a breaking change.</remarks>
        [G_NS.JsonProperty( Order = 2, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public ShowWhenBoolean Boolean { get; set; } = ShowWhenBoolean.AND;

        /// <summary>
        /// The list of AttributeFilter to apply the boolean logic too.
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        public List<AttributeFilter> Arguments { get; set; } = new List<AttributeFilter>();

        /// <inheritdoc />
        public override void UpdateCourseOfFireId( int courseOfFireId ) {
            foreach (var arg in this.Arguments) {
                arg.UpdateCourseOfFireId( courseOfFireId );
            }
        }

        /// <inheritdoc />
        public override int Count => Arguments.Sum( arg => arg.Count );

        /// <inheritdoc/>
        public override string ToString() {
            return string.Join( $" {Boolean} ", Arguments );
        }

        /// <summary>
        /// Returns a hash code unique ideifying this AttributeFiler. Incorporating the Operation, Boolean, and </summary>
        /// <inheritdoc />
        public override int GetHashCode() {
            StringBuilder sb = new StringBuilder();
            sb.Append( this.Operation.ToString() );
            sb.Append( this.Boolean.ToString() );
            foreach (var arg in this.Arguments) {
                sb.Append( arg.GetHashCode() );
            }
            return sb.ToString().GetHashCode();
        }

        /// <inheritdoc />
        public override bool Equals( object obj ) {
            if (obj is AttributeFilterEquation afe) {
                return Equals( afe );
            }
            return false;
        }

        /// <inheritdoc />
        public bool Equals( AttributeFilterEquation other ) {
            return this.GetHashCode() == other.GetHashCode();
        }

        /// <inheritdoc />
        public bool Equals( AttributeFilterEquation x, AttributeFilterEquation y ) => x.Equals( y );

        /// <inheritdoc />
        public int GetHashCode( AttributeFilterEquation obj ) => obj.GetHashCode();

        /// <inheritdoc />
        public override async Task FinishInitializationAsync() {
            foreach (var arg in Arguments) {
                await arg.FinishInitializationAsync();
            }
        }
    }
}
