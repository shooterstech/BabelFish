using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataModel.Clubs {

    /// <summary>
    /// A Club Team represents a group of Club Members (aka Orion Account members) that shoot the same <see cref="EventStyle"/>
    /// and often compete with each other during competitions.
    /// </summary>
    /// <remarks>
    /// Club Teams are stored in the database tables orion_account_team, orion_account_team_member, and orion_account_team_event_style.
    /// </remarks>
    public class ClubTeam : ICheckSum, IOnCloned, G_STJ_SER.IJsonOnDeserialized {

        #region Private and Protected Fields
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private VisibilityOption _visibility = VisibilityOption.PUBLIC;

        /// <summary>
        /// Helper property to return the list of valid values for <see cref="Visibility"/>.
        /// <list type="bullet">
        /// <item>
        /// <description>PROTECTED: May be seen by Club Members.</description>
        /// </item>
        /// <item>
        /// <description>PUBLIC: May be seen by anyone.</description>
        /// </item>
        /// </list>
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public static readonly List<VisibilityOption> VisibilityOptions = new List<VisibilityOption>() { VisibilityOption.PROTECTED, VisibilityOption.PUBLIC };

        #endregion

        #region Constructors, Factory Methods, and Initialization
        /// <summary>
        /// Default Constructor.
        /// <para>Initializes a new instance of the <see cref="ClubTeam"/> class with default values.</para>
        /// <para>Unless you happen to be a deserializer, it is recommended to use the <see cref="CreateAsync"/> method to create instances.</para>
        /// </summary>
        public ClubTeam() {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ClubTeam"/> and associates it with the provided <see cref="ClubDetail"/>.
        /// This method ensures that the new team is properly linked to the club's team list.
        /// </summary>
        /// <param name="club">The club to associate with the new team.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the newly created <see cref="ClubTeam"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided <paramref name="club"/> is null.</exception>
        public static Task<ClubTeam> CreateAsync( ClubDetail club ) {
            // Although this method is currently synchronous in its implementation, it is defined as CreateAsync() to allow for future enhancements
            // that may involve asynchronous operations (e.g., database calls, API requests) during the creation process.
            // By giving this Async name now, we can avoid breaking changes in the future if such enhancements are needed.

            if (club is null)
                throw new ArgumentNullException( nameof( club ), "ClubDetail cannot be null when creating a ClubTeam." );

            ClubTeam clubTeam = new ClubTeam();
            clubTeam.Club = club;
            clubTeam.Club.TeamList ??= new List<ClubTeam>();
            clubTeam.Club.TeamList.Add( clubTeam );

            return Task.FromResult( clubTeam );
        }

        /// <summary>
        /// Sets the Club property on the cloned instance to match the source instance. This ensures that when a ClubTeam is cloned, it remains associated with the same ClubDetail as the original.
        /// </summary>
        /// <param name="source">The original object that was cloned.</param>
        public void OnCloned( object source ) {

            if (source is ClubTeam sourceTeam) {
                this.Club = sourceTeam.Club;
            }
        }

        /// <summary>
        /// Gets called by System.Text.Json when an instance is deserialized.
        /// </summary>
        public void OnDeserialized() {
            this.TeamMembers ??= new List<ClubTeamMember>();
            this.EventStyles ??= new List<ClubTeamEventStyle>();

            // Set the backwards pointer for each Team Member and Event Style to this ClubTeam instance
            foreach (var member in TeamMembers) {
                if (member is not null)
                    member.ClubTeam = this;
            }

            foreach (var eventStyle in EventStyles) {
                if (eventStyle is not null)
                    eventStyle.ClubTeam = this;
            }
        }

        /// <summary>
        /// Gets called by NewtonSoft.Json's deserializer.
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) => OnDeserialized();

        #endregion

        #region Data Property Members

        /// <summary>
        /// Globally unique identifier for this team.
        /// <para>A value of 0 means the Club Team has not yet been saved to the database.</para>
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        public int TeamId { get; set; } = 0;

        /// <summary>
        /// The Club Team's Name.
        /// <para>Does not have to be unique, but probable should be unique within the Club (aka Orion Account).</para>
        /// </summary>
        /// <example>West Postomac AJROTC</example>
        [Required]
        [StringLength( 128, MinimumLength = 4, ErrorMessage = "Name must be at least 4 characters and cannot be longer than 128 characters." )]
        [G_STJ_SER.JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The team's preferred display name on small width screens (typically bootstrap's medium screens or smaller).
        /// <para>Value may be an empty string. Maximum length is 24 characters.</para>
        /// </summary>
        [StringLength( 24, ErrorMessage = "NameShort cannot be longer than 24 characters." )]
        [G_STJ_SER.JsonPropertyOrder( 3 )]
        [G_NS.JsonProperty( Order = 3 )]
        public string NameShort { get; set; } = string.Empty;

        /// <summary>
        /// The team's preferred display name on extra small width screens (typically smaller than bootstrap's small screens).
        /// <para>Value may be an empty string. Maximum length is 12 characters.</para>
        /// </summary>
        [StringLength( 12, ErrorMessage = "NameExtraShort cannot be longer than 12 characters." )]
        [G_STJ_SER.JsonPropertyOrder( 4 )]
        [G_NS.JsonProperty( Order = 4 )]
        public string NameExtraShort { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the mascot name.
        /// <para>Value is not required, may be an empty string.</para>
        /// </summary>
        /// <example>Wolverines</example>
        [G_STJ_SER.JsonPropertyOrder( 10 )]
        [G_NS.JsonProperty( Order = 10 )]
        [DefaultValue( "" )]
        public string Mascot { get; set; } = string.Empty;

        /// <summary>
        /// The list of team members for this Club Team.
        /// </summary>
        public List<ClubTeamMember> TeamMembers { get; set; } = new List<ClubTeamMember>();

        /// <summary>
        /// The list of <see cref="EventStyle"/> that this team participates in. A team can compete in many different event styles.
        /// <para>While not enforced, a Club Team should have at least one <see cref="ClubTeamEventStyle"/>.</para>
        /// <para>The preferred method for adding a <see cref="ClubTeamEventStyle"/> is to use the <see cref="ClubTeamEventStyle.CreateAsync(ClubTeam)"/> method.</para>
        /// </summary>
        public List<ClubTeamEventStyle> EventStyles { get; set; } = new List<ClubTeamEventStyle>();

        /// <summary>
        /// Gets or sets the visibility level that controls who can view this address.
        /// The set value must be contained in <see cref="VisibilityOptions"/>. If not, the most restrictive option (the first in the list) will be used instead.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 50 )]
        [G_NS.JsonProperty( Order = 50, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public VisibilityOption Visibility {
            get => _visibility;
            set {
                if (!VisibilityOptions.Contains( value ))
                    _visibility = VisibilityOptions[0];

                _visibility = value;
            }
        }
        #endregion

        #region Helper Properties

        /// <summary>
        /// Backwards pointer to the owning ClubDetail. This is not serialized in the API response, but is provided for ease of use in client applications.
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public ClubDetail? Club { get; set; }

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
            string combined = $"{Club?.AccountNumber}|{TeamId}|{Name}|{Mascot}|{Visibility.Description()}";
            var hash = Helpers.Common.Md5ToUlong( combined );

            foreach (var member in TeamMembers) {
                hash ^= member.CalculateChecksum();
            }

            foreach (var eventStyle in EventStyles) {
                hash ^= eventStyle.CalculateChecksum();
            }

            return hash;
        }

        #endregion
    }

}
