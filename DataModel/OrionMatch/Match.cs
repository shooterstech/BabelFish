using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.Match {
    [Serializable]
    public class Match {

        public Match() {
            SquaddingEvents = new List<SquaddingEvent>();
            Attributes = new List<Attribute>();
            ResultEvents = new List<ResultEventAbbr>();
            AuthorizationList = new List<MatchAuthorization>();
            CommonIncidentReports = new List<IncidentReportRuleDefinition>();
            MatchParticipantResults = new List<MatchParticipantResult>();
        }

        /// <summary>
        /// Unique MatchID for the competition to get squadding for. Will match exactly (assuming no errors) of the MatchID in the GetMatchDetailRequest
        /// 
        /// This is a required field.
        /// </summary>
        public string MatchID { get; set; }

        public string ParentID { get; set; }

        /// <summary>
        /// Name of the Match
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// External Result URL
        /// </summary>
        public string ResultURL { get; set; }

        /// <summary>
        /// Start Date of the Match. Formatted as YYYY-MM-dd
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// End Date of the Match. Formatted as YYYY-MM-dd
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// A human readable description of the match
        /// </summary>
        public string Description { get; set; }

        public string CourseOfFireDef { get; set; }

        public string MatchType { get; set; }

        public List<Attribute> Attributes { get; set; }

        /// <summary>
        /// The list of Events in the Match that have squadding. Also contains definitions on the types of OrionTargets used.
        /// 
        /// To pull the full squadding, use GetSquaddingListRequest()
        /// </summary>
        public List<SquaddingEvent> SquaddingEvents { get; set; }

        public List<ResultEventAbbr> ResultEvents { get; set; }

        public DateTime ResultEventsLastUpdate { get; set; }

        /// <summary>
        /// A list of common Incident Reports that may occure during the competition.
        /// </summary>
        public List<IncidentReportRuleDefinition> CommonIncidentReports { get; set; }

        public Location Location { get; set; }

        /// <summary>
        /// Sets the public visibility for the match. Valid values are
        /// Private : No visibility
        /// Protected : Only athletes, their coaches, and match officials may see
        /// Club : Same as participant, but also includes all club members of the sponsoring club
        /// Public : Everyone may view
        /// </summary>
        public string Visibility { get; set; }

        /// <summary>
        /// A list of Authorization roles participants in the match have.
        /// </summary>
        public List<MatchAuthorization> AuthorizationList { get; set; }

        public List<MatchParticipantResult> MatchParticipantResults { get; set; }

        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append("MatchDetail for ");
            foo.Append(Name);
            return foo.ToString();
        }
    }
}
