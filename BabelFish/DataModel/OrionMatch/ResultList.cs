using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShootersTech.BabelFish.DataModel.OrionMatch
{
    [Serializable]
    public class ResultList {

        public const string STATUS_PRELIMINARY = "PRELIMINARY";
        public const string STATUS_UNOFFICIAL = "UNOFFICIAL";
        public const string STATUS_OFFICIAL = "OFFICIAL";

        public ResultList() {
            Results = new List<ResultEvent>();
        }

        [JsonProperty(Order = 1)]
        public string MatchID { get; set; } = string.Empty;

        /// <summary>
        /// Set name of the Ranking Rule definition
        /// </summary>
        [JsonProperty(Order = 2)]
        public string RankingRuleDef { get; set; } = string.Empty;

        /// <summary>
        /// Partial, Unofficial, Official
        /// </summary>
        [JsonProperty(Order = 3)]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// The Version string of the JSON document
        /// </summary>
        [JsonProperty(Order = 4)]
        public string JSONVersion { get; set; } = string.Empty;

        [JsonProperty(Order = 5)]
        public bool Team { get; set; } = false;

        [JsonProperty(Order = 6)]
        public string ParentID { get; set; } = string.Empty;

        /// <summary>
        /// The relative importance / sort order of this ResultList within the match
        /// </summary>
        [JsonProperty(Order = 7)]
        public int SortOrder { get; set; } = 0;

        [JsonProperty(Order = 8)]
        public List<ResultEvent> Results { get; set; } = new List<ResultEvent>();

        /// <summary>
        /// Deprecated, use ResultName
        /// </summary>
        [Obsolete("Deprecated, use ResultName") ]
        [JsonProperty(Order = 15)]
        public string Name { get; set; } = string.Empty;

        [JsonProperty(Order = 9)]
        public string ResultName { get; set; } = string.Empty;

        [JsonProperty(Order = 10)]
        public DateTime LastUpdated { get; set; } = new DateTime();

        [JsonProperty(Order = 11)]
        public string Owner { get; set; } = string.Empty;

        /// <summary>
        /// Set to true if this ResultList is considered one of the most important and should be featured
        /// </summary>
        [JsonProperty(Order = 12)]
        public bool Primary { get; set; } = false;

        [JsonProperty(Order = 13)]
        public string UniqueID { get; set; } = string.Empty;

        [JsonProperty(Order = 14)]
        public string EventName { get; set; } = string.Empty;

        [JsonProperty(Order = 15)]
        public string ResultListID { get; set; } = string.Empty;


        /// <summary>
        /// The SetName of the Course of Fire
        /// </summary>
        public string CourseOfFireDef { get; set; } = string.Empty;

        public string ScoreConfigName { get; set; } = string.Empty;

        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append("ResultList for ");
            foo.Append(ResultName);
            return foo.ToString();
        }
    }
}
