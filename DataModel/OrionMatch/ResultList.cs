using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class ResultList : ResponseTemplate {

        public const string STATUS_PRELIMINARY = "PRELIMINARY";
        public const string STATUS_UNOFFICIAL = "UNOFFICIAL";
        public const string STATUS_OFFICIAL = "OFFICIAL";

        public ResultList() {
            Results = new List<ResultEvent>();
        }

        public string MatchID { get; set; }

        /// <summary>
        /// Set name of the Ranking Rule definition
        /// </summary>
        public string RankingRuleDef { get; set; }

        /// <summary>
        /// Partial, Unofficial, Official
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The Version string of the JSON document
        /// </summary>
        public string JSONVersion { get; set; }

        public bool Team { get; set; }

        public string ParentID { get; set; }

        /// <summary>
        /// The relative importance / sort order of this ResultList within the match
        /// </summary>
        public int SortOrder { get; set; }

        public List<ResultEvent> Results { get; set; }

        /// <summary>
        /// Deprecated, use ResultName
        /// </summary>
        [Obsolete("Deprecated, use ResultName") ]
        public string Name { get; set; }

        public string ResultName { get; set; }

        public DateTime LastUpdated { get; set; }

        public string Owner { get; set; }

        /// <summary>
        /// Set to true if this ResultList is considered one of the most important and should be featured
        /// </summary>
        public bool Primary { get; set; }

        public string UniqueID { get; set; }

        public string EventName { get; set; }

        public string ResultListID { get; set; }


        //????///////// <summary>
        //????///////// The SetName of the Course of Fire
        //????///////// </summary>
        //????//////public string CourseOfFireDef { get; set; }

        //????//////public string ScoreConfigName { get; set; }

        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append("ResultList for ");
            foo.Append(ResultName);
            return foo.ToString();
        }
    }
}
