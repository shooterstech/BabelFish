using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.OrionMatch
{

    [Serializable]
    public class Match
    {

        public Match() { }

        /// <summary>
        /// After an object is deserialized form JSON,
        /// adds defaults to empty properties
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized()]
        public void OnDeserialized(StreamingContext context)
        {
            if (ScoringSystems.Count == 0)
                ScoringSystems.Add("Orion Scoring System");
        }

        /// <summary>
        /// The list of Events in the Match that have squadding. Also contains definitions on the types of OrionTargets used.
        /// 
        /// To pull the full squadding, use GetSquaddingListRequest()
        /// </summary>
        [JsonProperty(Order = 1)]
        public List<SquaddingEvent> SquaddingEvents { get; set; } = new List<SquaddingEvent>();

        [JsonProperty(Order = 2)]
        public string ParentID { get; set; } = string.Empty;

        [JsonProperty(Order = 3)]
        public List<ResultEventAbbr> ResultEvents { get; set; } = new List<ResultEventAbbr>();

        /// <summary>
        /// External Result URL
        /// </summary>
        [JsonProperty(Order = 4)]
        public string ResultURL { get; set; } = string.Empty;

        [JsonProperty(Order = 5)]
        public DateTime ResultEventsLastUpdate { get; set; } = new DateTime();

        [JsonProperty(Order = 6)]
        [Obsolete("Use AttributeNames instead.")]
        public List<Attribute> Attributes { get; set; } = new List<Attribute>();

        /// <summary>
        /// List of Attribute SetNames used in this match.
        /// </summary>
        [JsonProperty(Order = 6)]
        public List<string> AttributeNames { get; set; } = new List<string>();

        /// <summary>
        /// Name of the Match
        /// </summary>
        [JsonProperty(Order = 7)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Sets the public visibility for the match. Valid values are
        /// Private : No visibility
        /// Protected : Only athletes, their coaches, and match officials may see
        /// Club : Same as participant, but also includes all club members of the sponsoring club
        /// Public : Everyone may view
        /// </summary>
        [JsonProperty(Order = 8)]
        public string Visibility { get; set; } = string.Empty;

        [JsonProperty(Order = 9)]
        public string AccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// Start Date of the Match. Formatted as YYYY-MM-dd
        /// </summary>
        [JsonProperty(Order = 10)]
        public string StartDate { get; set; } = string.Empty;

        /// <summary>
        /// Unique MatchID for the competition to get squadding for. Will match exactly (assuming no errors) of the MatchID in the GetMatchDetailRequest
        /// 
        /// This is a required field.
        /// </summary>
        [JsonProperty(Order = 11)]
        public string MatchID { get; set; } = string.Empty;

        /// <summary>
        /// The SharedKey is a defacto password. Allowing systems on the outside to
        /// make change requests to the match, such as add athletes or teams, insert
        /// shot data, etc.
        /// </summary>
        public string SharedKey { get; set; } = String.Empty;

        /// <summary>
        /// The Version string of the JSON document.
        /// Version 2022-04-09 represents ResultCOF in a dictionary format
        /// Version < 2022 represent ResultCOF in a tree format
        /// </summary>
        [JsonProperty(Order = 12)]
        public string JSONVersion { get; set; } = string.Empty;

        /// <summary>
        /// End Date of the Match. Formatted as YYYY-MM-dd
        /// </summary>
        [JsonProperty(Order = 13)]
        public string EndDate { get; set; } = string.Empty;

        /// <summary>
        /// A list of common Incident Reports that may occure during the competition.
        /// </summary>
        [JsonProperty(Order = 14)]
        public List<IncidentReportRuleDefinition> CommonIncidentReports { get; set; } = new List<IncidentReportRuleDefinition>();

        [JsonProperty(Order = 15)]
        public string CourseOfFireDef { get; set; } = string.Empty;

        /// <summary>
        /// SetName of the ScoreConfig used in this match.
        /// NOTE: The name of the ScoreFormatCollection is specified in the Course of Fire 
        /// </summary>
        [JsonProperty(Order = 16)]
        public string ScoreConfigName { get; set; }

        /// <summary>
        /// Name of the TargetCollection used in this match.
        /// </summary>
        [JsonProperty(Order = 17)]
        public string TargetCollectionName { get; set; }

        [JsonProperty(Order = 18)]
        public Location Location { get; set; } = new Location();

        /// <summary>
        /// A list of authorizations the caller has for this match. These values are 
        /// returned by the Rest API, but are not sent to the cloud. Instead 'AuthorizationList'
        /// is sent, and the list of Authorizations is derved using it and the caller's identificaiton.
        /// </summary>
        /// TODO: The list of Authorization is finite. Convert this property to be a list of enum values.
        /// The optional values are:
        /// Read Incident Reports
        /// Create Incident Reports
        /// Update Incident Reports
        /// Close Incident Reports
        /// Create Target Images
        /// Create Entries
        /// Update Entries
        /// Delete Entries
        /// Read Scores
        /// Read Results
        /// Read Squadding
        /// Read Personal Scores
        /// Read Personal Results
        /// Read Personal Squadding
        /// Read Personal Incident Reports
        ///
        [JsonProperty(Order = 19)]
        public List<string> Authorization { get; set; } = new List<string>();

        /// <summary>
        /// A list of Authorization roles participants in the match have.
        /// This list is sent to the Cloud, but is never seen as part of the Rest API. Instead
        /// the Rest API sends back a list of Authorizations the caller has in the match, with 
        /// the Property 'Authorization'.
        ///
        /// This list is only ever uploaded to the cloud. It is never (or at least should never) be
        /// sent back as part of an API request. 
        /// </summary>
        public List<MatchAuthorization> AuthorizationList { get; set; } = new List<MatchAuthorization>();


        /// <summary>
        /// A list of match participants, but only the athletes, not the teams. 
        ///
        /// This list is only ever uploaded to the cloud. It is never (or at least should never) be
        /// sent back as part of an API request.
        /// </summary>
        [Obsolete("Will be replaced soon with a more proper participant list.")]
        public List<MatchParticipantResult> MatchParticipantResults { get; set; } = new List<MatchParticipantResult>();

        /// <summary>
        /// A list of Result COF that the logged in user owns for this match. Meaning, these are the
        /// scores the logged in user shot. If a user is not logged in, or the logged in user is
        /// not an athletes, then this will be an empty list.
        /// </summary>
        [JsonProperty(Order = 20)]
        [Obsolete("Format of this data is in the old ResultCOF (pre 2022). Make a separate call using GetResultCOF() instead, which returns data in the 2022 format.")]
        public List<ResultCOF> ParticipantResults { get; set; } = new List<ResultCOF>();

        /// <summary>
        /// Contact information for the match, i.e. person's name, phone, email.
        /// </summary>
        public MatchContact MatchContact { get; set; } = new MatchContact();

        /// <summary>
        /// A list of scoring systems used in this match.
        /// </summary>
        public List<string> ScoringSystems { get; set; } = new List<string>();

        public override string ToString()
        {
            StringBuilder foo = new StringBuilder();
            foo.Append("MatchDetail for ");
            foo.Append(Name);
            return foo.ToString();
        }
    }
}