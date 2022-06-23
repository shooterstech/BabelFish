using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BabelFish.DataModel.Score
{
    public class EventStyleHistoryArguments : EventStyleArguments
    {
        [JsonProperty(Order = 8)]
        public bool? IncludeRelated { get; set; } = null;
    }
}
