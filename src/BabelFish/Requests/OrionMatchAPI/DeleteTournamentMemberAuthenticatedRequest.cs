using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class DeleteTournamentMemberAuthenticatedRequest : Request {

        public DeleteTournamentMemberAuthenticatedRequest( UserAuthentication credentials, MatchID tournamentId, MatchID matchId ) : base( "DeleteTournamentMember", credentials ) {
            HttpMethod = HttpMethod.Delete;
            TournamentId = tournamentId ?? throw new ArgumentNullException( nameof( tournamentId ) );
            MatchId = matchId ?? throw new ArgumentNullException( nameof( matchId ) );
            SubDomain = APIClients.APISubDomain.AUTHAPI;
        }

        public DeleteTournamentMemberAuthenticatedRequest( UserAuthentication credentials, TournamentMember tournamentMember ) : base( "DeleteTournamentMember", credentials ) {
            HttpMethod = HttpMethod.Delete;
            if (tournamentMember == null) {
                throw new ArgumentNullException( nameof( tournamentMember ) );
            }

            TournamentId = tournamentMember.TournamentId ?? throw new ArgumentNullException( nameof( tournamentMember ), "The tournament member must have a tournament id." );
            MatchId = tournamentMember.MatchId ?? throw new ArgumentNullException( nameof( tournamentMember ), "The tournament member must have a match id." );
            SubDomain = APIClients.APISubDomain.AUTHAPI;
        }

        /// <summary>
        /// The Match ID of the tournament where the member is being removed.
        /// </summary>
        public MatchID TournamentId { get; set; }

        /// <summary>
        /// The Match ID of the match being removed from tournament membership.
        /// </summary>
        public MatchID MatchId { get; set; }

        /// <inheritdoc />
        public override string RelativePath {
            get {
                if (TournamentId == null) {
                    throw new ArgumentNullException( nameof( TournamentId ), "The tournament id must be set to delete a tournament member." );
                }

                return $"/tournament/{TournamentId}/member";
            }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                if (MatchId == null) {
                    throw new ArgumentNullException( nameof( MatchId ), "The match id must be set to delete a tournament member." );
                }

                return new Dictionary<string, List<string>> {
                    { "match-id", new List<string> { MatchId.ToString() } }
                };
            }
        }
    }
}
