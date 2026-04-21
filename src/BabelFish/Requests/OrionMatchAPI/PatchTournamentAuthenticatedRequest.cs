using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class PatchTournamentAuthenticatedRequest : Request {

        public PatchTournamentAuthenticatedRequest( UserAuthentication credentials ) : base( "PatchTournament", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            SubDomain = APIClients.APISubDomain.AUTHAPI;
        }

        public PatchTournamentAuthenticatedRequest(
            UserAuthentication credentials,
            MatchID tournamentId,
            string? tournamentName = null,
            VisibilityOption? visibility = null ) : base( "PatchTournament", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            TournamentId = tournamentId ?? throw new ArgumentNullException( nameof( tournamentId ) );
            TournamentName = tournamentName;
            Visibility = visibility;
            SubDomain = APIClients.APISubDomain.AUTHAPI;
        }

        /// <summary>
        /// The Match ID of the tournament being updated.
        /// </summary>
        public MatchID? TournamentId { get; set; } = null;

        /// <summary>
        /// Optional updated user-facing name of the tournament.
        /// </summary>
        public string? TournamentName { get; set; } = null;

        /// <summary>
        /// Optional updated tournament visibility.
        /// </summary>
        public VisibilityOption? Visibility { get; set; } = null;

        /// <inheritdoc />
        public override string RelativePath {
            get {
                if (TournamentId == null) {
                    throw new ArgumentNullException( nameof( TournamentId ), "The tournament id must be set to patch a tournament." );
                }

                return $"/tournament/{TournamentId}";
            }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                var parameterList = new Dictionary<string, List<string>>();

                if (TournamentName != null) {
                    if (string.IsNullOrWhiteSpace( TournamentName )) {
                        throw new ArgumentNullException( nameof( TournamentName ), "If provided, the tournament name must not be blank." );
                    }

                    parameterList.Add( "name", new List<string> { TournamentName } );
                }

                if (Visibility.HasValue) {
                    parameterList.Add( "visibility", new List<string> { EnumHelper.MemberValue( Visibility.Value ) } );
                }

                if (parameterList.Count == 0) {
                    throw new ArgumentException( "At least one of tournament name or visibility must be provided." );
                }

                return parameterList;
            }
        }
    }
}
