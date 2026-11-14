using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class DeleteMergedResultListAuthenticatedResponse : Response<DeleteMergedResultListResponseWrapper> {

        public DeleteMergedResultListAuthenticatedResponse( DeleteMergedResultListAuthenticatedRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value.DeleteMergedResultListResponse.
        /// </summary>
        public DeleteMergedResultListResponse DeleteMergedResultListResponse {
            get { return Value.DeleteMergedResultListResponse; }
        }
    }
}
