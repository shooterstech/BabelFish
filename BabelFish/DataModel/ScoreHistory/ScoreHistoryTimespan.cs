using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.DataModel.ScoreHistory {
    public abstract class ScoreHistoryTimespan {

        public ScoreHistoryTimespan() { }

        public int NumberOfShots { get; set; }

        public Score SumScore { get; set; }

    }
}
