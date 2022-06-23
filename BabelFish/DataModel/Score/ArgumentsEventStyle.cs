using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BabelFish.DataModel.Score
{
    public class EventStyleArguments : ScoreArguments
    {
        [JsonProperty(Order = 9)]
        //public Definitions.SetName? EventStyle { get; set; } = null;
        public string EventStyle { get; set; } = string.Empty;

    }
}
