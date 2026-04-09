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

        #region Private Variables

        #endregion

        #region Constructors, Factory Methods, Initialization
        public EventScore() {
        }

        #endregion

        #region Event Handlers

        #endregion

        #region Data Model Properties

        /// <summary>
        /// The name of the Event that this EventScore is associated with. This should match the name of an Event defined in the <see cref="CourseOfFire">COURSE OF FIRE</see>.
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        public string EventName { get; set; } = string.Empty;

        /// <summary>
        /// ScoreFormatted may only be set when the Shot is part of a Result COF .Events dictrionary
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        public string ScoreFormatted { get; set; }

        /// <summary>
        /// The actual score the Participant has shot.
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        public Athena.Score Score { get; set; } = new Athena.Score();

        /// <summary>
        /// The projected / predicted score the Participant is expected to finish with.
        /// </summary>
        [G_NS.JsonProperty( Order = 4 )]
        public Athena.Score Projected { get; set; } = new Athena.Score();

        /// <summary>
        /// The type of Event that this EventScore is associated with. This should match the type of an Event defined in the <see cref="CourseOfFire">COURSE OF FIRE</see>.
        /// </summary>
        [G_NS.JsonProperty( Order = 10, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public EventtType EventType { get; set; } = EventtType.NONE;

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
        [G_NS.JsonProperty( Order = 11, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        [DefaultValue( ResultStatus.FUTURE )]
        public ResultStatus Status { get; set; } = ResultStatus.FUTURE;

        /// <summary>
        /// If this Event matches with a defined EventStyle
        /// this is the SetName of that EventStyle
        /// </summary>
        [G_NS.JsonProperty( Order = 12 )]
        public SetName EventStyleDef { get; set; } = SetName.DEFAULT;

        /// <summary>
        /// If this Event matches with a defined StageStyle
        /// this is the SetName of that StageStyle
        /// </summary>
        [G_NS.JsonProperty( Order = 13 )]
        public SetName StageStyleDef { get; set; } = SetName.DEFAULT;

        /// <summary>
        /// The number of shots the athletes has fired in this Event.
        /// NOTE that this is different from the number of shots in the event.
        /// </summary>
        [G_NS.JsonProperty( Order = 15 )]
        public int NumShotsFired { get; set; } = 0;

        #endregion

        #region Helper Properties

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

        #endregion

        #region Public Methods

        #endregion

        #region Protected and Private Methods
        public bool ShouldSerializeProjected() {
            return this.Projected != null && !this.Projected.IsZero;
        }

        public bool ShouldSerializeEventStyleDef() {
            return !this.EventStyleDef.IsDefault;
        }

        public bool ShouldSerializeStageStyleDef() {
            return !this.StageStyleDef.IsDefault;
        }
        #endregion
    }
}
