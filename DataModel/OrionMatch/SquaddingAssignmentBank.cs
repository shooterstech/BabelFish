using System;
using System.Collections.Generic;
using System.Text;

namespace BabelFish.DataModel.Match {

    /// <summary>
    /// A SquaddingAssignmentBank represents the complete squadding of one participant (athlete or team) in a squadding Event, where the participant is firing on a multiple targets.
    /// </summary>
    [Serializable]
    public class SquaddingAssignmentBank : SquaddingAssignment, IComparable<SquaddingAssignmentBank> {

        public string Relay { get; set; }

        public string Bank { get; set; }

        public int CompareTo(SquaddingAssignmentBank other) {
            int compare = this.Range.CompareTo(other.Range);
            if (compare != 0)
                return compare;

            compare = this.Relay.CompareTo(other.Relay);
            if (compare != 0)
                return compare;

            compare = this.Bank.CompareTo(other.Bank);
            if (compare != 0)
                return compare;

            return this.FiringOrder.CompareTo(other.FiringOrder);

        }

        public override string ToString() {
            StringBuilder str = new StringBuilder();
            str.Append("Range: ");
            str.Append(this.Range);
            str.Append(" Relay: ");
            str.Append(this.Relay);
            str.Append(" Bank: ");
            str.Append(this.Bank);
            str.Append(" Firing Order: ");
            str.Append(this.FiringOrder);
            return str.ToString();
        }
    }
}
