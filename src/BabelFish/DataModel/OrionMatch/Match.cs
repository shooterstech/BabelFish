using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters.Microsoft;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    [Serializable]
    public class Match {

        private Logger logger = LogManager.GetCurrentClassLogger();

        public Match() { }


        /// <summary>
        /// After an object is deserialized form JSON,
        /// adds defaults to empty properties
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized()]
        public void OnDeserialized( StreamingContext context ) {
            if (ScoringSystems.Count == 0)
                ScoringSystems.Add( "Orion Scoring System" );
        }

        /// <summary>
        /// The list of Events in the Match that have squadding. Also contains definitions on the types of OrionTargets used.
        /// 
        /// To pull the full squadding, use GetSquaddingListRequest()
        /// </summary>
        [JsonPropertyOrder( 1 )]
        public List<SquaddingEvent> SquaddingEvents { get; set; } = new List<SquaddingEvent>();

        [JsonPropertyOrder( 2 )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.MatchIdConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.MatchIdConverter ) )]
        public MatchID? ParentID { get; set; }

        /// <summary>
        /// Returns the .ParentID as a MatchID instance.
        /// </summary>
        [Obsolete( "Use .ParentID" )]
        public MatchID GetParentId() {
            return ParentID;
        }

        /// <summary>
        /// The list of Events in the Match that have Result Lists associated with them.
        /// </summary>
        [JsonPropertyOrder( 3 )]
        public List<ResultEventAbbr> ResultEvents { get; set; } = new List<ResultEventAbbr>();

        /// <summary>
        /// External Result URL
        /// </summary>
        [JsonPropertyOrder( 4 )]
        public string ResultURL { get; set; } = string.Empty;

        /// <summary>
        /// The UTC time that the ResultEvent (scores for the match) were last updated.
        /// </summary>
        [JsonPropertyOrder( 5 )]
        public DateTime ResultEventsLastUpdate { get; set; } = new DateTime();

        [JsonPropertyOrder( 6 )]
        [Obsolete( "Use AttributeNames instead." )]
        public List<Attribute> Attributes { get; set; } = new List<Attribute>();

        /// <summary>
        /// List of Attribute SetNames used in this match.
        /// </summary>
        [JsonPropertyOrder( 6 )]
        public List<string> AttributeNames { get; set; } = new List<string>();

        /// <summary>
        /// The name of the Match
        /// </summary>
        public string Name {
            get; set;
        } = string.Empty;

        /// <summary>
        /// Sets the public visibility for the match. Valid values are
        /// Private : No visibility
        /// Protected : Only athletes, their coaches, and match officials may see
        /// Club : Same as participant, but also includes all club members of the sponsoring club
        /// Public : Everyone may view
        /// </summary>
        [JsonPropertyOrder( 8 )]
        public VisibilityOption Visibility { get; set; } = VisibilityOption.PRIVATE;

        /// <summary>
        /// The orion account or at home account who owns this match.
        /// </summary>
        /// <example>OrionAcct000001 or AtHomeAcct123456</example>
        [JsonPropertyOrder( 9 )]
        public string OwnerId { get; set; } = string.Empty;

        /// <summary>
        /// Unique MatchID for the competition to get squadding for. Will match exactly (assuming no errors) of the MatchID in the GetMatchDetailRequest
        /// 
        /// This is a required field.
        /// </summary>
        [JsonPropertyOrder( 11 )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.MatchIdConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.MatchIdConverter ) )]
        public MatchID MatchID { get; set; }

        /// <summary>
        /// The Version string of the JSON document.
        /// Version 2022-04-09 represents ResultCOF in a dictionary format
        /// Version < 2022 represent ResultCOF in a tree format
        /// </summary>
        [JsonPropertyOrder( 12 )]
        public string JSONVersion { get; set; } = string.Empty;

        /// <summary>
        /// The start date of the match.
        /// </summary>
        [JsonPropertyOrder( 13 )]
        [G_STJ_SER.JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// the end date of the match.
        /// </summary>
        [JsonPropertyOrder( 14 )]
        [G_STJ_SER.JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The type of match, as specified by the Match Director.
        /// </summary>
        [JsonPropertyOrder( 15 )]
        public CompetitionTypeOptions MatchType { get; set; } = CompetitionTypeOptions.LOCAL_MATCH;

        /// <summary>
        /// The COURSE OF FIRE definition used in the conduct of this match.
        /// </summary>
        /// <remarks>In the future MatchV2 class, Matches will be able to have multiple COURSES OF FIRE. This property will be replaced.</remarks>
        [JsonPropertyOrder( 16 )]
        public string CourseOfFireDef { get; set; } = string.Empty;

        /// <summary>
        /// SetName of the ScoreConfig used in this match.
        /// NOTE: The name of the ScoreFormatCollection is specified in the Course of Fire 
        /// </summary>
        /// <remarks>In the future MatchV2 class, Matches will be able to have multiple COURSES OF FIRE, and with each CourseOfFireDef will have its own ScoreConfigName. This property will be replaced.</remarks>
        [JsonPropertyOrder( 17 )]
        public string ScoreConfigName { get; set; }

        /// <summary>
        /// Name of the TargetCollection used in this match.
        /// </summary>
        /// <remarks>In the future MatchV2 class, Matches will be able to have multiple COURSES OF FIRE, and with each CourseOfFireDef will have its own TargetColle3citonName. This property will be replaced.</remarks>
        [JsonPropertyOrder( 18 )]
        public string TargetCollectionName { get; set; }

        /// <summary>
        /// String holding the software (Orion Scoring System) and Version number of the software that created or last updated this match.
        /// </summary>
        [JsonPropertyOrder( 19 )]
        public string Creator { get; set; }

        /// <summary>
        /// The location of the match.
        /// </summary>
        [JsonPropertyOrder( 20 )]
        public Location Location { get; set; } = new Location();

        /// <summary>
        /// A list of authorized capabilities the caller has for this match. These values are 
        /// returned by the Rest API, but are not sent to the cloud. Instead 'AuthorizationList'
        /// is sent, and the list of Authorizations is derved using it and the caller's identificaiton.
        /// </summary>
        /// <remarks>Should really rename this to Capabilities or AuthorizedCapabilities</remarks>
        [JsonPropertyOrder( 21 )]
        public List<MatchAuthorizationCapability> Authorization { get; set; } = new List<MatchAuthorizationCapability>();

        /// <summary>
        /// NewtonSoft helper method to determine if Authoirazation should be serialized.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeAuthorization() {
            return Authorization is not null && Authorization.Count > 0;
        }

        /*
         * EKA Note November 2025
         * Removed as a property, because Role Authorization is saved instead to the MatchParticipant object. No need to replicate that data here.
         *
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
        */

        /*
         * EKA Note November 2025
         * Removed as a property, because its been marked as deprecated for so long.
         * 
        /// <summary>
        /// A list of match participants, but only the athletes, not the teams. 
        /// 
        /// This information is largely rhetotical with Get Participant List API call.
        ///
        /// This list is only ever uploaded to the cloud. It is never (or at least should never) be
        /// sent back as part of an API request.
        /// </summary>
        [Obsolete( "Will be replaced soon with a more proper participant list." )]
        public List<MatchParticipantResult> MatchParticipantResults { get; set; } = new List<MatchParticipantResult>();
        */

        /*
         * EKA Note November 2025
         * Removed as a property, because its been marked as deprecated for so long.
         * 
        /// <summary>
        /// A list of Result COF that the logged in user owns for this match. Meaning, these are the
        /// scores the logged in user shot. If a user is not logged in, or the logged in user is
        /// not an athletes, then this will be an empty list.
        /// </summary>
        [JsonPropertyOrder ( 20 )]
        [Obsolete( "Format of this data is in the old ResultCOF (pre 2022). Make a separate call using GetResultCOF() instead, which returns data in the 2022 format." )]
        public List<ResultCOF> ParticipantResults { get; set; } = new List<ResultCOF>();
        */

        /// <summary>
        /// Contact information for the match administrators.
        /// </summary>
        [JsonPropertyOrder( 31 )]
        public List<Contact> MatchContacts { get; set; } = new List<Contact>();

        /// <summary>
        /// A list of scoring system names used in this match.
        /// </summary>
        [JsonPropertyOrder( 32 )]
        public List<string> ScoringSystems { get; set; } = new List<string>();

        /// <summary>
        /// Newtonsoft.json helper method, to determine if ScoreSystems property should be serialized.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeScoringSystems() {
            return ScoringSystems != null && ScoringSystems.Count > 0;
        }

        /// <summary>
        /// The type of scoring system used in this match.
        /// </summary>
        [JsonPropertyOrder( 33 )]
        [DefaultValue( ScoringSystem.UNKNOWN )]
        public ScoringSystem ScoringSystemType { get; set; } = ScoringSystem.UNKNOWN;

        /// <summary>
        /// A list of MatchHtmlReports (e.g. pressrelease.html) that exist for this match.
        /// </summary>
        public List<MatchHtmlReport> HtmlReports { get; set; } = new List<MatchHtmlReport>();

        /// <summary>
        /// Newtonsoft.json helper method, to determine if HtmlReports property should be serialized.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeHtmlReports() {
            return HtmlReports is not null && HtmlReports.Count > 0;
        }

        /// <summary>
        /// The SharedKey is a defacto password. Allowing systems on the outside to
        /// make change requests to the match, such as add athletes or teams, insert
        /// shot data, etc.
        /// </summary>
        [JsonPropertyOrder( 50 )]
        [Obsolete( "Replaced with user based Authorization with RoleAuthorization" )]
        public string SharedKey { get; set; } = String.Empty;

        /// <summary>
        /// UTC time the match data was last updated.
        /// </summary>
        [JsonPropertyOrder( 99 )]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Helper function that indicates if this Match is currently going on. Which is 
        /// determined by the Match's Start and End Date.
        /// </summary>
        [JsonIgnore]
        public bool IsOnGoing {
            get {
                return StartDate <= DateTime.Today
                    && EndDate >= DateTime.Today;
            }
        }

        /// <inheritdoc />
        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append( "MatchDetail for " );
            foo.Append( Name );
            return foo.ToString();
        }
    }
}
