using System.ComponentModel;
using Scopos.BabelFish.DataActors.OrionMatch;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// A ResultEvent represents the score one participant earned in an Event. ResultEvents often contain the score
    /// not just for the top level Event, but for the children as well.
    /// </summary>
    public class ResultEvent :
        IEventScoreProjection,
        IRLIFItem,
        ICheckSum {

        #region Private Fields
        //Key is the Singular Event Name, Value is the Shot
        private Dictionary<string, Athena.Shot.Shot> _shotsByEventName = null;

        //Cached copy of the name of the top level event.
        private string _topLevelEventName = "";
        #endregion

        #region Constructors, Factory Methods, and Initialization
        public ResultEvent() {
            //Purposefully set TeamMemebers to null so if it is an individual the attribute doesn't get added into the JSON
            TeamMembers = null;
        }
        #endregion

        #region Data Model Properties
        /// <summary>
        /// Data on the person or team who shot this score.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 10 )]
        [G_NS.JsonProperty( Order = 10 )]
        public Participant Participant { get; set; } = new Individual();

        /// <summary>
        /// The local Match ID that generated this ResultEvent.
        /// Information on that match may be looked up in the ResultList's Metadata field.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        public MatchID MatchID { get; set; } = MatchID.DEFAULT;

        [G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        public string ResultCOFID { get; set; }

        /// <summary>
        /// The absolute ranking of this competitor, using actual (and not projected) scores fired.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 3 )]
        [G_NS.JsonProperty( Order = 3 )]
        public int Rank { get; set; }

        /// <summary>
        /// RankOrder is very nearly the same as Rank. The difference is if there is an unbreakable tie. In an
        /// unbreakable tie the two partjicipants are given the same Rank but different RankOrder.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 4 )]
        [G_NS.JsonProperty( Order = 4 )]
        public int RankOrder { get; set; }

        /// <summary>
        /// The change in this participant's result list ranking. A positive value means the participant
        /// has moved up in the ranking. A negative value means the participant has moved down.
        /// <para>A change in the ranking can only happen when the participant is shooting (INTERMEDIATE Result Status), so 
        /// there is no difference between an absolute and projected ranking change. They are all projected. </para>
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 5 )]
        [G_NS.JsonProperty( Order = 5 )]
        [DefaultValue( 0 )]
        public int RankDelta { get; set; } = 0;

        /// <summary>
        /// For internal use only, to learn which ResultEvents to apply Command Automation to
        /// </summary>
        [G_NS.JsonIgnore]
        public int BottomRank { get; set; }

        /// <summary>
        /// The projected rank of this competitor, using projected scores.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 6 )]
        [G_NS.JsonProperty( Order = 6 )]
        [DefaultValue( 0 )]
        public int ProjectedRank { get; set; } = 0;


        /// <summary>
		/// ProjectedRankOrder is very nearly the same as ProjectedRank. The difference is if there is an unbreakable tie. In an
		/// unbreakable tie the two partjicipants are given the same ProjectedRank but different ProjectedRankOrder.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 7 )]
        [G_NS.JsonProperty( Order = 7 )]
        [DefaultValue( 0 )]
        public int ProjectedRankOrder { get; set; } = 0;


        /// <summary>
        /// The Local Date that this score was shot. 
        /// NOTE Local Date is not necessarily the same as the GMT date.
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        [G_STJ_SER.JsonPropertyOrder( 9 )]
        [G_NS.JsonProperty( Order = 9 )]
        public DateTime LocalDate { get; set; } = DateTime.Today;

        [G_STJ_SER.JsonPropertyOrder( 11 )]
        [G_NS.JsonProperty( Order = 11 )]
        public Dictionary<string, EventScore> EventScores { get; set; }


        [G_STJ_SER.JsonPropertyOrder( 12 )]
        [G_NS.JsonProperty( Order = 12 )]
        public Dictionary<string, EventScore> ResultCofScores { get; set; }

        /// <summary>
        /// Scores for each Singular Event (usually a Shot).
        /// The Key is the sequence number, which is represented here as a string, but is really a float. The Value is the Shot object.
        /// To get a dictionary of Shots by their EventName, use GetShotsByEventName()
        /// In the Result Event object, which is part of a Resuslt List, the Shots dictionary is purposefully not included
        /// to conserve length of data. It is included in ResultEvents because of the IEventScoreProjection interface.
        /// </summary>
        [G_STJ_SER.JsonIgnore]
        [G_NS.JsonIgnore]
        [DefaultValue( null )]
        public Dictionary<string, Athena.Shot.Shot> Shots { get; set; } = new Dictionary<string, Athena.Shot.Shot>();

        /// <inheritdoc />
        [G_STJ_SER.JsonPropertyOrder( 15 )]
        [G_NS.JsonProperty( Order = 15 )]
        public Athena.Shot.Shot? LastShot { get; set; } = null;

        /// <inheritdoc />
        [G_STJ_SER.JsonPropertyOrder( 15 )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateTimeConverter ) )]
        [G_NS.JsonProperty( Order = 15 )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateTimeConverter ) )]
        //EKA NOTE Nov 2025: Choosing to use .UtcNow instead of .MinValue. The idea is, if LastUpdated is not a part of the REST API
        //returned json (as would be the case for Orion 2.23 or before)
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// If this is a team score, the TeamMembers will be the scores of the team members. If this is an Individual value will be null.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 21 )]
        [G_NS.JsonProperty( Order = 21 )]
        public List<ResultEvent>? TeamMembers { get; set; }


        /// <summary>
        /// The list of <see cref="RemarkAction"/> this Participant has for this Course of Fire. This can include things like DNS, DSQ, or in a Final AT RISK.
        /// </summary>
        /// <remarks>The value of the RemarkList is copied from the <see cref="CourseOfFireEntry.RemarkList"/>.</remarks>
        [G_STJ_SER.JsonPropertyOrder( 25 )]
        [G_NS.JsonProperty( Order = 25 )]
        public RemarkList RemarkList { get; set; } = new RemarkList();

        #endregion

        #region Helper Properties

        /// <inheritdoc />
        /// <remarks>Squadding Assignmetn is not a part of the REST API response for GetResultList. It is included here to allow the Result
        /// List Formatter access to squadding information, so it may display it on a formatted result list.</remarks>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public SquaddingAssignment SquaddingAssignment { get; set; }

        /// <summary>
        /// Helper property to easily check if this participant is shooting out of competition (for score only). This is determined by checking if the RemarkList contains a ParticipantRemark of OUT_OF_COMPETITION.
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public bool OutOfCompetition {
            get {
                return RemarkList.IsShowingParticipantRemark( ParticipantRemark.OUT_OF_COMPETITION );
            }
        }

        /// <inheritdoc />
        /// <remarks>Choosing not to include CheckSum in the serialized value, as it is not a top level document.</remarks>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public string CheckSum { get; set; } = string.Empty;

        #endregion

        #region Methods

        /// <inheritdoc />
        public List<IEventScoreProjection> GetTeamMembersAsIEventScoreProjection() {
            if (TeamMembers == null) {
                return new List<IEventScoreProjection>();
            }

            return TeamMembers.ToList<IEventScoreProjection>();
        }

        /// <inheritdoc />
        public void SetTeamMembersFromIEventScoreProjection( List<IEventScoreProjection> teamMembers ) {

            if (TeamMembers == null)
                TeamMembers = new List<ResultEvent>();

            TeamMembers.Clear();

            foreach (var tm in teamMembers) {
                TeamMembers.Add( (ResultEvent)tm );
            }
        }

        /// <inheritdoc />
        public void ProjectScores( ProjectorOfScores ps ) {
            ps.ProjectEventScores( this );
        }

        public bool ShouldSerializeResultCOFScores() {
            return (ResultCofScores != null && ResultCofScores.Count > 0);
        }

        /// <summary>
        /// The idea of this name scheme for the key is to use matchId as a namespace. Since each event name within a
        /// match has to be unique, and each matchId is unique, then the key too will be unique.
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static string KeyForResultCofScore( string matchId, string eventName ) {
            return $"{matchId}: {eventName}";
        }

        /// <summary>
        /// The idea of this name scheme for the key is to use matchId as a namespace. Since each event name within a
        /// match has to be unique, and each matchId is unique, then the key too will be unique.
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static string KeyForResultCofScore( MatchID matchId, string eventName ) {
            return $"{matchId}: {eventName}";
        }

        /// <inheritdoc />
		public ResultStatus GetStatus() {
            if (string.IsNullOrEmpty( _topLevelEventName )) {
                if (this.EventScores is not null) {
                    foreach (var es in this.EventScores.Values) {
                        if (es.EventType == Definitions.EventtType.EVENT) {
                            _topLevelEventName = es.EventName;
                            return es.Status;
                        }
                    }
                }

                //EKA NOTE: Should we check .EventScores.ResultCofScores ? 

            } else if (this.EventScores.TryGetValue( _topLevelEventName, out EventScore es )) {
                return es.Status;
            }

            return ResultStatus.OFFICIAL;
        }

        /// <summary>
        /// A Newtonsoft Conditional Property to only serialize TeamMembers when the list has something in it.
        /// https://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeTeamMembers() {
            return (TeamMembers != null && TeamMembers.Count > 0);
        }

        /// <summary>
        /// Newtonsoft Conditional Property to only serialize RemarkList when the list has something in it.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeRemarkList() {
            return (RemarkList != null && RemarkList.Count > 0);
        }

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

        /// <inheritdoc />
        public Athena.Shot.Shot? GetLastCompetitionShot() {
            Athena.Shot.Shot lastShot = null;

            if (Shots != null) {
                foreach (var shot in Shots) {
                    if (shot.Value.TimeScored > lastShot.TimeScored) {
                        lastShot = shot.Value;
                    }
                }
            }
            return lastShot;
        }

        /// <inheritdoc />
        public bool CurrentlyCompetingOrRecentlyDone() {
            if (GetStatus() == ResultStatus.INTERMEDIATE)
                return true;

            if (LastShot != null && ((DateTime.UtcNow - LastShot.TimeScored.ToUniversalTime()).TotalMinutes < 5.0))
                return true;

            return false;
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"ResultEvent for {this.Participant.DisplayName}";
        }


        /// <inheritdoc />
        public ulong CalculateChecksum() {
            var combine = $"{MatchID}|{Rank}|{RankOrder}|{RankDelta}|{ProjectedRank}|{ProjectedRankOrder}|{LocalDate.ToString( DateTimeFormats.DATE_FORMAT )}";
            var hash = Helpers.Common.Md5ToUlong( combine );

            hash ^= Participant.CalculateChecksum();

            if (EventScores is not null) {
                foreach (var es in EventScores) {
                    hash ^= es.Value.CalculateChecksum();
                }
            }

            if (ResultCofScores is not null) {
                foreach (var rCof in ResultCofScores) {
                    hash ^= rCof.Value.CalculateChecksum();
                }
            }

            if (TeamMembers is not null) {
                foreach (var tm in TeamMembers) {
                    hash ^= tm.CalculateChecksum();
                }
            }

            // NOTE: We are purposefully not including Shots in the checksum calculation, as this property is not included in the REST API response for ResultEvents.

            return hash;
        }

        #endregion
    }
}
