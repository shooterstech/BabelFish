using System;
using System.Collections.Generic;
using System.Text;
using ShootersTech.DataModel.Athena;

namespace ShootersTech.DataModel.ScoreHistory {
    public abstract class ScoreHistoryTimespan : ScoreHistoryBase {

        public ScoreHistoryTimespan() : base() { }

        public Score SumScore { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

    }
}
