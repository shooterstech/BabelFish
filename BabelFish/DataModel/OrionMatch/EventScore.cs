using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.OrionMatch
{

    /// <summary>
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

        public ShootersTech.DataModel.Athena.Score Score { get; set; } = new ShootersTech.DataModel.Athena.Score();

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
        /// Set Name of the target definition
        /// EKA: TargetDef no longer used. Is specified in Course of Fire Def
        /// </summary>
        //public string? TargetDef { get; set; }

        //public Ammunition? Ammunition { get; set; }

        //public ScoreAverage? Average { get; set; }

        //public Coordinate Coordinate {get; set;} = new Coordinate();
    }
}