using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.DataModel.ScoreHistory {

    /// <summary>
    /// Represents the data returned by a Get score History API Call
    /// </summary>
    public class ScoreHistory  {

        /// <summary>
        /// Public base constructor.
        /// </summary>
        public ScoreHistory() { }

        public List<ScoreHistoryEntry> ScoreHistoryList { get; set; } = new List<ScoreHistoryEntry>();

    }
}
