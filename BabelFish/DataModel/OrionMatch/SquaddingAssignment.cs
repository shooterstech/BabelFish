using System;
using System.Collections.Generic;
using System.Text;

namespace BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// Abstract class representing the complete squadding assignment for one participant (athlete or team).
    /// </summary>
    [Serializable]
    public abstract class SquaddingAssignment  {

        public SquaddingAssignment() { }

        public string Range { get; set; } = string.Empty;

        public int RangeSortOrder { get; set; } = 1;

        public int FiringOrder { get; set; } = 0;

        public Participant Participant { get; set; } = new Individual();

    }
}
