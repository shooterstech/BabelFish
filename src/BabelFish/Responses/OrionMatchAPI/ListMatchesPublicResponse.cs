using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class ListMatchesPublicResponse : ListMatchesAbstractResponse {

        public ListMatchesPublicResponse( ListMatchesPublicRequest request ) : base() {
            Request = request;
        }

        /// <inheritdoc />
        public new ListMatchesPublicRequest GetNextRequest() {
            return (ListMatchesPublicRequest)base.GetNextRequest();
        }
    }
}
