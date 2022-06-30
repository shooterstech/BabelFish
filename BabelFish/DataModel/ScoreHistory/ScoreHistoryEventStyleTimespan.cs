using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.DataModel.ScoreHistory {
    public class ScoreHistoryEventStyleTimespan : ScoreHistoryTimespan {


        public const int CONCRETE_CLASS_ID = 3;

        public ScoreHistoryEventStyleTimespan() : base() {
            this.ConcreteClassId = CONCRETE_CLASS_ID;
        }



        /// <summary>
        /// String, formatted as a SetName, representing the Event Style this ScoreHistryEntry represents
        /// </summary>
        public string EventStyle { get; set; }
    }
}
