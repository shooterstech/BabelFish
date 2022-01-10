using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.Match {
    [Serializable]
    public class OrionTargetStage : IComparable<OrionTargetStage> {

        public OrionTargetStage() {
            SortOrder = 1;
            this.Target = new OrionTarget();
        }

        public OrionTargetStage(string name, string stage, int series, OrionTarget target) {
            this.Name = name;
            this.Series = series;
            this.Stage = stage;
            this.Target = target;
            this.Key = stage + series.ToString();
            SortOrder = 1;
        }

        public OrionTargetStage(string name, string stage, int series, OrionTarget target, int sortOrder, bool relayReset) {
            this.Name = name;
            this.Series = series;
            this.Stage = stage;
            this.Target = target;
            this.Key = stage + series.ToString();
            this.SortOrder = sortOrder;
            this.RelayReset = relayReset;
        }

        /// <summary>
        /// Human readable name of the TargetStage
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// An ID, used by Orion, to identify the target photograph and map it to a series of events.
        /// </summary>
        public string Key { get; set; }

        public string Stage { get; set; }

        public int Series { get; set; }

        /// <summary>
        /// The brief definition of the type of target that is shot during this stage.
        /// </summary>
        public OrionTarget Target { get; set; }

        /// <summary>
        /// When sorting a list of OrionTargetStages, this is the order the objects should appear in the list.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// If true, the first relay (usually relay 1) shoots first. If false, the relay that shot in the previous OrionTargetStage shoots.
        /// </summary>
        public bool RelayReset { get; set; }

        public int CompareTo(OrionTargetStage other) {
            int compare = this.SortOrder.CompareTo(other.SortOrder);
            if (compare != 0)
                return compare;

            compare = this.Stage.CompareTo(other.Stage);
            if (compare != 0)
                return compare;

            compare = this.Series.CompareTo(other.Series);
            if (compare != 0)
                return compare;

            return this.Name.CompareTo(other.Name);
        }

        public override string ToString() {
            return Name;
        }
    }
}
