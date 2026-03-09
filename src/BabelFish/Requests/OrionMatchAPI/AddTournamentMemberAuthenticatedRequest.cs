using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class AddTournamentMemberAuthenticatedRequest : Request {

        public AddTournamentMemberAuthenticatedRequest( UserAuthentication credentials, MatchID tournamentId, MatchID matchId ) : base( "AddTournamentMember", credentials ) {
            HttpMethod = HttpMethod.Post;
            TournamentId = tournamentId ?? throw new ArgumentNullException( nameof( tournamentId ) );
            MatchId = matchId ?? throw new ArgumentNullException( nameof( matchId ) );
            SubDomain = APIClients.APISubDomain.AUTHAPI;
        }

        /// <summary>
        /// The Match ID of the tournament where a new member is being added.
        /// </summary>
        public MatchID TournamentId { get; set; }

        /// <summary>
        /// The Match ID of the match being added as a tournament member.
        /// </summary>
        public MatchID MatchId { get; set; }

        /// <inheritdoc />
        public override string RelativePath {
            get {
                if (TournamentId == null) {
                    throw new ArgumentNullException( nameof( TournamentId ), "The tournament id must be set to add a tournament member." );
                }

                return $"/tournament/{TournamentId}/member";
            }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                if (MatchId == null) {
                    throw new ArgumentNullException( nameof( MatchId ), "The match id must be set to add a tournament member." );
                }

                return new Dictionary<string, List<string>> {
                    { "match-id", new List<string> { MatchId.ToString() } }
                };
            }
        }
    }
}
