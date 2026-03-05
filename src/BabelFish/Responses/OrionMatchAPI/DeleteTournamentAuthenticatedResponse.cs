using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class DeleteTournamentAuthenticatedResponse : Response<DeleteTournamentResponseWrapper> {

        public DeleteTournamentAuthenticatedResponse( DeleteTournamentAuthenticatedRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value.DeleteTournamentResponse.
        /// </summary>
        public DeleteTournamentResponse DeleteTournamentResponse {
            get { return Value.DeleteTournamentResponse; }
        }
    }
}
