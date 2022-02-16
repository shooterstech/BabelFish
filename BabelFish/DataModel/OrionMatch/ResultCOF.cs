using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string AccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// The Local Date that this score was shot. 
        /// Formatted as yyyy-MM-dd
        /// </summary>
        public string LocalDate { get; set; } = string.Empty;

        public string MatchID { get; set; } = string.Empty;

        public EventScore EventScore { get; set; } = new EventScore();

        /// <summary>
        /// FUTURE, INTERMEDIATE, UNOFFICIAL, OFFICIAL
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// The Version string of the JSON document
        /// </summary>
        public string JSONVersion { get; set; } = string.Empty;

        /// <summary>
        /// GUID assigned to this result
        /// </summary>
        public string ResultCOFID { get; set; } = string.Empty;

        public string MatchType { get; set; } = string.Empty;

        public Participant Participant { get; set; } = new Individual();

        public string ParentID { get; set; } = string.Empty;

        /// <summary>
        /// GUID returned from API
        /// </summary>
        public string RESULTCOF_ResultCOFID { get; set; } = string.Empty;

        /// <summary>
        /// The GMT time this ResultCOF was last updated
        /// </summary>
        public DateTime LastUpdated { get; set; } = new DateTime();

        /// <summary>
        /// SetName of the Course Of Fire definition
        /// </summary>
        public string CourseOfFireDef { get; set; } = string.Empty;

        public string Owner { get; set; } = string.Empty;

        public string UniqueID { get; set; } = string.Empty;

        public string CheckSum { get; set; } = string.Empty;

        public string MatchName { get; set; } = string.Empty;

        /// <summary>
        /// The GUID of the orion app user who shot this score. Is blank if not known.
        /// </summary>
        public string UserID { get; set; } = string.Empty;
        //TODO: Not in API return data, is this supposed to be Individual().UserID?

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
