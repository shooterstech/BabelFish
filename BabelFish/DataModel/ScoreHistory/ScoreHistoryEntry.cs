using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.Athena;

namespace Scopos.BabelFish.DataModel.ScoreHistory {


    public abstract class ScoreHistoryEntry : ScoreHistoryBase {

        /// <summary>
        /// Public constructor
        /// </summary>
        public ScoreHistoryEntry() : base() { }

        /// <summary>
        /// The UUID formatted result cof id for this ScoreHistoryEntry.
        /// This field may be an empty string if the object is a member of a ScoreHistoryEventStyleEntry's .StageScores list
        /// </summary>
        public string ResultCOFID { get; set; } = "";

        /// <summary>
        /// String formatted as a SetName, representing the CourseOfFire Definition shot for this ScoreHistoryEntry
        /// This field may be an empty string if the object is a member of a ScoreHistoryEventStyleEntry's .StageScores list
        /// </summary>
        public string CourseOfFireDef { get; set; } = "";

        /// <summary>
        /// NOTE: Current this field is TypeOfMatch, but it will soon be renmaed int eh API
        /// </summary>
        [JsonConverter( typeof( StringEnumConverter ) )]
        public MatchTypeOptions MatchType { get; set; } = MatchTypeOptions.TRAINING;

        public string MatchName { get; set; } = "";
        
        public string MatchLocation { get; set; } = "";

        public string ScoreFormatted { get; set; } = "";

        public Athena.Score Score { get; set; }
    }
}
