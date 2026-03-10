using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class CreateTournamentAuthenticatedRequest : Request {

        public CreateTournamentAuthenticatedRequest( UserAuthentication credentials ) : base( "CreateTournament", credentials ) {
            HttpMethod = HttpMethod.Post;
            SubDomain = APIClients.APISubDomain.AUTHAPI;
        }

        public CreateTournamentAuthenticatedRequest( UserAuthentication credentials, Tournament tournament ) : base( "CreateTournament", credentials ) {
            if (tournament == null) {
                throw new ArgumentNullException( nameof( tournament ) );
            }
            
            HttpMethod = HttpMethod.Post;
            TournamentName = tournament.TournamentName;
            OwnerId = tournament.OwnerId;
            Visibility = tournament.Visibility;
            ShowOnSearch = tournament.IncludeInSearchResults;
            MemberPolicy = tournament.MemberPolicy;
            SubDomain = APIClients.APISubDomain.AUTHAPI;
        }

        /// <summary>
        /// The user-facing name of the tournament.
        /// </summary>
        public string TournamentName { get; set; } = string.Empty;

        /// <summary>
        /// License number of the Orion account that owns this tournament.
        /// </summary>
        public string OwnerId { get; set; } = string.Empty;

        /// <summary>
        /// Optional visibility value. Valid values are Public, Protected, and Private.
        /// </summary>
        public VisibilityOption Visibility { get; set; } = VisibilityOption.PRIVATE;

        /// <summary>
        /// Optional flag indicating if this tournament is returned in search results.
        /// </summary>
        public bool ShowOnSearch { get; set; } = false;

        /// <summary>
        /// Optional member policy value. Valid values are OPEN, REQUEST, and INVITE.
        /// </summary>
        public MemberPolicyOption? MemberPolicy { get; set; } = MemberPolicyOption.INVITE;

        /// <inheritdoc />
        public override string RelativePath {
            get {
                return "/tournament";
            }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                if (string.IsNullOrWhiteSpace( TournamentName )) {
                    throw new ArgumentNullException( $"{nameof( TournamentName )} must be set to create a tournament." );
                }

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>() {
                    { "name", new List<string> { TournamentName } },
                    { "owner-id", new List<string> { OwnerId } }
                };

                parameterList.Add("visibility", new List<string> { EnumHelper.MemberValue(Visibility) });

                parameterList.Add("show-on-search", new List<string> { ShowOnSearch.ToString() });

                if (MemberPolicy.HasValue) {
                    parameterList.Add( "member-policy", new List<string> { EnumHelper.MemberValue( MemberPolicy.Value ) } );
                }

                return parameterList;
            }
        }

    }
}
