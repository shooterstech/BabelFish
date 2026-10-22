using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataModel.Clubs {
    public class ClubTeamMember : Contact, ICheckSum, IOnCloned {

        #region Private and Protected Fields
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructors, Factory Methods, and Initialization
        /// <summary>
        /// Default Constructor.
        /// <para>Initializes a new instance of the <see cref="ClubTeam"/> class with default values.</para>
        /// <para>Unless you happen to be a deserializer, it is recommended to use the <see cref="CreateAsync"/> method to create instances.</para>
        /// </summary>
        public ClubTeamMember() {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ClubTeamMember"/> and associates it with the provided <see cref="ClubTeam"/>.
        /// This method ensures that the new team member is properly linked to the club's team list.
        /// </summary>
        /// <param name="clubTeam">The club team to associate with the new team member.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the newly created <see cref="ClubTeamMember"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided <paramref name="clubTeam"/> is null.</exception>
        public static Task<ClubTeamMember> CreateAsync( ClubTeam clubTeam ) {
            // Although this method is currently synchronous in its implementation, it is defined as CreateAsync() to allow for future enhancements
            // that may involve asynchronous operations (e.g., database calls, API requests) during the creation process.
            // By giving this Async name now, we can avoid breaking changes in the future if such enhancements are needed.

            if (clubTeam is null)
                throw new ArgumentNullException( nameof( clubTeam ), "ClubTeam cannot be null when creating a ClubTeamMember." );

            ClubTeamMember clubTeamMember = new ClubTeamMember();
            clubTeamMember.ClubTeam = clubTeam;
            clubTeamMember.ClubTeam.TeamMembers ??= new List<ClubTeamMember>();
            clubTeam.TeamMembers.Add( clubTeamMember );

            return Task.FromResult( clubTeamMember );
        }

        /// <summary>
        /// Sets the Club property on the cloned instance to match the source instance. This ensures that when a ClubTeam is cloned, it remains associated with the same ClubDetail as the original.
        /// </summary>
        /// <param name="source">The original object that was cloned.</param>
        public void OnCloned( object source ) {

            if (source is ClubTeamMember sourceMember) {
                this.ClubTeam = sourceMember.ClubTeam;
            }
        }

        #endregion

        #region Data Property Members

        /// <summary>
        /// Boolean, indicating if this team member is also a Captain for the Club Team.
        /// </summary>
        public bool IsCaptain { get; set; } = false;

        /// <summary>
        /// Boolean, indicating if this team member is also a Coach for the Club Team.
        /// </summary>
        public bool IsCoach { get; set; } = false;

        #endregion

        #region Helper Properties
        /// <summary>
        /// Backwards pointer to the owning <see cref="ClubTeam"/>. This is not serialized in the API response, but is provided for ease of use in client applications.
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public ClubTeam? ClubTeam { get; set; }

        /// <inheritdoc />
        /// <remarks>Choosing not to include CheckSum in the serialized value, as it is not a top level document.</remarks>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public string CheckSum { get; set; } = string.Empty;

        #endregion

        #region Methods

        /// <inheritdoc />
        public ulong CalculateChecksum() {
            // Combine relevant properties into a single string for hashing
            string combined = $"{UserId}|{DisplayName}|{AccountUrlPath}|{Email}|{Phone}|{IsCaptain.ToString()}|{IsCoach.ToString()}";

            var hash = Helpers.Common.Md5ToUlong( combined );
            return hash;
        }

        #endregion
    }
}
