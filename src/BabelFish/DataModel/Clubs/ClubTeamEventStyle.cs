using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataModel.Clubs {
    public class ClubTeamEventStyle : ICheckSum, IGetEventStyleDefinition {

        #region Private and Protected Fields
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructors, Factory Methods, and Initialization
        /// <summary>
        /// Default Constructor.
        /// <para>Initializes a new instance of the <see cref="ClubTeamEventStyle"/> class with default values.</para>
        /// <para>Unless you happen to be a deserializer, it is recommended to use the <see cref="CreateAsync"/> method to create instances.</para>
        /// </summary>
        public ClubTeamEventStyle() {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ClubTeamEventStyle"/> and associates it with the provided <see cref="ClubTeam"/>.
        /// This method ensures that the new EventStyle is properly linked to the club's team list.
        /// </summary>
        /// <param name="clubTeam">The club team to associate with the new EventStyle.</param>
        /// <param name="eventStyleDef">The event style definition to associate with the new ClubTeamEventStyle.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the newly created <see cref="ClubTeamEventStyle"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided <paramref name="clubTeam"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the provided <paramref name="eventStyleDef"/> is the default SetName.</exception>
        /// <exception cref="DefinitionNotFoundException">Thrown when the provided <paramref name="eventStyleDef"/> cannot be found.</exception>
        public static async Task<ClubTeamEventStyle> CreateAsync( SetName eventStyleDef, ClubTeam clubTeam ) {
            if (clubTeam is null)
                throw new ArgumentNullException( nameof( clubTeam ), "ClubTeam cannot be null when creating a ClubTeamEventStyle." );
            if (eventStyleDef is null)
                throw new ArgumentNullException( nameof( eventStyleDef ), "EventStyleDef cannot be null when creating a ClubTeamEventStyle." );
            if (eventStyleDef.IsDefault)
                throw new ArgumentException( "EventStyleDef cannot be default when creating a ClubTeamEventStyle.", nameof( eventStyleDef ) );

            // Load the Event Style definition to ensure it exists and is valid. This will throw an exception if the DefinitionNotFound exception if it cannot be found.
            var eventStyleDefinition = await DefinitionCache.GetEventStyleDefinitionAsync( eventStyleDef );

            // Make sure the ClubTeam's EventStyles list is initialized
            clubTeam.EventStyles ??= new List<ClubTeamEventStyle>();

            // Make sure the ClubTeam doesn't already have this EventStyleDef. If it does, we can return the existing instance instead of creating a duplicate.
            var existingEventStyle = clubTeam.EventStyles.FirstOrDefault( es => es.EventStyleDef == eventStyleDef );
            if (existingEventStyle != null)
                return existingEventStyle;

            // OK, OK, now we can create a new ClubTeamEventStyle and add it to the ClubTeam's EventStyles list.
            ClubTeamEventStyle clubTeamEventStyle = new ClubTeamEventStyle();
            clubTeamEventStyle.EventStyleDef = eventStyleDef;
            clubTeamEventStyle.ClubTeam = clubTeam;
            clubTeamEventStyle.HeaderName = StringFormatting.GetTruncatedString( eventStyleDefinition.CommonName, 24 ); ;
            clubTeam.EventStyles.Add( clubTeamEventStyle );

            return clubTeamEventStyle;
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
        /// The <see cref="EventStyle"/>that this <see cref="ClubTeam"/> participates in.
        /// </summary>
        public SetName EventStyleDef { get; set; } = new SetName();

        /// <summary>
        /// When scores for this Event Style are displayed, the header name is how the Club wishes to label the scores.
        /// <para>For example "Three-Posiiton Air Rifle" or "Smallbore".</para>
        /// </summary>
        [Required]
        [StringLength( 24, MinimumLength = 4, ErrorMessage = "HeaderName, must be at least 4 characters and no longer than 24 characters." )]
        [DefaultValue( "" )]
        public string HeaderName { get; set; } = string.Empty;
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
        /// <inheritdoc/>
        /// <exception cref="ArgumentException">Thrown if the value of .EventStyleDef could not be parsed. Which shouldn't happen.</exception>
        public async Task<EventStyle> GetEventStyleDefinitionAsync() {

            return await DefinitionCache.GetEventStyleDefinitionAsync( EventStyleDef );

        }

        /// <summary>
        /// Non-async method to try and return the <see cref="EventStyle"/> definition for <see cref="EventStyleDef"/>.
        /// <para>A returned value of false, may mean that the <see cref="EventStyle"/> could not be found in the <see cref="DefinitionCache"/>,
        /// and it may be found later once the cache is populated.</para>
        /// </summary>
        /// <param name="eventStyle">The output parameter that will contain the <see cref="EventStyle"/> if found.</param>
        /// <returns>True if the <see cref="EventStyle"/> was found; otherwise, false.</returns>
        public bool TryGetEventStyleDefinition( out EventStyle eventStyle ) {
            return DefinitionCache.TryGetEventStyleDefinition( EventStyleDef, out eventStyle );
        }

        /// <inheritdoc />
        public ulong CalculateChecksum() {
            // Combine relevant properties into a single string for hashing
            string combined = $"{EventStyleDef.ToString()}";

            var hash = Helpers.Common.Md5ToUlong( combined );
            return hash;
        }

        #endregion
    }
}
