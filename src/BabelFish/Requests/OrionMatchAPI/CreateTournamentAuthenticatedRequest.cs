using System.Linq;
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
            OwnerId = ParseOwnerId( tournament.OwnerId );
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
        public int OwnerId { get; set; }

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
        public string? MemberPolicy { get; set; } = "INVITE"; 

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
                    { "owner-id", new List<string> { OwnerId.ToString() } }
                };

                parameterList.Add("visibility", new List<string> { GetEnumMemberValue(Visibility) });

                parameterList.Add("show-on-search", new List<string> { ShowOnSearch.ToString() });

                if (!string.IsNullOrWhiteSpace( MemberPolicy )) {
                    parameterList.Add( "member-policy", new List<string> { MemberPolicy } );
                }

                return parameterList;
            }
        }

        private static int ParseOwnerId( string ownerId ) {
            if (string.IsNullOrWhiteSpace( ownerId )) {
                throw new ArgumentNullException( nameof( ownerId ), "The owner id must be set on the Tournament object." );
            }

            if (int.TryParse( ownerId, out int parsedOwnerId )) {
                return parsedOwnerId;
            }

            var ownerIdDigits = new string( ownerId.Where( char.IsDigit ).ToArray() );
            if (int.TryParse( ownerIdDigits, out parsedOwnerId )) {
                return parsedOwnerId;
            }

            throw new ArgumentException( "The Tournament.OwnerId must contain a numeric Orion account id (for example, \"2255\" or \"OrionAcct0002255\")." );
        }

    }
}
