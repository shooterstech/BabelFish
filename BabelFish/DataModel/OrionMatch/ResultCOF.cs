using Scopos.BabelFish.DataActors.OrionMatch;
using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// Result COF format for (JSONVersion) "2022-04-09"
    /// </summary>
    [Serializable]
    public class ResultCOF : IEventScoreProjection {
        //Key is the Singular Event Name, Value is the Shot
        private Dictionary<string, Athena.Shot.Shot> shotsByEventName = null;

        /// <summary>
        /// GUID assigned to this result
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        public string ResultCOFID { get; set; } = string.Empty;

        /// <summary>
        /// The Owner of this data. 
        /// If it starts with "OrionAcct" this it is owned by a club, and the data is considered public.
        /// If it is a GUID, this it is the User ID of the person who owns the data, and is considered protected.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        public string OwnerId { get; set; } = string.Empty;

        /// <summary>
        /// FUTURE, INTERMEDIATE, UNOFFICIAL, OFFICIAL
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 3 )]
        [G_NS.JsonProperty( Order = 3, DefaultValueHandling = G_NS.DefaultValueHandling.Populate )]
        [DefaultValue( ResultStatus.FUTURE )]
        public ResultStatus Status { get; set; } = ResultStatus.FUTURE;

        /// <summary>
        /// The Version string of the JSON document.
        /// Should be "2022-04-09"
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 4 )]
        [G_NS.JsonProperty( Order = 4 )]
        public string JSONVersion { get; set; } = string.Empty;

        /// <summary>
        /// The IoT Topic to monitor to receive live updates to this Result Course of Fire.
        /// Note if the Result COF is completed, no updated will be provided on this topic. 
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 5 )]
        [G_NS.JsonProperty( Order = 5 )]
        public string LiveTopic { get; set; } = string.Empty;

        /// <summary>
        /// The GMT time this ResultCOF was last updated
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 6 )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateTimeConverter ) )]
        [G_NS.JsonProperty( Order = 6 )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateTimeConverter ) )]
        public DateTime LastUpdated { get; set; } = new DateTime();

        /// <summary>
        /// Boolean indicating if this is a partial Result COF that contains only delta (Delta is true),
        /// or a Result COF that has all of the EventScores and Shots (Delta is false).
        /// ResultCOF pulled from the REST API, this value should be false.
        /// ResultCOF pushed from the IoT Topic, this value should be true.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 7 )]
        [G_NS.JsonProperty( Order = 7 )]
        public bool Delta { get; set; }

        /// <summary>
        /// Unique ID for the match.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 8 )]
        [G_NS.JsonProperty( Order = 8 )]
        public string MatchID { get; set; } = string.Empty;

        /// <summary>
        /// Human readable name of the match.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 9 )]
        [G_NS.JsonProperty( Order = 9 )]
        public string MatchName { get; set; } = string.Empty;

        /// <summary>
        /// City, state, and possible country of the location of the match
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 10 )]
        [G_NS.JsonProperty( Order = 10 )]
        public string MatchLocation { get; set; } = "";

        /// <summary>
        /// Unique ID for the parent of this match, if this is a Virtual Match. If this is not a
        /// Virtual Match, then it will be the same value as MatchID.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 11 )]
        [G_NS.JsonProperty( Order = 11 )]
        public string ParentID { get; set; } = string.Empty;


        [G_STJ_SER.JsonPropertyOrder( 12 )]
        [G_NS.JsonProperty( Order = 12 )]

        public MatchTypeOptions MatchType { get; set; } = MatchTypeOptions.TRAINING;

        /// <summary>
        /// The Local Date that this score was shot. 
        /// NOTE Local Date is not necessarily the same as the GMT date.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 13 )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonProperty( Order = 13 )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime LocalDate { get; set; } = DateTime.Today;

        /// <summary>
        /// The Firing Point Label of the current match, this is a string because it could not be a number
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 14 )]
        [G_NS.JsonProperty( Order = 14 )]
        public string FiringPointNumber { get; set; } = "0";

        /// <summary>
        /// String holding the software (Orion Scoring System) and Version number of the software.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 15 )]
        [G_NS.JsonProperty( Order = 15 )]
        public string Creator { get; set; }

        /// <summary>
        /// SetName of the Course Of Fire definition
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 20 )]
        [G_NS.JsonProperty( Order = 20 )]
        public string CourseOfFireDef { get; set; } = string.Empty;

        /// <summary>
        /// SetName of the ScoreConfig used in this match.
        /// NOTE: The name of the ScoreFormatCollection is specified in the Course of Fire 
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 21 )]
        [G_NS.JsonProperty( Order = 21 )]
        public string ScoreConfigName { get; set; }

        /// <summary>
        /// Name of the TargetCollection used in this match.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 22 )]
        [G_NS.JsonProperty( Order = 22 )]
        public string TargetCollectionName { get; set; }

        /// <summary>
        /// The name of the Target definition to use as the default when creating a new Course of Fire. 
        /// Must be a value specified in the TargetCollectionDef.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 23 )]
        [G_NS.JsonProperty( Order = 23 )]
        public string DefaultTargetDefinition { get; set; }


        /// <summary>
        /// The GUID of the orion app user who shot this score. Is blank if not known.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 30 )]
        [G_NS.JsonProperty( Order = 30 )]
        public string UserID { get; set; } = string.Empty;

        /// <summary>
        /// Data on the person or team who shot this score.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 31 )]
        [G_NS.JsonProperty( Order = 31 )]
        public Participant Participant { get; set; } = new Individual();

        /// <summary>
        /// Scores for each composite Event.
        /// The Key of the Dictionary is the Event Name. the Value is the Event Score
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 40 )]
        [G_NS.JsonProperty( Order = 40 )]
        public Dictionary<string, EventScore> EventScores { get; set; } = new Dictionary<string, EventScore>();

        /// <summary>
        /// Scores for each Singular Event (usually a Shot).
        /// The Key is the sequence number, which is represented here as a string, but is really a float. The Value is the Shot object.
        /// To get a dictionary of Shots by their EventName, use GetShotsByEventName()
        /// </summary>
        /// <remarks>Value may be null.</remarks>
        [G_STJ_SER.JsonPropertyOrder( 50 )]
        [G_NS.JsonProperty( Order = 50 )]
        public Dictionary<string, Athena.Shot.Shot> Shots { get; set; } = new Dictionary<string, Athena.Shot.Shot>();

        /// <inheritdoc />
        [G_STJ_SER.JsonPropertyOrder( 51 )]
        [G_NS.JsonProperty( Order = 51 )]
        public Athena.Shot.Shot ? LastShot { get; set; } = null;


        /// <inheritdoc />
        public Dictionary<string, Athena.Shot.Shot> GetShotsByEventName() {
            if (shotsByEventName != null)
                return shotsByEventName;

            shotsByEventName = new Dictionary<string, Athena.Shot.Shot>();

            foreach (var t in Shots.Values)
                if (!string.IsNullOrEmpty( t.EventName ))
                    shotsByEventName.Add( t.EventName, t );

            return shotsByEventName;
        }

        /// <summary>
        /// Describes how to display shot graphics and (text) scores to spectators, during a Live event.
        /// LAE: Changed to ShotGraphicDisplays from Show. was not functioning properly.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 60 )]
        [G_NS.JsonProperty( Order = 60 )]
        public ShotGraphicDisplay LiveDisplay { get; set; } = new ShotGraphicDisplay();

        /// <summary>
        /// Describes how to display shot graphics and (text) scores to spectators, after an event is completed.
        /// </summary>
        /// <remarks> EKA: Currently this field is not used (Apr 2024), as we have no way of populating the values from the range script.
        /// Rezults currently attempts to infer what to display, but can be kludgy depending on the COF.</remarks>
        [G_STJ_SER.JsonPropertyOrder( 61 )]
        [G_NS.JsonProperty( Order = 61 )]
        public List<ShotGraphicDisplay> PostDisplay { get; set; } = new List<ShotGraphicDisplay>();

        /*
         * TODO: Make this an enum
         */
        /// <summary>
        /// The event that caused the publication of this Result COF.
        /// Current known values are an empty string, ShotDetected, and NotSureButWasntAShot
        /// </summary>
        public string GenerativeEvent { get; set; } = "ShotDetected";

        /// <summary>
        /// The Owner of this data. 
        /// If it starts with "OrionAcct" this it is owned by a club, and the data is considered public.
        /// If it is a GUID, this it is the User ID of the person who owns the data, and is considered protected.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 99 )]
        [G_NS.JsonProperty( Order = 99 )]
        [Obsolete( "Use OwnerId instead." )]
        public string Owner {
            get { return this.OwnerId; }
            set { this.OwnerId = value; }
        }

        [G_STJ_SER.JsonPropertyOrder( 99 )]
        [G_NS.JsonProperty( Order = 99 )]
        [Obsolete( "Use OwnerId instead." )]
        public string AccountNumber { get; set; } = string.Empty;

        /// <inheritdoc />
		public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append( "ResultCOF for " );
            foo.Append( Participant.DisplayName );
            foo.Append( ": " );
            foo.Append( MatchName );
            return foo.ToString();
        }

        /// <inheritdoc />
        public Scopos.BabelFish.DataModel.Athena.Shot.Shot? GetLastCompetitionShot()
        {
            Athena.Shot.Shot lastShot = null;

            if (Shots != null) {
                foreach (var shot in Shots) {
                    if (lastShot == null || shot.Value.TimeScored > lastShot.TimeScored) {
                        lastShot = shot.Value;
                        continue;
                    }
                }
            }
            return lastShot;
        }

		/// <inheritdoc />
		public ResultStatus GetStatus() {
            return this.Status;
        }

        /// <inheritdoc />
        public void ProjectScores( ProjectorOfScores ps ) {
            ps.ProjectEventScores( this );
        }

        /// <inheritdoc />
        public List<IEventScoreProjection> GetTeamMembersAsIEventScoreProjection() {
            //Result COF does not have TeamMembers (not yet at least) so returning an empty list
            return new List<IEventScoreProjection>();
        }

        /// <inheritdoc />
        public void SetTeamMembersFromIEventScoreProjection( List<IEventScoreProjection> teamMembers ) {
            //Result COF does not have TeamMembers (not yet at least) so doing nothing
            ;
        }
    }
}