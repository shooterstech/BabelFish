using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// A SquaddingAssignmentFiringPoint represents the complete squadding of one participant (athlete or team) in a squadding Event, where the participant is firing on a single target.
    /// </summary>
    [Serializable]
    public class SquaddingAssignmentFiringPoint : SquaddingAssignment, IComparable<SquaddingAssignmentFiringPoint> {

        public SquaddingAssignmentFiringPoint() : base() {
			SquaddingType = SquaddingAssignmentType.FIRING_POINT;

			ReentryTag = "";
        }

        /// <summary>
        /// The name of the firing point the Individual is squadded on. Usually represented by an integer or a single character.
        /// </summary>
        public string FiringPoint { get; set; }

        /// <summary>
        /// The name of the relay the Individual is squadded on. Usually represented by an integer value.
        /// </summary>
        public string Relay { get; set; }

        public int CompareTo(SquaddingAssignmentFiringPoint other) {
            int compare = this.Range.CompareTo(other.Range);
            if (compare != 0)
                return compare;

            compare = this.Relay.CompareTo(other.Relay);
            if (compare != 0)
                return compare;

            compare = this.FiringPoint.CompareTo(other.FiringPoint);
            if (compare != 0)
                return compare;

            compare = this.ReentryTag.CompareTo(other.ReentryTag);
            if (compare != 0)
                return compare;

            return this.FiringOrder.CompareTo(other.FiringOrder);
        }

        public override string ToString( bool useAbbreviation ) {
            if (useAbbreviation) {
                return $"R{this.Relay} FP{this.FiringPoint}";
            } else {
				return $"Relay {this.Relay} FP {this.FiringPoint}";
			}
        }
    }
}
