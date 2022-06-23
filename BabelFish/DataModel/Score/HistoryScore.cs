using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BabelFish.DataModel.Score
{
    public abstract class HistoryScore
    {
        public HistoryScoreField ScoreHistory { get; set; }
    }
}
