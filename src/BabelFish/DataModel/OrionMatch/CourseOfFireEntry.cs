using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Abstract class representing the Entry and Squadding information for a single Course of Fire for a Match Participant.
    /// The concrete classes are <see cref="CourseOfFireEntryIndividual"/> and <see cref="CourseOfFireEntryTeam"/>, which represent
    /// the specific information for an Individual or a Team entry, respectively.
    /// <para>Withiin a <see cref="Match"/> there are multiple <see cref="CourseOfFireStructure">Courses of Fire</see>, and each Participant may be entered in some or all of the Course of Fire.
    /// This class represents the information about a single Course of Fire for a single Participant, including whether they are entered
    /// in the Course of Fire, and if so, their Squadding information for that Course of Fire.</para>
    /// <para> The preferred method for creating a new entry is to use the <see cref="MatchParticipant.CreateEntry(int)"/> method.
    /// Which will also set the backwards pointer <see cref="MatchParticipant"/></para>
    /// </summary>
    public abstract class CourseOfFireEntry :
        G_STJ_SER.IJsonOnDeserialized,
        G_STJ_SER.IJsonOnDeserializing {

        #region Private and Protected Fields
        protected Logger _logger = LogManager.GetCurrentClassLogger();
        protected bool _ignoreEvents = false;
        #endregion

        #region Constructors, Facory Methods, and Initialization Methods
        /// <summary>
        /// This method is called after deserialization with System.Text.Json and will disable event firing during deserialization.
        /// </summary>
        public void OnDeserialized() {
            _ignoreEvents = false;
        }

        /// <summary>
        /// Method is called before deserialization with System.Text.Json and will re-enable event firing after deserialization is complete.
        /// </summary>
        public void OnDeserializing() {
            _ignoreEvents = true;
        }
        #endregion

        #region Events
        public event EventHandler<EventArgs<Team>> OnTeamJoined;
        public event EventHandler<EventArgs<Team>> OnTeamLeft;
        #endregion

        #region Data Model Properties
        [G_NS.JsonProperty( Order = 1, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public int CourseOfFireId { get; set; } = 0;

        /// <summary>
        /// Concrete class identifying the type of Participant.
        /// This is used during deserialization to determine which concrete class to deserialize to, either a <see cref="CourseOfFireEntryIndividual"/> or a <see cref="CourseOfFireEntryTeam"/>.
        /// </summary>
        [G_NS.JsonProperty( Order = 3, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public ParticipantType ParticipantType { get; set; } = ParticipantType.INDIVIDUAL;

        /// <summary>
        /// The entry status for this Course of Fire. Indicates whether the Participant is entered, not entered, or withdrew from the Course of Fire.
        /// </summary>
        [G_NS.JsonProperty( Order = 5, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public EntryStatus EntryStatus { get; set; } = EntryStatus.NOT_ENTERED;

        /// <summary>
        /// The list of <see cref="RemarkAction"/> this Participant has for this Course of Fire. This can include things like DNS, DSQ, or in a Final AT RISK.
        /// </summary>
        [G_NS.JsonProperty( Order = 15 )]
        public RemarkList RemarkList { get; set; } = new RemarkList();

        /// <summary>
        /// Gets or sets a boolean indicating whether the Participant is shooting out of competition for this Course of Fire (aka shooting for score only).
        /// Their scores will be listed, but not ranked. 
        /// </summary>
        [DefaultValue( false )]
        public bool OutOfCompetition { get; set; } = false;

        /// <summary>
        /// This property is considered the source of truth for Team Membership.
        /// <para>A null value means the participant is not currently assigned to any team.</para>
        /// </summary>
        public Team Team { get; set; }

        public string TeamParticipantID {
            get; set;
        }
        #endregion

        #region Helper Properties

        /// <summary>
        /// Backwards pointer to the MatchParticipant that owns this CourseOfFireEntry. This is set when the CourseOfFireEntry is created using the <see cref="MatchParticipant.CreateEntry(int)"/> method, and should not be set manually.
        /// </summary>
        [G_NS.JsonIgnore]
        public MatchParticipant? MatchParticipant { get; internal set; } = null;

        /// <summary>
        /// Readonly helper property to return the CourseOfFireStructure for this CourseOfFireEntry.
        /// <para>Returns null if the MatchParticipant, Project, or Match is null, or if the CourseOfFireStructure cannot be found.</para>
        /// </summary>
        [G_NS.JsonIgnore]
        public CourseOfFireStructure? CourseOfFireStructure {
            get {
                if (MatchParticipant is null
                    || MatchParticipant.Project is null
                    || MatchParticipant.Project.Match is null) {
                    return null;
                }
                var match = MatchParticipant.Project.Match;
                if (match.MatchStructure.TryGetCourseOfFireStructure( CourseOfFireId, out CourseOfFireStructure courseOfFireStructure )) {
                    return courseOfFireStructure;
                }

                return null;
            }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Adds the current participant to the specified team for the same Course of Fire, enforcing team size constraints.
        /// </summary>
        /// <remarks>This method requires this instance is properly associated with a <see cref="MatchProject"/></remarks>
        /// <param name="team">The team to join. </param>
        /// <exception cref="ArgumentNullException">Thrown if the team parameter is null.</exception>
        /// <exception cref="BackwardsPointerException">Thrown if the team or the current entry is not properly associated with a <see cref="MatchProject"/>,</exception>
        /// <exception cref="TeamFullException">Thrown if the team already has the maximum number of members allowed for this Course of Fire.</exception>
        public void JoinTeam( Team team ) {
            if (team is null) {
                throw new ArgumentNullException( nameof( team ) );
            }

            if (team.MatchParticipant is null) {
                var msg = "Team does not have a backwards pointer to a MatchParticipant. This likely means either an error, or the Team was deserialized outside a MatchProject.";
                throw new BackwardsPointerException( msg );
            }

            var courseOfFireStructure = this.CourseOfFireStructure;
            if (courseOfFireStructure is null) {
                var msg = "CourseOfFireStructure is null. This likely means either an error, or the CourseOfFireEntry was deserialized outside a MatchProject.";
                throw new BackwardsPointerException( msg );
            }

            if (this.MatchParticipant is null) {
                var msg = "MatchParticipant is null. This likely means either an error, or the CourseOfFireEntry was deserialized outside a MatchProject.";
                throw new BackwardsPointerException( msg );
            }

            if (this.MatchParticipant.Participant is null) {
                var msg = "Participant is null. This likely means either an error, or the CourseOfFireEntry was deserialized outside a MatchProject.";
                throw new BackwardsPointerException( msg );
            }

            //Leave any team the participant is currently on for this Course of Fire before joining the new team.
            LeaveTeam();

            // Find the CourseOfFireEntry for the passed in Team, that has the same CourseOfFireId as this entry.
            // GetEntryByCourseOfFireId will create a new Entry if one does not exist.
            var teamEntry = (CourseOfFireEntryTeam)team.MatchParticipant.GetEntryByCourseOfFireId( CourseOfFireId );
            if (teamEntry.TeamMembers.Count >= courseOfFireStructure.MaxNumberOfTeamMembers) {
                var msg = $"Team {team.TeamName} already has the maximum number of members for this Course of Fire. Max Team Size is {courseOfFireStructure.MaxNumberOfTeamMembers}.";
                throw new TeamFullException( msg );
            }

            teamEntry.TeamMembers.Add( this.MatchParticipant.Participant );
            this.Team = team;
            this.TeamParticipantID = Team.MatchParticipant.ParticipantID;

            if (!_ignoreEvents) {
                OnTeamJoined?.Invoke( this, new EventArgs<Team>( team ) );
            }
        }

        /// <summary>
        /// Removes the <see cref="Participant"/>, represented in this Entry from the team they are on for the same Course of Fire, if any.
        /// If the participant is not currently on a team for this Course of Fire, this method does nothing.
        /// </summary>
        /// <exception cref="BackwardsPointerException"></exception>
        public void LeaveTeam() {

            // If Team is null, then it means the participant is not currently on a team for this Course of Fire, so we can just return.
            if (this.Team is null)
                return;

            var courseOfFireStructure = this.CourseOfFireStructure;
            if (courseOfFireStructure is null) {
                var msg = "CourseOfFireStructure is null. This likely means either an error, or the CourseOfFireEntry was deserialized outside a MatchProject.";
                throw new BackwardsPointerException( msg );
            }

            if (Team.MatchParticipant is null) {
                var msg = "Team.MatchParticipant is null. This likely means either an error, or the CourseOfFireEntry was deserialized outside a MatchProject.";
                throw new BackwardsPointerException( msg );
            }

            if (Team.MatchParticipant.Participant is null) {
                var msg = "Team.MatchParticipant.Participant is null. This likely means either an error, or the CourseOfFireEntry was deserialized outside a MatchProject.";
                throw new BackwardsPointerException( msg );
            }

            if (this.MatchParticipant is null) {
                var msg = "MatchParticipant is null. This likely means either an error, or the CourseOfFireEntry was deserialized outside a MatchProject.";
                throw new BackwardsPointerException( msg );
            }

            if (this.MatchParticipant.Participant is null) {
                var msg = "Participant is null. This likely means either an error, or the CourseOfFireEntry was deserialized outside a MatchProject.";
                throw new BackwardsPointerException( msg );
            }

            // Find the CourseOfFireEntry for the passed in Team, that has the same CourseOfFireId as this entry.
            // GetEntryByCourseOfFireId will create a new Entry if one does not exist.
            var teamEntry = (CourseOfFireEntryTeam)Team.MatchParticipant.GetEntryByCourseOfFireId( CourseOfFireId );
            teamEntry.TeamMembers.Remove( this.MatchParticipant.Participant );

            var oldTeam = Team;
            Team = null;

            if (!_ignoreEvents) {
                OnTeamLeft?.Invoke( this, new EventArgs<Team>( oldTeam ) );
            }
        }

        /// <summary>
        /// Newtonsoft.json helper method to determine if the RemarkList property should be serialized. We only want to serialize it if it has at least one RemarkAction in it.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeRemarkList() {
            return RemarkList != null && RemarkList.Count > 0;
        }

        #endregion
    }

    public class CourseOfFireEntryIndividual : CourseOfFireEntry {

        /// <summary>
        /// Initializes a new instance of the CourseOfFireEntryIndividual class, setting the participant type to
        /// individual and generating a new unique identifier for the result.
        /// </summary>
        public CourseOfFireEntryIndividual() {
            ParticipantType = ParticipantType.INDIVIDUAL;
            ResultCofId = System.Guid.NewGuid().ToString();
        }

        /// <summary>
        /// The unique identifier for the Result Course of Fire.
        /// </summary>
        [G_NS.JsonProperty( Order = 10 )]
        public string ResultCofId { get; set; } = string.Empty;

        /*
         * Purposefully not including a PaidStatus, as I am not ready to think through the implications of that, and it is not necessary for the current use cases.
         */

        /// <summary>
        /// Gets or sets the squadding assignment that defines where the Participant will shoot for this Course of Fire.
        /// </summary>
        [G_NS.JsonProperty( Order = 20 )]
        public SquaddingAssignment SquaddingAssignment { get; set; } = new SquaddingAssignmentFiringPoint();
    }

    /// <summary>
    /// Represents a team entry for a course of fire within a Match, with the participant type set to TEAM.
    /// </summary>
    /// <remarks>Team members are tracked as part of the Team <see cref="Participant"/>.</remarks>
    public class CourseOfFireEntryTeam : CourseOfFireEntry {

        /// <summary>
        /// Initializes a new instance of the CourseOfFireEntryTeam class with the participant type set to TEAM.
        /// </summary>
        public CourseOfFireEntryTeam() {
            ParticipantType = ParticipantType.TEAM;
        }

        /// <summary>
        /// Backwards pointer to the members of the team. 
        /// </summary>
        public List<Participant> TeamMembers { get; set; } = new List<Participant>();

        //There is nothing additiional to track for Team entries, as Teams are not squadded (not yet at least). 
        //Team members are tracked as part of the Team Participant
    }
}
