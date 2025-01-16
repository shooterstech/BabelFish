using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.AttributeValue;

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
        
        public MatchTypeOptions MatchType { get; set; } = MatchTypeOptions.TRAINING;

        public string MatchName { get; set; } = "";

        /// <summary>
        /// The Match ID that this score was shot in. An empty string means the match ID is 
        /// either not known, or the score was entered manually. 
        /// </summary>
        public string MatchID { get; set; } = "";
        
        public string MatchLocation { get; set; } = "";

        public string ScoreFormatted { get; set; } = "";

        public Athena.Score Score { get; set; }

        
        public VisibilityOption Visibility { get; set; } = VisibilityOption.PRIVATE;
    }
}
