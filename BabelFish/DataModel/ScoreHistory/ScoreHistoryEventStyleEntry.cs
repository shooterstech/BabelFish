using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.BabelFish.DataModel.ScoreHistory {

    /// <summary>
    /// Data model that represents one athlete's score shot for one Course of Fire. 
    /// </summary>
    public class ScoreHistoryEventStyleEntry : ScoreHistoryEntry {

        public const int CONCRETE_CLASS_ID = 1;

        /// <summary>
        /// Pulic Constructor
        /// </summary>
        public ScoreHistoryEventStyleEntry()  : base() {
            ConcreteClassId = CONCRETE_CLASS_ID;
        }

        /// <summary>
        /// String, formatted as a SetName, representing the Event Style this ScoreHistryEntry represents
        /// </summary>
        public string EventStyle { get; set; }

        public override string ToString()
        {
            StringBuilder foo = new StringBuilder();
            foo.Append("EventStyle: ");
            foo.Append(base.StartDate.ToString("MM/dd/yyyy"));
            foo.Append(" - ");
            foo.Append(base.UserId);
            foo.Append(" - ");
            foo.Append(EventStyle);
            return foo.ToString();
        }
    }
}
