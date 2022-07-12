using System;
using System.Collections.Generic;
using System.Text;
using ShootersTech.BabelFish.DataModel.Athena;

namespace ShootersTech.BabelFish.DataModel.ScoreHistory {
    public abstract class ScoreHistoryTimespan : ScoreHistoryBase {

        public ScoreHistoryTimespan() : base() { }

        public Score SumScore { get; set; }

    }
}
