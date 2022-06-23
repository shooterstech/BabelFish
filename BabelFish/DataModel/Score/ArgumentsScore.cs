using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BabelFish.DataModel.Score
{
    public abstract class ScoreArguments
    {
        [JsonProperty(Order = 2)]
        public DateTime StartDate { get; set; }

        [JsonProperty(Order = 3)]
        public DateTime EndDate { get; set; }

        [JsonProperty(Order = 4)]
        public List<Guid> UserIDs { get; set; }

        [JsonProperty(Order = 5)]
        public Helpers.ScoreFormat Format { get; set; }

        [JsonProperty(Order = 6)]
        public bool PublicOnly { get; set; }

        [JsonProperty(Order = 7)]
        public int Limit { get; set; }
    }
}
