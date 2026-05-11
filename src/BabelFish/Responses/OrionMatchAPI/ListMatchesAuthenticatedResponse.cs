using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class ListMatchesAuthenticatedResponse : ListMatchesAbstractResponse {

        public ListMatchesAuthenticatedResponse( ListMatchesAuthenticatedRequest request ) : base() {
            Request = request;
        }

        /// <inheritdoc />
        public new ListMatchesAuthenticatedRequest GetNextRequest() {
            return (ListMatchesAuthenticatedRequest)base.GetNextRequest();
        }
    }
}
