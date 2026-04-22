namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// Any Team, or group of Participants in a Match. The group of Participants can be athletes or other teams
    /// </summary>
    [Serializable]
    public class Team : Participant,
        G_STJ_SER.IJsonOnDeserializing,
        G_STJ_SER.IJsonOnDeserialized {

        /// <summary>
        /// Deprecated concrete class identifier. 
        /// </summary>
        [Obsolete( "Deprecated March 2026. Use Type instead." )]
        public const int CONCRETE_CLASS_ID = 2;

        #region Private and Protected Fields
        private string _teamName = string.Empty;
        private bool _ignoreEvents = false;
        #endregion

        #region Constructors, Facory Methods, and Initialization Methods

        /// <summary>
        /// Constructor for Team. Sets the ParticipantTyhpe to the correct value for a Team, and initializes the TeamMembers and TeamCaptains lists.
        /// </summary>
        public Team() : base() {
            ConcreteClassId = CONCRETE_CLASS_ID;
            ParticipantType = ParticipantType.TEAM;
            this.TeamMembers = new List<Participant>();
            this.TeamCaptains = new List<Individual>();
        }

        /// <summary>
        /// Gets called after System.Text.Json deserializes an instance of this class. Sets the
        /// Team property of each TeamMember to this Team, and sets _ignoreEvents to false to
        /// allow events to fire after deserialization.
        /// </summary>
        public void OnDeserialized() {
            foreach (var tm in this.TeamMembers) {
                tm.Team = this;
            }
            _ignoreEvents = false;
        }

        /// <summary>
        /// Gets called before System.Text.Json deserializes an instance of this class.
        /// Sets _ignoreEvents to true to prevent events from firing during deserialization.
        /// </summary>
        public void OnDeserializing() {
            _ignoreEvents = true;
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Gets called when a Participant is added to the TeamMembers list. The event handler is passed the Participant that was added to the TeamMembers list.
        /// </summary>
        public EventHandler<EventArgs<Participant>> OnTeamMemberAdded;
        #endregion

        #region Data Model Properties
        /*
        /// <summary>
        /// The contributing team members. These are the Participants that will make up the score shot by the team. 
        /// </summary>
        [G_NS.JsonProperty( Order = 5 )]
        public List<Participant> TeamMembers { get; set; }
        */

        /// <summary>
        /// Returns the same value as DisplayName, but is intended to be used when the Participant is a Team. The setter does nothing, as the TeamName is always the same as DisplayName.
        /// </summary>
        public override string TeamName {
            get {
                return this._teamName;
            }
            set {
                this._teamName = value.Trim();
                if (this.DefaultDisplayName)
                    SetDefaultDisplayName();
            }
        }

        /// <summary>
        /// The designated team captains for this team. A Team captain may also be a coach and may also be a member.
        /// </summary>
        [G_NS.JsonProperty( Order = 25 )]
        public List<Individual> TeamCaptains { get; set; }

        #endregion

        #region Methods
        public void AddTeamMember( Participant participant ) {
            this.TeamMembers.Add( participant );
            participant.Team = this;
            if (!_ignoreEvents)
                OnTeamMemberAdded?.Invoke( this, new EventArgs<Participant>( participant ) );
        }

        /// <inheritdoc />
        public override void SetDefaultDisplayName() {
            this.DefaultDisplayName = true;
            this._displayName = this.TeamName;
        }
        #endregion

        /// <inheritdoc/>
        public override int UniqueMergeId {
            get {
                return this.DisplayName.GetHashCode();
            }
        }
    }
}
