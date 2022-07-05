using System;
using System.Collections.Generic;
using System.Text;
using ShootersTech.DataModel.Definitions;

namespace ShootersTech.DataModel.ScoreHistory {

    /// <summary>
    /// Represents the data returned by a GetScoreAverage API call.
    /// </summary>
    public class ScoreAverage {

        public ScoreAverage() { }

        public List<ScoreAverageBase> ScoreAverageList { get; set; } = new List<ScoreAverageBase>();

    }
}
