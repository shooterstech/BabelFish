using System.ComponentModel;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// A ResultCOF is considered a 'compiled' data structure. It contains data for a given Course of Fire fired within a Match.
    /// This includes the Event Scores and Shots, as well as metadata about the match and participant who fired the scores..
    /// </summary>
    [Serializable]
    public class ResultCOF :
        IEventScoreProjection,
        ISaveToFile,
        ICheckSum {

        #region Private Variables
        //Key is the Singular Event Name, Value is the Shot
        private Dictionary<string, Athena.Shot.Shot> _shotsByEventName = null;

        #endregion

        #region Constructors, Initialization, and Factory Methods

        #endregion

        #region Events

        #endregion

        #region Data Model Properties
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
        /// The status of this Result COF. It is generally best to call .GetStatus() instead of reading the value from
        /// .Status, as the status may be updated if the last updated time is more than an hour old.
        /// <list type="bullet">
        /// <item>FUTURE</item>
        /// <item>INTERMEDIATE</item>
        /// <item>UNOFFICIAL</item>
        /// <item>OFFICIAL</item>
        /// </list>
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 3 )]
        [G_NS.JsonProperty( Order = 3, DefaultValueHandling = G_NS.DefaultValueHandling.Populate )]
        [DefaultValue( ResultStatus.FUTURE )]
        public ResultStatus Status { get; set; } = ResultStatus.FUTURE;

        /// <summary>
        /// Gets or sets the visibility for this Result COF.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 4 )]
        [G_NS.JsonProperty( Order = 4 )]
        public VisibilityOption Visibility { get; set; } = VisibilityOption.PRIVATE;

        /// <summary>
        /// The IoT Topic to monitor to receive live updates to this Result Course of Fire.
        /// Note if the Result COF is completed, no updated will be provided on this topic. 
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 5 )]
        [G_NS.JsonProperty( Order = 5 )]
        [Obsolete( "Being removed because it was never used in practice. Deprecated April 2026" )]
        public string LiveTopic { get; set; } = string.Empty;

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
        public MatchID MatchID { get; set; } = MatchID.DEFAULT;

        public int CourseOfFireId { get; set; } = 1;

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
        /// Read only unique ID for the parent of this match, if this is a Virtual Match. If this is not a
        /// Virtual Match, then it will be the same value as MatchID.
        /// </summary>
        [G_NS.JsonIgnore]
        public MatchID ParentID {
            get {
                return this.MatchID.GetParentMatchID();
            }
        }


        [G_STJ_SER.JsonPropertyOrder( 12 )]
        [G_NS.JsonProperty( Order = 12 )]

        public CompetitionTypeOptions MatchType { get; set; } = CompetitionTypeOptions.TRAINING;

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
        /// The Firing Point Number that this Result COF was shot on. 
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 14 )]
        [G_NS.JsonProperty( Order = 14 )]
        [Obsolete( "This field is being renamed to SquaddingAssignment, as FiringPointNumber is not an accurate description for all disciplines." )]
        public string FiringPointNumber { get; set; } = "0";

        /// <summary>
        /// The SquaddingAssignment (e.g. firing point number, squad number, etc.) that this Result COF was shot on.
        /// <para>Value is null if it is not known.</para>
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 14 )]
        [G_NS.JsonProperty( Order = 14 )]
        public SquaddingAssignment? SquaddingAssignment { get; set; } = null;

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
        public SetName CourseOfFireDef { get; set; } = new SetName();

        /// <summary>
        /// SetName of the ScoreConfig used in this match.
        /// NOTE: The name of the ScoreFormatCollection is specified in the Course of Fire 
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 21 )]
        [G_NS.JsonProperty( Order = 21 )]
        public string ScoreConfigName { get; set; }

        /// <summary>
        /// Name of the TargetCollection used in firing this Result Course of Fire. The <see cref="TargetCollection">TARGET COLLECTION</see>
        /// is defined within the <see cref="CourseOfFire">COURSE OF FIRE</see>.
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
        [Obsolete( "This field is no longer used. The default target definition should be specified in the Course of Fire definition." )]
        public string DefaultTargetDefinition { get; set; }


        /// <summary>
        /// Read only UUID of the Scopos account user who shot this score. Is blank if not known.
        /// <para>This value is the same as <see cref="Participant.UserID"/>.</para>
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 30 )]
        [G_NS.JsonProperty( Order = 30 )]
        public string UserID {
            get {
                if (Participant is null && Participant is Individual inv)
                    return inv?.UserID ?? string.Empty;

                return string.Empty;
            }
        }

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
        /// The list of <see cref="RemarkAction"/> this Participant has for this Course of Fire. This can include things like DNS, DSQ, or in a Final AT RISK.
        /// </summary>
        /// <remarks>The value of the RemarkList is copied from the <see cref="CourseOfFireEntry.RemarkList"/>.</remarks>
        [G_STJ_SER.JsonPropertyOrder( 45 )]
        [G_NS.JsonProperty( Order = 45 )]
        public RemarkList RemarkList { get; set; }

        /// <summary>
        /// Newtonsoft Conditional Property to only serialize RemarkList when the list has something in it.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeRemarkList() {
            return (RemarkList != null && RemarkList.Count > 0);
        }

        /// <inheritdoc/>
        [G_STJ_SER.JsonPropertyOrder( 46 )]
        [G_NS.JsonProperty( Order = 46 )]
        public bool OutOfCompetition { get; set; }

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
        public Athena.Shot.Shot? LastShot { get; set; } = null;

        /// <inheritdoc />
        [G_STJ_SER.JsonPropertyOrder( 55 )]
        [G_NS.JsonProperty( Order = 55 )]
        public Dictionary<string, EventScore> ResultCofScores { get; set; } = null;

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
         * Question: Should this be an enum?
         */
        /// <summary>
        /// The event that caused the publication of this Result COF.
        /// Current known values are an empty string, ShotDetected, and NotSureButWasntAShot
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 62 )]
        [G_NS.JsonProperty( Order = 62 )]
        public string GenerativeEvent { get; set; } = "ShotDetected";

        /// <summary>
        /// The Version string of the JSON document.
        /// Should be "2022-04-09"
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 90 )]
        [G_NS.JsonProperty( Order = 90 )]
        public string JSONVersion { get; set; } = string.Empty;

        /// <summary>
        /// The GMT time this ResultCOF was last updated
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 91 )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateTimeConverter ) )]
        [G_NS.JsonProperty( Order = 91 )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateTimeConverter ) )]
        public DateTime LastUpdated { get; set; } = DateTime.MinValue;

        /// <summary>
        /// The Owner of this data. 
        /// If it starts with "OrionAcct" this it is owned by a club, and the data is considered public.
        /// If it is a GUID, this it is the User ID of the person who owns the data, and is considered protected.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 98 )]
        [G_NS.JsonProperty( Order = 98 )]
        [Obsolete( "Use OwnerId instead." )]
        public string Owner {
            get { return this.OwnerId; }
            set { this.OwnerId = value; }
        }

        [G_STJ_SER.JsonPropertyOrder( 99 )]
        [G_NS.JsonProperty( Order = 99 )]
        [Obsolete( "Use OwnerId instead." )]
        public string AccountNumber { get; set; } = string.Empty;

        #endregion

        #region Helper Properties

        #endregion

        #region Methods
        /// <inheritdoc />
        public Dictionary<string, Athena.Shot.Shot> GetShotsByEventName() {
            if (_shotsByEventName != null)
                return _shotsByEventName;

            _shotsByEventName = new Dictionary<string, Athena.Shot.Shot>();

            foreach (var t in Shots.Values)
                if (!string.IsNullOrEmpty( t.EventName ))
                    _shotsByEventName.Add( t.EventName, t );

            return _shotsByEventName;
        }

        /// <summary>
        /// Newtownsoft helper method to determine if ResultCofScores should be serialized. We only want to serialize it if it is not null, and if it has at least one score in it.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeResultCofScores() {
            return this.ResultCofScores is not null
                && this.ResultCofScores.Count > 0;
        }

        /// <summary>
        /// Newtonsoft.json helper method to determine if SquaddingAssignment should be serialized.
        /// We only want to serialize it if it is not null, and if it is not "Not Yet Squadded", which is the default value when we don't know the squadding assignment.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeSquaddingAssignment() {
            return SquaddingAssignment is not null && !SquaddingAssignment.NotYetSquadded;
        }

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
        public Scopos.BabelFish.DataModel.Athena.Shot.Shot? GetLastCompetitionShot() {
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

        /// <inheritdoc />
        public bool CurrentlyCompetingOrRecentlyDone() {
            if (GetStatus() == ResultStatus.INTERMEDIATE)
                return true;

            if (LastShot != null && ((DateTime.UtcNow - LastShot.TimeScored.ToUniversalTime()).TotalMinutes < 5.0))
                return true;

            return false;
        }

        #region ISaveToFile Methods
        /// <inheritdoc />
        public string GetFileName() {
            return $"{ResultCOFID}.json";
        }

        /// <inheritdoc />
        public string SaveToFile( DirectoryInfo relativeDirectory ) {

            if (relativeDirectory == null)
                throw new ArgumentNullException( nameof( relativeDirectory ) );

            string filePath = Path.Combine( relativeDirectory.FullName, GetRelativePath() );

            var directoryPath = Path.GetDirectoryName( filePath );

            if (!Directory.Exists( directoryPath )) {
                Directory.CreateDirectory( directoryPath );
            }

            string json = SerializeToJson();

            File.WriteAllText( filePath, json );

            return filePath;
        }

        /// <inheritdoc />
        public string SaveToFile( FileInfo fileInfo ) {

            if (fileInfo == null)
                throw new ArgumentNullException( nameof( fileInfo ) );

            string json = SerializeToJson();

            File.WriteAllText( fileInfo.FullName, json );

            return fileInfo.FullName;
        }

        /// <inheritdoc />
        public string GetRelativePath() {
            return this.GetFileName();
        }

        /// <inheritdoc />
        public string SerializeToJson() {
            string json = G_NS.JsonConvert.SerializeObject( this, Helpers.SerializerOptions.NewtonsoftJsonSerializer );

            return json;
        }
        #endregion

        #endregion

        /// <inheritdoc />
        public string CheckSum { get; set; }

        /// <inheritdoc />
        public ulong CalculateChecksum() {
            var combined = $"{ResultCOFID}|{OwnerId}|{Status}|{Visibility}|{MatchID}|{MatchName}|{MatchLocation}|{ParentID}|{MatchType}|{LocalDate.ToString( DateTimeFormats.DATE_FORMAT )}|{FiringPointNumber}|{Creator}|{CourseOfFireDef}|{ScoreConfigName}|{TargetCollectionName}|{DefaultTargetDefinition}|{UserID}";
            var hash = Helpers.Common.Md5ToUlong( combined );

            hash ^= Participant.CalculateChecksum();

            if (EventScores is not null) {
                foreach (var es in EventScores) {
                    hash ^= es.Value.CalculateChecksum();
                }
            }

            if (Shots is not null) {
                foreach (var es in Shots) {
                    hash ^= es.Value.CalculateChecksum();
                }
            }

            if (ResultCofScores is not null) {
                foreach (var rCof in ResultCofScores) {
                    hash ^= rCof.Value.CalculateChecksum();
                }
            }

            return hash;
        }

    }
}
