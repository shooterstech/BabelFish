using System;
using System.Collections.Generic;
using System.Text;

namespace BabelFish.DataModel.Match {
    /// <summary>
    /// Abstract class representing the complete squadding assignment for one participant (athlete or team).
    /// </summary>
    [Serializable]
    public abstract class SquaddingAssignment  {

        public SquaddingAssignment() {
            RangeSortOrder = 1;
        }

        public string Range { get; set; }

        public int RangeSortOrder { get; set; }

        public int FiringOrder { get; set; }

        public Participant Participant { get; set; }
        
    }
}
