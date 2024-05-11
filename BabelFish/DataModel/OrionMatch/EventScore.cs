using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Describes the status and score for one composite Event within a Course of Fire.
    /// Scores of individual shots are not included (as they are not composite events).
    /// EventScore format for (JSONVersion) "2022-04-09"
    /// </summary>
    [Serializable]
    public class EventScore {

        public EventScore() {
        }

        /// <summary>
        /// FUTURE
        /// INTERMEDIATE
        /// UNOFFICIAL
        /// OFFICIAL
        /// </summary>
        [JsonConverter( typeof( StringEnumConverter ) )]
        public ResultStatus Status { get; set; } = ResultStatus.INTERMEDIATE;

        /// <summary>
        /// If this Event matches with a defined EventStyle
        /// this is the SetName of that EventStyle
        /// </summary>
        public string EventStyleDef { get; set; } = string.Empty;

        public Athena.Score Score { get; set; } = new Athena.Score();

        [DefaultValue( null )]
        public Athena.Score Projected { get; set; } = null;

        /// <summary>
        /// EVENT
        /// STAGE
        /// SERIES
        /// SHOT
        /// etc
        /// </summary>
        public string EventType { get; set; } = string.Empty;

        public string EventName { get; set; } = string.Empty;

        /// <summary>
        /// The number of shots the athletes has fired in this Event.
        /// NOTE that this is different from the number of shots in the event.
        /// </summary>
        public int NumShotsFired { get; set; } = 0;

        /// <summary>
        /// If this Event matches with a defined StageStyle
        /// this is the SetName of that StageStyle
        /// </summary>
        public string StageStyleDef { get; set; } = string.Empty;

        /// <summary>
        /// ScoreFormatted may only be set when the Shot is part of a Result COF .Events dictrionary
        /// </summary>
        public string ScoreFormatted { get; set; }
    }
}