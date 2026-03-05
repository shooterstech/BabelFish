using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class DeleteTournamentAuthenticatedRequest : Request {

        public DeleteTournamentAuthenticatedRequest( UserAuthentication credentials, MatchID tournamentId ) : base( "DeleteTournament", credentials ) {
            HttpMethod = HttpMethod.Delete;
            TournamentId = tournamentId ?? throw new ArgumentNullException( nameof( tournamentId ) );
            SubDomain = APIClients.APISubDomain.AUTHAPI;
        }

        /// <summary>
        /// The Match ID of the tournament to delete.
        /// </summary>
        public MatchID TournamentId { get; set; }

        /// <inheritdoc />
        public override string RelativePath {
            get {
                if (TournamentId == null) {
                    throw new ArgumentNullException( nameof( TournamentId ), "The tournament id must be set to delete a tournament." );
                }

                return $"/tournament/{TournamentId}";
            }
        }
    }
}
