using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.Match {
    [Serializable]
    public class ResultCOF {

        public ResultCOF() {
            Participant = new Individual();
            EventScore = new EventScore();
        }

        public string MatchID { get; set; }

        public string ParentID { get; set; }

        /// <summary>
        /// The Version string of the JSON document
        /// </summary>
        public string JSONVersion { get; set; }

        /// <summary>
        /// The GMT time this ResultCOF was last updated
        /// </summary>
        public DateTime LastUpdated { get; set; }

        public string MatchName { get; set; }

        public string MatchType { get; set; }

        /// <summary>
        /// The Local Date that this score was shot. 
        /// Formatted as yyyy-MM-dd
        /// </summary>
        public string LocalDate { get; set; }

        /// <summary>
        /// GUID assigned to this result
        /// </summary>
        public string ResultCOFID { get; set; }

        /// <summary>
        /// SetName of the Course Of Fire definition
        /// </summary>
        public string CourseOfFireDef { get; set; }

        /// <summary>
        /// The GUID of the orion app user who shot this score. Is blank if not known.
        /// </summary>
        public string UserID { get; set; }

        public Participant Participant { get; set; }

        public EventScore EventScore { get; set; }

        /// <summary>
        /// FUTURE, INTERMEDIATE, UNOFFICIAL, OFFICIAL
        /// </summary>
        public string Status { get; set; }

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
