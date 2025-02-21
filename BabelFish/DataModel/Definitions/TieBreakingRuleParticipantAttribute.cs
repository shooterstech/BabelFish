using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Defines how to compare two IEventScores using a property from their Participant object.
    /// </summary>
    public class TieBreakingRuleParticipantAttribute : TieBreakingRuleBase, IEqualityComparer<TieBreakingRuleParticipantAttribute>, IEquatable<TieBreakingRuleParticipantAttribute> {

        /// <summary>
        /// Public constructor.
        /// </summary>
        public TieBreakingRuleParticipantAttribute() {
            this.Method = TieBreakingRuleMethod.PARTICIPANT_ATTRIBUTE;
        }

        /// <summary>
        /// The property name from the Participant object use to compare.
        /// </summary>
        /// <remarks>
        /// Optional values are:
        /// <list type="bullet">
        /// <item>FamilyName</item>
        /// <item>GivenName</item>
        /// <item>MiddleName</item>
        /// <item>CompetitorNumber</item>
        /// <item>DisplayName</item>
        /// <item>DisplayNameShort</item>
        /// <item>HomeTown</item>
        /// <item>Country</item>
        /// <item>Club</item>
        /// </list>
        /// </remarks>
        public string Source { get; set; }

        /// <inheritdoc/>
        public override string ToString() {
            return $"{Method} {SortOrder} {Source}";
        }

        #region Equal Operators

        /// <inheritdoc/>
        public bool Equals( TieBreakingRuleParticipantAttribute x, TieBreakingRuleParticipantAttribute y ) {
            return x.Source == y.Source
                && x.SortOrder == y.SortOrder;
        }

        /// <inheritdoc/>
        public int GetHashCode( TieBreakingRuleParticipantAttribute obj ) {
            return ToString().GetHashCode();
        }

        /// <inheritdoc/>
        public bool Equals( TieBreakingRuleParticipantAttribute other ) {
            return Equals( this, other );
        }

        /// <inheritdoc/>
        public override bool Equals( object obj ) {
            if (obj == null || obj is not TieBreakingRuleParticipantAttribute)
                return false;

            return Equals( (TieBreakingRuleParticipantAttribute)obj );
        }

        /// <inheritdoc/>
        public override int GetHashCode() {
            return GetHashCode( this );
        }

        #endregion
    }
}
