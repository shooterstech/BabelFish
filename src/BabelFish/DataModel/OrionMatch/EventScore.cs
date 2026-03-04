using System.ComponentModel;
using Scopos.BabelFish.DataModel.Definitions;


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
        /// The status of this Result COF. It is generally best to call .GetStatus() instead of reading the value from
        /// .Status, as the status may be updated if the last updated time is more than an hour old.
        /// <list type="bullet">
        /// <item>FUTURE</item>
        /// <item>INTERMEDIATE</item>
        /// <item>UNOFFICIAL</item>
        /// <item>OFFICIAL</item>
        /// </list>
        /// </summary>
        [G_NS.JsonConverter( typeof( G_NS_CONV.StringEnumConverter ) )]
        [G_NS.JsonProperty( DefaultValueHandling = G_NS.DefaultValueHandling.Populate )]
        [DefaultValue( ResultStatus.FUTURE )]
        public ResultStatus Status { get; set; } = ResultStatus.FUTURE;

        /// <summary>
        /// If this Event matches with a defined EventStyle
        /// this is the SetName of that EventStyle
        /// </summary>
        [DefaultValue( "" )]
        public SetName EventStyleDef { get; set; } = SetName.DEFAULT;

        /// <summary>
        /// The actual score the Participant has shot.
        /// </summary>
        public Athena.Score Score { get; set; } = new Athena.Score();

        /// <summary>
        /// The projected / predicted score the Participant is expected to finish with.
        /// </summary>
        [DefaultValue( null )]
        public Athena.Score Projected { get; set; } = null;

        /// <summary>
        /// EVENT
        /// STAGE
        /// SERIES
        /// SHOT
        /// etc
        /// </summary>
        [DefaultValue( "" )]
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
        [DefaultValue( "" )]
        public SetName StageStyleDef { get; set; } = SetName.DEFAULT;

        /// <summary>
        /// ScoreFormatted may only be set when the Shot is part of a Result COF .Events dictrionary
        /// </summary>
        public string ScoreFormatted { get; set; }

        /// <summary>
        /// A Temporary field that's needed by the TournamentMerger
        /// </summary>
        [G_NS.JsonIgnore]
        public MatchID MatchId { get; set; } = MatchID.DEFAULT;

        /// <summary>
        /// A Temporary field that's needed by the TournamentMerger
        /// </summary>
        [G_NS.JsonIgnore]
        public Participant? Participant { get; set; } = null;
    }
}
