using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class Match : ResponseTemplate {

        public Match() {
            SquaddingEvents = new List<SquaddingEvent>();
            Attributes = new List<Attribute>();
            ResultEvents = new List<ResultEventAbbr>();
            CommonIncidentReports = new List<IncidentReportRuleDefinition>();
            Contact = new MatchContact();
            ScoringSystems = new List<string>();
        }

        /// <summary>
        /// After an object is deserialized form JSON, checks that the properties are
        /// properly initialized, and not null.
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized()]
        public void OnDeserialized(StreamingContext context) {
            if (SquaddingEvents == null)
                SquaddingEvents = new List<SquaddingEvent>();

            if (Attributes == null)
                Attributes = new List<Attribute>();

            if (ResultEvents == null)
                ResultEvents = new List<ResultEventAbbr>();

            if (CommonIncidentReports == null)
                CommonIncidentReports = new List<IncidentReportRuleDefinition>();

            if (Contact == null)
                Contact = new MatchContact();

            if (ScoringSystems == null)
                ScoringSystems = new List<string>() { "Orion Scoring System" };

        }

        /// <summary>
        /// The list of Events in the Match that have squadding. Also contains definitions on the types of OrionTargets used.
        /// 
        /// To pull the full squadding, use GetSquaddingListRequest()
        /// </summary>
        public List<SquaddingEvent> SquaddingEvents { get; set; }

        public string ParentID { get; set; }

        public List<ResultEventAbbr> ResultEvents { get; set; }

        /// <summary>
        /// External Result URL
        /// </summary>
        public string ResultURL { get; set; }

        public DateTime ResultEventsLastUpdate { get; set; }

        public List<Attribute> Attributes { get; set; }

        /// <summary>
        /// Name of the Match
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Sets the public visibility for the match. Valid values are
        /// Private : No visibility
        /// Protected : Only athletes, their coaches, and match officials may see
        /// Club : Same as participant, but also includes all club members of the sponsoring club
        /// Public : Everyone may view
        /// </summary>
        public string Visibility { get; set; }

        public string AccountNumber { get; set; }

        /// <summary>
        /// Start Date of the Match. Formatted as YYYY-MM-dd
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// Unique MatchID for the competition to get squadding for. Will match exactly (assuming no errors) of the MatchID in the GetMatchDetailRequest
        /// 
        /// This is a required field.
        /// </summary>
        public string MatchID { get; set; }

        public string JSONVersion { get; set; }

        /// <summary>
        /// End Date of the Match. Formatted as YYYY-MM-dd
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// A list of common Incident Reports that may occure during the competition.
        /// </summary>
        public List<IncidentReportRuleDefinition> CommonIncidentReports { get; set; }

        public string CourseOfFireDef { get; set; }

        public Location Location { get; set; }

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
        public List<string> Authorization { get; set; }

        /// <summary>
        /// A list of Authorization roles participants in the match have.
        /// This list is sent to the Cloud, but is never seen as part of the Rest API. Instead
        /// the Rest API sends back a list of Authorizations the caller has in the match, with 
        /// the Property 'Authorization'.
        ///
        /// This list is only ever uploaded to the cloud. It is never (or at least should never) be
        /// sent back as part of an API request. 
        /// </summary>
        public List<MatchAuthorization> AuthorizationList { get; set; }


        /// <summary>
        /// A list of match participants, but only the athletes, not the teams. 
        ///
        /// This list is only ever uploaded to the cloud. It is never (or at least should never) be
        /// sent back as part of an API request.
        /// </summary>
        [Obsolete( "Will be replaced soon with a more proper participant list." )]
        public List<MatchParticipantResult> MatchParticipantResults { get; set; }

        /// <summary>
        /// A list of Result COF that the logged in user owns for this match. Meaning, these are the
        /// scores the logged in user shot. If a user is not logged in, or the logged in user is
        /// not an athletes, then this will be an empty list.
        /// </summary>
        public List<ResultCOF> ParticipantResults { get; set; }

        /// <summary>
        /// Contact information for the match, i.e. person's name, phone, email.
        /// </summary>
        public MatchContact MatchContact { get; set; }

        /// <summary>
        /// A list of scoring systems used in this match.
        /// </summary>
        public List<string> ScoringSystems { get; set; }

        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append("MatchDetail for ");
            foo.Append(Name);
            return foo.ToString();
        }
    }
}
