using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.DataModel.OrionMatch {
    [Serializable]
    /// <summary>
    /// A SquaddingAssignmentFiringPoint represents the complete squadding of one participant (athlete or team) in a squadding Event, where the participant is firing on a single target.
    /// </summary>
    public class SquaddingAssignmentSquad : SquaddingAssignment, IComparable<SquaddingAssignmentSquad> {

        public const int CONCRETE_CLASS_ID = 3;

        public SquaddingAssignmentSquad() : base() {
            ConcreteClassId = CONCRETE_CLASS_ID;
        }

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
