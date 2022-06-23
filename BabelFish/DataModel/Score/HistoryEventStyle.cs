using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BabelFish.DataModel.Score
{
    [Serializable]
    public class EventStyleHistory : HistoryScore
    {
        public EventStyleHistoryArguments Arguments { get; set; }
    }
}
