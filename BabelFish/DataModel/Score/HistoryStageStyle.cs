using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BabelFish.DataModel.Score
{
    [Serializable]
    public class StageStyleHistory : HistoryScore
    {
        public StageStyleHistoryArguments Arguments { get; set; }
    }
}
