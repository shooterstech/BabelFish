using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class ListParentMatchChildrenPublicResponse : ListParentMatchChildrenAbstractResponse {

        public ListParentMatchChildrenPublicResponse( ListParentMatchChildrenPublicRequest request ) : base() {
            Request = request;
        }

        /// <inheritdoc/>
        public new ListParentMatchChildrenPublicRequest GetNextRequest() {
            return (ListParentMatchChildrenPublicRequest)base.GetNextRequest();
        }
    }
}
