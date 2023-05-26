using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.OrionMatch
{
    /// <summary>
    /// Result COF format for (JSONVersion) "2022-04-09"
    /// </summary>
    [Serializable]
    public class ResultCOF
    {
        [JsonProperty(Order = 1)]
        public string AccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// GUID assigned to this result
        /// </summary>
        [JsonProperty(Order = 2)]
        public string ResultCOFID { get; set; } = string.Empty;

        /// <summary>
        /// The Owner of this data. 
        /// If it starts with "OrionAcct" this it is owned by a club, and the data is considered public.
        /// If it is a GUID, this it is the User ID of the person who owns the data, and is considered protected.
        /// </summary>
        [JsonProperty(Order = 3)]
        public string Owner { get; set; } = string.Empty;

        /// <summary>
        /// The Version string of the JSON document.
        /// Should be "2022-04-09"
        /// </summary>
        [JsonProperty(Order = 4)]
        public string JSONVersion { get; set; } = string.Empty;

        /// <summary>
        /// The IoT Topic to monitor to receive live updates to this Result Course of Fire.
        /// Note if the Result COF is completed, no updated will be provided on this topic. 
        /// </summary>
        [JsonProperty(Order = 5)]
        public string LiveTopic { get; set; } = string.Empty;

        /// <summary>
        /// The GMT time this ResultCOF was last updated
        /// </summary>
        [JsonProperty(Order = 6)]
        public DateTime LastUpdated { get; set; } = new DateTime();

        /// <summary>
        /// Boolean indicating if this is a partial Result COF, containing only delta. 
        /// ResultCOF pulled from the REST API, this value should be false.
        /// ResultCOF pushed from the IoT Topic, this value should be true.
        /// </summary>
        [JsonProperty(Order = 7)]
        public bool Delta { get; set; }

        /// <summary>
        /// Unique ID for the match.
        /// </summary>
        [JsonProperty(Order = 10)]
        public string MatchID { get; set; } = string.Empty;

        /// <summary>
        /// Human readable name of the match.
        /// </summary>
        [JsonProperty(Order = 11)]
        public string MatchName { get; set; } = string.Empty;

        /// <summary>
        /// City, state, and possible country of the location of the match
        /// </summary>
        public string MatchLocation { get; set; } = "";

        /// <summary>
        /// Unique ID for the parent of this match, if this is a Virtual Match. If this is not a
        /// Virtual Match, then it will be the same value as MatchID.
        /// </summary>
        [JsonProperty(Order = 12)]
        public string ParentID { get; set; } = string.Empty;


        [JsonProperty( Order = 13 )]
        [JsonConverter( typeof( StringEnumConverter ) )]
        public MatchTypeOptions MatchType { get; set; } = MatchTypeOptions.TRAINING;

        /// <summary>
        /// The Local Date that this score was shot. 
        /// NOTE Local Date is not necessarily the same as the GMT date.
        /// Formatted as yyyy-MM-dd
        /// </summary>
        [JsonProperty(Order = 14)]
        public string LocalDate { get; set; } = string.Empty;

        /// <summary>
        /// FUTURE, INTERMEDIATE, UNOFFICIAL, OFFICIAL
        /// </summary>
        [JsonProperty(Order = 15)]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// SetName of the Course Of Fire definition
        /// </summary>
        [JsonProperty(Order = 20)]
        public string CourseOfFireDef { get; set; } = string.Empty;

        /// <summary>
        /// SetName of the ScoreConfig used in this match.
        /// NOTE: The name of the ScoreFormatCollection is specified in the Course of Fire 
        /// </summary>
        [JsonProperty(Order = 21)]
        public string ScoreConfigName { get; set; }

        /// <summary>
        /// Name of the TargetCollection used in this match.
        /// </summary>
        [JsonProperty(Order = 22)]
        public string TargetCollectionName { get; set; }


        /// <summary>
        /// The GUID of the orion app user who shot this score. Is blank if not known.
        /// </summary>
        [JsonProperty(Order = 30)]
        public string UserID { get; set; } = string.Empty;

        /// <summary>
        /// Data on the person or team who shot this score.
        /// </summary>
        [JsonProperty(Order = 31)]
        public Participant Participant { get; set; } = new Individual();

        /// <summary>
        /// Scores for each composite Event.
        /// The Key of the Dictionary is the Event Name. the Value is the Event Score
        /// </summary>
        [JsonProperty(Order = 40)]
        public Dictionary<string, EventScore> EventScores { get; set; } = new Dictionary<string, EventScore>();

        /// <summary>
        /// Scores for each Singular Event (usually a Shot).
        /// The Key is the sequence number, which is represented here as a string, but is really a float. The Value is the Shot object
        /// </summary>
        [JsonProperty(Order = 50)]
        public Dictionary<string, Scopos.BabelFish.DataModel.Athena.Shot.Shot> Shots = new Dictionary<string, Scopos.BabelFish.DataModel.Athena.Shot.Shot>();

        /// <summary>
        /// Describes how to display shot graphics and (text) scores to spectators, during a Live event.
        /// </summary>
        [JsonProperty(Order = 60)]
        public Scopos.BabelFish.DataModel.Athena.DataFormat.ShotGraphicShow LiveDisplay { get; set; }

        /// <summary>
        /// Describes how to display shot graphics and (text) scores to spectators, after an event is completed.
        /// </summary>
        [JsonProperty(Order = 61)]
        public List<Scopos.BabelFish.DataModel.Athena.DataFormat.ShotGraphicShow> PostDisplay { get; set; }

        /// <summary>
        /// GUID returned from API
        /// EKA: This field is used for storage in Dynamo only. 
        /// </summary>
        //public string RESULTCOF_ResultCOFID { get; set; } = string.Empty;

        /// <summary>
        /// EKA: This field is used for storage in Dynamo only. 
        /// </summary>
        //public string UniqueID { get; set; } = string.Empty;

        /// <summary>
        /// EKA: This field is used for storage in Dynamo only. 
        /// </summary>
        //public string CheckSum { get; set; } = string.Empty;

        public override string ToString()
        {
            StringBuilder foo = new StringBuilder();
            foo.Append("ResultCOF for ");
            foo.Append(Participant.DisplayName);
            foo.Append(": ");
            foo.Append(MatchName);
            return foo.ToString();
        }
    }
}