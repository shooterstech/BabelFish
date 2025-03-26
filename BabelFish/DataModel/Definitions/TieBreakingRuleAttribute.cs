using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Defines how to compare tow IEventScore based on an Attribute Value of the Participat object.
    /// </summary>
    public class TieBreakingRuleAttribute : TieBreakingRuleBase, IEqualityComparer<TieBreakingRuleAttribute>, IEquatable<TieBreakingRuleAttribute> {

        /// <summary>
        /// Public constructor.
        /// </summary>
        public TieBreakingRuleAttribute() {
            this.Method = TieBreakingRuleMethod.ATTRIBUTE;
        }

        /// <summary>
        /// The SetName of the Attribute to use to compare. Must be a "Simple Attribute"
        /// </summary>
        /// <remarks>
        [G_NS.JsonProperty( Order = 2 )]
        public string Source { get; set; }

        /// <inheritdoc/>
        public override string ToString() {
            return $"{Method} {SortOrder} {Source}";
        }

        #region Equal Operators

        /// <inheritdoc/>
        public bool Equals( TieBreakingRuleAttribute x, TieBreakingRuleAttribute y ) {
            return x.Source == y.Source
                && x.SortOrder == y.SortOrder;
        }

        /// <inheritdoc/>
        public int GetHashCode( TieBreakingRuleAttribute obj ) {
            return obj.ToString().GetHashCode();
        }

        /// <inheritdoc/>
        public bool Equals( TieBreakingRuleAttribute other ) {
            return Equals( this, other );
        }

        /// <inheritdoc/>
        public override bool Equals( object obj ) {
            if (obj == null || obj is not TieBreakingRuleAttribute)
                return false;

            return Equals( (TieBreakingRuleAttribute)obj );
        }

        /// <inheritdoc/>
        public override int GetHashCode() {
            return GetHashCode( this );
        }

        #endregion
    }
}
