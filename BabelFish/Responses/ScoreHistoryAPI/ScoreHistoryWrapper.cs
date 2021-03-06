using System;
using System.Collections.Generic;
using System.Text;
using ShootersTech.BabelFish.DataModel.ScoreHistory;

namespace ShootersTech.BabelFish.Responses.ScoreHistoryAPI {

    /// <summary>
    /// Helper class that creates the added structure in the data model needed for Deserialzing a Match object from json.
    /// </summary>
    public class ScoreHistoryWrapper {

        public ScoreHistory ScoreHistory { get; set; }
    }
}
