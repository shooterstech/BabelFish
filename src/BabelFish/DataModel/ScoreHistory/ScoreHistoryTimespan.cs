using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Athena;

namespace Scopos.BabelFish.DataModel.ScoreHistory {
    public abstract class ScoreHistoryTimespan : ScoreHistoryBase {

        public ScoreHistoryTimespan() : base() { }

        public Score SumScore { get; set; }

    }
}
