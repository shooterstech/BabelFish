using System;
using System.Collections.Generic;
using System.Text;

namespace BabelFish.DataModel.OrionMatch {

    /*
    //Proposed Solution #1
    public class SquaddingListFiringPointsList
    {
        public string SquaddingListID { get; set; }
        public string MatchID { get; set; }
        public string ParentID { get; set; }
        public string EventName { get; set; }
        public string AccountNumber { get; set; }
        public string JSONVersion { get; set; }

        public List<SquaddingAssignmentFiringPoint> SquaddingList = new List<SquaddingAssignmentFiringPoint>();
    }

    //Proposed Solution #2
    public class SquaddingListWrapper : Squadding {
        public List<SquaddingAssignmentFiringPoint> SquaddingList = new List<SquaddingAssignmentFiringPoint>();
    }
    */

    /// <summary>
    /// A SquaddingAssignmentFiringPoint represents the complete squadding of one participant (athlete or team) in a squadding Event, where the participant is firing on a single target.
    /// </summary>
    [Serializable]
    public class SquaddingAssignmentFiringPoint : SquaddingAssignment, IComparable<SquaddingAssignmentFiringPoint> {

        public const int CONCRETE_CLASS_ID = 1;

        public SquaddingAssignmentFiringPoint() : base() {
            ConcreteClassId = CONCRETE_CLASS_ID;

            RelaySortOrder = 1;
            FiringPointSortOrder = 1;
            ReentryTag = "";
        }

        public string Relay { get; set; }

        public int RelaySortOrder { get; set; }

        public string FiringPoint { get; set; }

        public int FiringPointSortOrder { get; set; }

        public string ReentryTag { get; set; }

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

        public override string ToString() {
            StringBuilder str = new StringBuilder();
            str.Append("Range: ");
            str.Append(this.Range);
            str.Append(" Relay: ");
            str.Append(this.Relay);
            str.Append(" FP: ");
            str.Append(this.FiringPoint);
            str.Append(" Firing Order: ");
            str.Append(this.FiringOrder);
            return str.ToString();
        }
    }
}
