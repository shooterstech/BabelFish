using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.DataModel.ScoreHistory {
    public class ScoreHistoryEventStyleTimespan : ScoreHistoryTimespan {

        public ScoreHistoryEventStyleTimespan() { }



        /// <summary>
        /// String, formatted as a SetName, representing the Event Style this ScoreHistryEntry represents
        /// </summary>
        public string EventStyle { get; set; }
    }
}
