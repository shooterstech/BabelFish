using System.ComponentModel;
using Scopos.BabelFish.DataActors.ResultListMerger;
using Scopos.BabelFish.DataModel.Definitions;


namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Describes the status and score for one composite Event within a Course of Fire.
    /// Scores of individual shots are not included (as they are not composite events).
    /// EventScore format for (JSONVersion) "2022-04-09"
    /// </summary>
    [Serializable]
    public class EventScore : ICheckSum {

        #region Private Variables

        #endregion

        #region Constructors, Factory Methods, Initialization
        /// <summary>
        /// Public constructor.
        /// </summary>
        public EventScore() {
        }

        /// <summary>
        /// Copy constructor. Creates a new instance of EventScore by copying all data model properties from an existing instance.
        /// </summary>
        /// <param name="other">The EventScore instance to copy from.</param>
        public EventScore( EventScore other ) {
            // Data Model Properties
            this.EventName = other.EventName;
            this.ScoreFormatted = other.ScoreFormatted;
            this.Score = new Athena.Score( other.Score );
            this.Projected = new Athena.Score( other.Projected );
            this.EventType = other.EventType;
            this.Status = other.Status;
            this.EventStyleDef = other.EventStyleDef;
            this.StageStyleDef = other.StageStyleDef;
            this.NumShotsFired = other.NumShotsFired;

            // Helper Properties
            this.MatchId = other.MatchId;
            this.Participant = other.Participant;
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
        public SetName EventStyleDef { get; set; } = new SetName();

        /// <summary>
        /// If this Event matches with a defined StageStyle
        /// this is the SetName of that StageStyle
        /// </summary>
        [G_NS.JsonProperty( Order = 13 )]
        public SetName StageStyleDef { get; set; } = new SetName();

        /// <summary>
        /// The number of shots the athletes has fired in this Event.
        /// NOTE that this is different from the number of shots in the event.
        /// </summary>
        [G_NS.JsonProperty( Order = 15 )]
        public int NumShotsFired { get; set; } = 0;

        #endregion

        #region Helper Properties

        /// <summary>
        /// A Temporary field that's needed by the <see cref="ResultListMergerEngine"/>.
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public MatchID MatchId { get; set; } = MatchID.DEFAULT;

        /// <summary>
        /// A Temporary field that's needed by the <see cref="ResultListMergerEngine"/>.
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public Participant? Participant { get; set; } = null;

        /// <inheritdoc />
        /// <remarks>Choosing not to include CheckSum in the serialized value, as it is not a top level document.</remarks>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public string CheckSum { get; set; }

        #endregion

        #region Public Methods
        /// <inheritdoc />
        public ulong CalculateChecksum() {
            var combined = $"{Status}|{EventType}|{EventName}|{NumShotsFired}";
            var hash = Helpers.Common.Md5ToUlong( combined );

            if (Score is not null)
                hash ^= Score.CalculateChecksum();

            if (Projected is not null)
                hash ^= Projected.CalculateChecksum();

            return hash;
        }

        /// <summary>
        /// Returns a string that represents the current object, including the event name and the formatted score.
        /// </summary>
        public override string ToString() {
            return $"{this.EventName}: {this.ScoreFormatted}";
        }
        #endregion

        #region Protected and Private Methods
        /// <summary>
        /// Newtonsoft.json helper method to determine whether the Projected property should be serialized. We only want to
        /// serialize it if it's not null and not zero, as otherwise it doesn't add any information and just takes up space in the JSON.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeProjected() {
            return this.Projected != null && !this.Projected.IsZero;
        }

        /// <summary>
        /// Newtonsoft.json helper method to determine whether the EventStyleDef property should be serialized. We only want to
        /// serialize it if it's not the default value, as otherwise it doesn't add any information and just takes up space in the JSON.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeEventStyleDef() {
            return !this.EventStyleDef.IsDefault;
        }

        /// <summary>
        /// Newtonsoft.json helper method to determine whether the StageStyleDef property should be serialized. We only want to
        /// serialize it if it's not the default value, as otherwise it doesn't add any information and just takes up space in the JSON.
        /// </summary>
        public bool ShouldSerializeStageStyleDef() {
            return !this.StageStyleDef.IsDefault;
        }

        #endregion
    }
}
