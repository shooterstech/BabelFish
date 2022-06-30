using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.DataModel.ScoreHistory {

    /// <summary>
    /// Data model that represents one stage of one athlete's score shot within a Course of Fire. 
    /// </summary>
    public class ScoreHistoryStageStyleEntry : ScoreHistoryEntry {

        public const int CONCRETE_CLASS_ID = 2;

        public ScoreHistoryStageStyleEntry() : base() {
            this.ConcreteClassId = CONCRETE_CLASS_ID;
        }



        /// <summary>
        /// String, formatted as a SetName, representing the Stage Style this ScoreHistryEntry represents
        /// </summary>
        public string StageStyle { get; set; }
    }
}
