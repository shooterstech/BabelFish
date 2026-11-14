using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class ListParentMatchChildrenAuthenticatedResponse : ListParentMatchChildrenAbstractResponse {

        public ListParentMatchChildrenAuthenticatedResponse( ListParentMatchChildrenAuthenticatedRequest request ) : base() {
            Request = request;
        }

        /// <inheritdoc/>
        public new ListParentMatchChildrenAuthenticatedRequest GetNextRequest() {
            return (ListParentMatchChildrenAuthenticatedRequest)base.GetNextRequest();
        }
    }
}
