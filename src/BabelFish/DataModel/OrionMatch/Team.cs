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



        private static List<Tuple<string, string>> _replacementStringsFirstRound = new List<Tuple<string, string>>() {
            Tuple.Create( "High School", string.Empty ),
            Tuple.Create( "American Legion Post", "Post" ),
            Tuple.Create( "Gun Club", string.Empty ),
            Tuple.Create( "Academy", string.Empty ),
            Tuple.Create( "Shooting Sports", string.Empty )
        };

        private static List<Tuple<string, string>> _replacementStringsSecondRound = new List<Tuple<string, string>>() {
            Tuple.Create( "ARJTOC", string.Empty ),
            Tuple.Create( "NJROTC", string.Empty ),
            Tuple.Create( "MCJROTC", string.Empty ),
            Tuple.Create( "CGJROTC", string.Empty ),
            Tuple.Create( "SFJROTC", string.Empty ),
            Tuple.Create( "AFJROTC", string.Empty ),
            Tuple.Create( "JROTC", string.Empty ),
            Tuple.Create( "WEST", "W" ),
            Tuple.Create( "EAST", "E" ),
            Tuple.Create( "NORTH", "N" ),
            Tuple.Create( "SOUTH", "S" ),
            Tuple.Create( "County", string.Empty )
        };

        #region Private and Protected Fields
        private bool _ignoreEvents = false;
        private string _teamName = string.Empty;
        #endregion

        #region Constructors, Facory Methods, and Initialization Methods

        /// <summary>
        /// Constructor for Team. Sets the ParticipantTyhpe to the correct value for a Team, and initializes the TeamMembers and TeamCaptains lists.
        /// </summary>
        public Team() : base() {
            ConcreteClassId = CONCRETE_CLASS_ID;
            ParticipantType = ParticipantType.TEAM;
            this.TeamCaptains = new List<Individual>();
        }

        /// <summary>
        /// Gets called after System.Text.Json deserializes an instance of this class. Sets the
        /// Team property of each TeamMember to this Team, and sets _ignoreEvents to false to
        /// allow events to fire after deserialization.
        /// </summary>
        public void OnDeserialized() {
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
        public event EventHandler<EventArgs<Participant>> OnTeamMemberAdded;
        #endregion

        #region Data Model Properties

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
        /// <inheritdoc />
        public override string GetDisplayNameShort( int maxLength = DISPLAY_NAME_SHORT_MAX_LENGTH ) {
            if (maxLength < DISPLAY_NAME_EXTRA_SHORT_MAX_LENGTH)
                maxLength = DISPLAY_NAME_EXTRA_SHORT_MAX_LENGTH;

            // First check the length of the DisplayName. If it is shorter than the max length, return it.
            if (this.DisplayName.Length <= maxLength) {
                return this.DisplayName;
            }

            // Next check the length of the DisplayNameShort. If it is shorter than the max length, return it.
            if (!string.IsNullOrWhiteSpace( this.DisplayNameShort ) && this.DisplayNameShort.Length <= maxLength) {
                return this.DisplayNameShort;
            }

            // Next check the length of the DisplayNameExtraShort. If it is shorter than the max length, return it.
            if (!string.IsNullOrWhiteSpace( this.DisplayNameExtraShort ) && this.DisplayNameExtraShort.Length <= maxLength) {
                return this.DisplayNameExtraShort;
            }

            // Using the values in _replacementStringsFirstRound, replace any matching strings, using case insensitive search, in the DisplayName with the replacement string.
            string displayNameShort = this.DisplayName;
            foreach (var replacement in _replacementStringsFirstRound) {
                displayNameShort = System.Text.RegularExpressions.Regex.Replace(
                    displayNameShort,
                    System.Text.RegularExpressions.Regex.Escape( replacement.Item1 ),
                    replacement.Item2,
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase
                );
            }

            if (displayNameShort.Length <= maxLength) {
                return displayNameShort;
            }

            // Using the values in _replacementStringsSecondRound, replace any matching strings, using case insensitive search, in the DisplayName with the replacement string.
            foreach (var replacement in _replacementStringsSecondRound) {
                displayNameShort = System.Text.RegularExpressions.Regex.Replace(
                    displayNameShort,
                    System.Text.RegularExpressions.Regex.Escape( replacement.Item1 ),
                    replacement.Item2,
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase
                );
            }

            if (displayNameShort.Length <= maxLength) {
                return displayNameShort;
            }

            // If it is still too long, truncate the string to the max lngth and return it.
            return StringFormatting.GetTruncatedString( displayNameShort, maxLength );
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


        /// <inheritdoc />
        public override ulong CalculateChecksum() {
            var combined = $"{DisplayName}|{CompetitorNumber}|{Country}|{HomeTown}|{Club}|{TeamName}";
            var hash = Helpers.Common.Md5ToUlong( combined );

            foreach (var attributeValue in AttributeValues) {
                hash ^= attributeValue.CalculateChecksum();
            }

            foreach (var teamCaptain in TeamCaptains) {
                hash ^= teamCaptain.CalculateChecksum();
            }

            hash ^= RemarkList.CalculateChecksum();

            return hash;
        }
    }
}
