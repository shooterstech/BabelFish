namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// Any Team, or group of Participants in a Match. The group of Participants can be athletes or other teams
    /// </summary>
    [Serializable]
    public class Team : Participant {

        public const int CONCRETE_CLASS_ID = 2;

        public Team() : base() {
            ConcreteClassId = CONCRETE_CLASS_ID;
            this.TeamMembers = new List<Participant>();
            this.TeamCaptains = new List<Individual>();
        }

        /// <summary>
        /// The contributing team members. These are the Participants that will make up the score shot by the team. 
        /// </summary>
        [G_NS.JsonProperty( Order = 5 )]
        public List<Participant> TeamMembers { get; set; }

        /// <summary>
        /// Returns the same value as DisplayName, but is intended to be used when the Participant is a Team. The setter does nothing, as the TeamName is always the same as DisplayName.
        /// </summary>
        [G_NS.JsonIgnore] // TeamName is always the same as DisplayName, so ignore it for JSON purposes.
        public override string TeamName { get => base.DisplayName; set {; } }

        /// <summary>
        /// The designated team captains for this team. A Team captain may also be a coach and may also be a member.
        /// </summary>
        [G_NS.JsonProperty( Order = 25 )]
        public List<Individual> TeamCaptains { get; set; }

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

            foreach (var teamMember in TeamMembers) {
                hash ^= teamMember.CalculateChecksum();
            }

            foreach (var teamCaptain in TeamCaptains) {
                hash ^= teamCaptain.CalculateChecksum();
            }

            hash ^= RemarkList.CalculateChecksum();

            return hash;
        }
    }
}
