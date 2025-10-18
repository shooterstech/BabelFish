using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetTournamentAuthenticatedRequest : GetTournamentAbstractRequest {

        /// <summary>
        /// Consructor
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="credentials"></param>
        public GetTournamentAuthenticatedRequest( MatchID tournamentId, UserAuthentication credentials ) : base( "GetTournament", tournamentId, credentials ) {
        }
    }
}
