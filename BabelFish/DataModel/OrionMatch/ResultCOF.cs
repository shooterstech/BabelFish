using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BabelFish.DataModel.OrionMatch {

    public class CourseOfFireWrapper
    {
        public ResultCOF ResultCOF = new ResultCOF();

        public override string ToString()
        {
            StringBuilder foo = new StringBuilder();
            foo.Append("ResultCOF for ");
            foo.Append(ResultCOF.EventScore.EventName);
            foo.Append(": ");
            foo.Append(ResultCOF.Participant.DisplayName);
            return foo.ToString();
        }
    }

    [Serializable]
    public class ResultCOF
    {
        [JsonProperty(Order = 1)]
        public string AccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// The Local Date that this score was shot. 
        /// Formatted as yyyy-MM-dd
        /// </summary>
        [JsonProperty(Order = 2)]
        public string LocalDate { get; set; } = string.Empty;

        [JsonProperty(Order = 3)]
        public string MatchID { get; set; } = string.Empty;

        [JsonProperty(Order = 4)]
        public EventScore EventScore { get; set; } = new EventScore();

        /// <summary>
        /// FUTURE, INTERMEDIATE, UNOFFICIAL, OFFICIAL
        /// </summary>
        [JsonProperty(Order = 5)]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// The Version string of the JSON document
        /// </summary>
        [JsonProperty(Order = 6)]
        public string JSONVersion { get; set; } = string.Empty;

        /// <summary>
        /// GUID assigned to this result
        /// </summary>
        [JsonProperty(Order = 7)]
        public string ResultCOFID { get; set; } = string.Empty;

        [JsonProperty(Order = 8)]
        public string MatchType { get; set; } = string.Empty;

        [JsonProperty(Order = 9)]
        public Participant Participant { get; set; } = new Individual();

        [JsonProperty(Order = 10)]
        public string ParentID { get; set; } = string.Empty;

        /// <summary>
        /// GUID returned from API
        /// </summary>
        [JsonProperty(Order = 11)]
        public string RESULTCOF_ResultCOFID { get; set; } = string.Empty;

        /// <summary>
        /// The GMT time this ResultCOF was last updated
        /// </summary>
        [JsonProperty(Order = 12)]
        public DateTime LastUpdated { get; set; } = new DateTime();

        /// <summary>
        /// SetName of the Course Of Fire definition
        /// </summary>
        [JsonProperty(Order = 13)]
        public string CourseOfFireDef { get; set; } = string.Empty;

        [JsonProperty(Order = 14)]
        public string Owner { get; set; } = string.Empty;

        [JsonProperty(Order = 15)]
        public string UniqueID { get; set; } = string.Empty;

        [JsonProperty(Order = 16)]
        public string CheckSum { get; set; } = string.Empty;

        [JsonProperty(Order = 17)]
        public string MatchName { get; set; } = string.Empty;

        /// <summary>
        /// The GUID of the orion app user who shot this score. Is blank if not known.
        /// </summary>
        public string UserID { get; set; } = string.Empty;

        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append("ResultCOF for ");
            foo.Append(EventScore.EventName);
            foo.Append(": ");
            foo.Append(Participant.DisplayName);
            return foo.ToString();
        }
    }
}
