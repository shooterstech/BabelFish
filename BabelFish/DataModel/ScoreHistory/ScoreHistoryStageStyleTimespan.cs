using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.DataModel.ScoreHistory {
    public class ScoreHistoryStageStyleTimespan : ScoreHistoryTimespan {

        public ScoreHistoryStageStyleTimespan() { }



        /// <summary>
        /// String, formatted as a SetName, representing the Stage Style this ScoreHistryEntry represents
        /// </summary>
        public string StageStyle { get; set; }

    }
}
