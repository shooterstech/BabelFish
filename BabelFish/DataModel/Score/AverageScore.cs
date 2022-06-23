using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BabelFish.DataModel.Score
{
    public abstract class AverageScore
    {
        public AverageScoreField ScoreAverage { get; set; }
    }
}
