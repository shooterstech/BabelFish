using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    [Serializable]
    /// <summary>
    /// A SquaddingAssignmentSquad represents the complete squadding of one participant (athlete or team) in a squadding Event, where the participant is firing within a squad of participants. 
    /// Squads are almost exclusively used in shotgun events.
    /// </summary>
    public class SquaddingAssignmentSquad : SquaddingAssignment, IComparable<SquaddingAssignmentSquad> {
        
        /*
         * NOTE: In shotgun, where they use squads, there is no concept of a relay. There is such a concept of order of squads, but the term 'relay' is not used.
         */

        public SquaddingAssignmentSquad() : base() {
            SquaddingType = SquaddingAssignmentType.SQUAD;
        }

        /// <summary>
        /// The name of the squad the Individual is squadded on.
        /// </summary>
        public string Squad {get; set; }

        public int CompareTo(SquaddingAssignmentSquad other) {
            int compare = this.Range.CompareTo(other.Range);
            if (compare != 0)
                return compare;

            compare = this.Squad.CompareTo(other.Squad);
            if (compare != 0)
                return compare;

            return this.FiringOrder.CompareTo(other.FiringOrder);

        }

        public override string ToString() {
            StringBuilder str = new StringBuilder();
            str.Append("Range: ");
            str.Append(this.Range);
            str.Append(" Squad: ");
            str.Append(this.Squad);
            str.Append(" Firing Order: ");
            str.Append(this.FiringOrder);
            return str.ToString();
        }
    }
}
