using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class TournamentSearchAuthenticatedResponse : TournamentSearchAbstractResponse {

        public TournamentSearchAuthenticatedResponse( TournamentSearchAuthenticatedRequest request ) : base() {
            Request = request;
        }

        /// <inheritdoc />
        public new TournamentSearchAuthenticatedRequest GetNextRequest() {
            return (TournamentSearchAuthenticatedRequest)base.GetNextRequest();
        }
    }
}
