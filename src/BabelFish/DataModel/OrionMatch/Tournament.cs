using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataActors.ResultListMerger;
using Scopos.BabelFish.DataModel.Common;


namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// A Tournament is a group of Matches.
    /// <para>The Matches in the Tournament are called its <see cref="TournamentMembers">members</see>.</para>
    /// <para>Scores from the member's Result Lists may be merged together in interesting ways (e.g. sum all the scores together).
    /// These are called <see cref="MergedResultList"/>.</para>
    /// <para>The <see cref="MatchID"/> for tournaments always end in ".2".</para>
    /// </summary>
    public class Tournament : 
        IMergedResultListContainer,
        G_STJ_SER.IJsonOnDeserialized,
        G_STJ_SER.IJsonOnDeserializing {

        #region Private Variables
        private bool _ignoreEvents = false;
        private Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructors, Factory Methods, and Initialization Methods

        /// <summary>
        /// Public constructor.
        /// <para>Usually not called directly, instead Tournaments are read from REST API using
        /// <see cref="OrionMatchAPIClient.GetTournamentPublicAsync(MatchID)"/> (or related method).</para>
        /// </summary>
        public Tournament()  { }

        /// <summary>
        /// This method is called after deserialization with System.Text.Json. 
        /// </summary>
        public void OnDeserialized() {

            _ignoreEvents = false;
            foreach (var tournamentMember in TournamentMembers) {
                tournamentMember.Tournament = this;
            }

            foreach (var mergedResultList in MergedResultLists) {
                mergedResultList.Container = this;
            }
        }

        /// <summary>
        /// Method is called before deserialization with System.Text.Json.
        /// </summary>
        public void OnDeserializing() {

            _ignoreEvents = true;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Gets invoked when a new TournamentMember is added to this Tournament. The new member is passed in as an argument.
        /// </summary>
        public event EventHandler<EventArgs<TournamentMember>> OnTournamentMemberAdded;

        /// <summary>
        /// Gets invoked when a new MergedResultList is added to this Tournament. The new MergedResultList is passed in as an argument.
        /// </summary>
        public event EventHandler<EventArgs<MergedResultList>> OnMergedResultListAdded;

        #endregion

        #region Data Model Properties
        /// <summary>
        /// Concrete class identifier
        /// </summary>
        [G_NS.JsonProperty( Order = 1, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public MatchType MatchType { get; set; }

        /// <summary>
        /// The Unique Match ID
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.MatchIdConverter ) )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.MatchIdConverter ) )]
        public MatchID MatchId { get; set; }

        /* Order = 3 reserved for concrete classes */

        /// <summary>
        /// Human readable name given to the match. Does not have to be unique (but is helpful if it is).
        /// </summary>
        [G_NS.JsonProperty( Order = 4 )]
        public string MatchName { get; set; }

        //EKA Question Oct 2025: Should we have a ShortMatchName. The idea is, if this match is part of a Tournament, this ShortMatchName can be used as the default column header name.

        /* Order = 5 reserved for concrete classes */

        /// <summary>
        /// The account identifier that owns this Match.
        /// <para>Will be in the form OrionAcct000001 or AtHome000001.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 6 )]
        public string OwnerId { get; set; } = string.Empty;

        /* Order = 7, 8, 9 skipped for future use */

        /// <summary>
        /// The start date of the match
        /// </summary>
        /// <example>2001-01-01</example>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        [G_NS.JsonProperty( Order = 10 )]
        public DateTime StartDate { get; set; } = DateTime.Today;

        /// <summary>
        /// The end date of the match
        /// </summary>
        /// <example>2001-01-01</example>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        [G_NS.JsonProperty( Order = 11 )]
        public DateTime EndDate { get; set; } = DateTime.Today;

        /// <summary>
        /// The visibility (who can see it) given to this match. 
        /// </summary>
        [G_NS.JsonProperty( Order = 12, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public VisibilityOption Visibility { get; set; } = VisibilityOption.PRIVATE;

        /* Order = 13, 14 skipped for future use */

        /// <summary>
        /// Indicates if this match should be include on match search results.
        /// </summary>
        [G_NS.JsonProperty( Order = 15 )]
        public bool IncludeInSearchResults { get; set; } = false;

        /// <summary>
        /// List of officials for this match.
        /// </summary>
        /// <remarks>OCT 2025: Currently unknown what this object will look like.</remarks>
        public List<object> Officials { get; set; } = new List<object>();

        /// <summary>
        /// Newtonsoft helper method to determine if Officials should be part of the serialized json.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeOfficials() {
            return Officials != null && Officials.Count > 0;
        }


        /// <summary>
        /// Purposefully redundant. Exact same value as MatchName. But since this is a concrete 
        /// implementation of a Tournament we call it TournamentName.
        /// </summary>
        [G_NS.JsonProperty( Order = 5 )]
        public string TournamentName {
            get {
                return this.MatchName;
            }
        }

        /// <summary>
        /// Purposefully redundant. Exact same value as MatchId. But since this is a concrete 
        /// implementation of a Tournament we call it TournamentId.
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        public MatchID TournamentId {
            get {
                return this.MatchId;
            }
        }

        /// <summary>
        /// The policy in place to allow new matches (in the form of TournamentMembers) to be added to this Tournament. For example,
        /// if the policy is set to "Invite", then new members can only be added by being invited by the owner of the tournament.
        /// If the policy is set to "Open", then anyone can add themselves as a member of the tournament.
        /// </summary>
        [G_NS.JsonProperty( Order = 10 )]
        public MemberPolicyOption MemberPolicy { get; set; } = MemberPolicyOption.INVITE;

        /// <summary>
        /// The list of Matches that are Members of this Tournament.
        /// <para>The preferred method for adding a TournamentMmber is to use the <see cref="AddMember(TournamentMember)"/> method.
        /// As this method will invoke <see cref="OnTournamentMemberAdded"/> event.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 30 )]
        public List<TournamentMember> TournamentMembers { get; set; } = new List<TournamentMember>();

        /// <summary>
        /// A list of MergedResultLists. Each MergedResultList specifies a way to merge scores from the member's Result Lists together.
        /// </summary>
        [G_NS.JsonProperty( Order = 35 )]
        public List<MergedResultList> MergedResultLists { get; set; } = new List<MergedResultList>();

        /// <summary>
        /// Indicates whether the tournament is abbreviated. Abbreviated tournamnents may still have members/result
        /// lists but these lists may be incomplete or include only requested information (e.g. for incoming invites)
        /// </summary>
        [G_NS.JsonProperty( Order = 50 )]
        public bool Abbreviated { get; set; } = false;
        #endregion

        #region Helper Properties

        /// <inheritdoc />
        [G_NS.JsonIgnore]
        public IResultListFetcher ResultListFetcher { get; private set; } = new OrionMatchAPIClient();

        #endregion

        #region Methods
        /// <summary>
        /// This is the preferred method for adding a TournamentMember to this Tournament. It will add the member to the TournamentMembers list,
        /// set the member's Tournament property to this Tournament, and invoke the OnTournamentMemberAdded event.
        /// </summary>
        /// <param name="member"></param>
        public void AddMember( TournamentMember member ) {
            TournamentMembers.Add( member );
            member.Tournament = this;

            if (!_ignoreEvents) {
                OnTournamentMemberAdded?.Invoke( this, new EventArgs<TournamentMember>( member ) );
            }
        }

        /// <summary>
        /// Creates a new MergedResultList and adds it to <see cref="MergedResultLists"/>, then invokes the <see cref="OnMergedResultListAdded"/> event.
        /// The configuration of the MergedResultList is determined by the passed in MergeMethodType. For example, if MergeMethodType.SUM is passed in,
        /// then the MergedResultList's Configuration will be set to a new instance of SumMethodConfiguration.
        /// </summary>
        /// <param name="resultListName"></param>
        /// <param name="mergeMethodType"></param>
        /// <returns></returns>
        /// <remarks>NOTE This code is effectively the same as <see cref="MatchStructure.CreateMergedResultListAsync(MergeMethodType)"/>. If you change
        /// code here, change it there too.</remarks>
        public async Task<MergedResultList> AddMergedResultListAsync( string resultListName, MergeMethodType mergeMethodType ) {

            MergedResultList mrl = await MergedResultList.CreateAsync( this, resultListName, mergeMethodType );

            MergedResultLists.Add( mrl );

            if (!_ignoreEvents) {
                OnMergedResultListAdded?.Invoke( this, new EventArgs<MergedResultList>( mrl ) );
            }
            return mrl;
        }

        #endregion
    }
}
