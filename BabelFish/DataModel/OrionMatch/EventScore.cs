using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.OrionMatch
{

    /// <summary>
    /// Describes the status and score for one composite Event within a Course of Fire.
    /// Scores of individual shots are not included (as they are not composite events).
    /// EventScore format for (JSONVersion) "2022-04-09"
    /// </summary>
    [Serializable]
    public class EventScore
    {

        public const string EVENTSTATUS_FUTURE = "FUTURE";
        public const string EVENTSTATUS_INTERMEDIATE = "INTERMEDIATE";
        public const string EVENTSTATUS_UNOFFICIAL = "UNOFFICIAL";
        public const string EVENTSTATUS_OFFICIAL = "OFFICIAL";

        public EventScore()
        {
        }

        /// <summary>
        /// FUTURE
        /// INTERMEDIATE
        /// UNOFFICIAL
        /// OFFICIAL
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// If this Event matches with a defined EventStyle
        /// this is the SetName of that EventStyle
        /// </summary>
        public string EventStyleDef { get; set; } = string.Empty;

        //ScoreFormat is no longer used. Instead format is specified in the Course of Fire Definition
        //public string ScoreFormat { get; set; } = string.Empty;

        public Scopos.BabelFish.DataModel.Athena.Score Score { get; set; } = new Scopos.BabelFish.DataModel.Athena.Score();

        /// <summary>
        /// EVENT
        /// STAGE
        /// SERIES
        /// SHOT
        /// etc
        /// </summary>
        public string EventType { get; set; } = string.Empty;

        /// <summary>
        /// The date and time of the first shot, in this event. 
        /// EKA: Field is not really used, removing it.
        /// </summary>
        //public DateTime EventTime { get; set; } = new DateTime();

        public string EventName { get; set; } = string.Empty;

        //public List<EventScore> Children { get; set; } = new List<EventScore>();

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