using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class GetTournamentAuthenticatedResponse : GetTournamentAbstractResponse {

        public GetTournamentAuthenticatedResponse( GetTournamentAuthenticatedRequest request ) : base() {
            this.Request = Request;
        }
    }
}