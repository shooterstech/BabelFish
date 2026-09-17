using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class PostMatchParentAuthenticatedResponse : Response<PostMatchParentWrapper> {

        public PostMatchParentAuthenticatedResponse( PostMatchParentAuthenticatedRequest request ) : base() {
            Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value.PostMatchParent.
        /// </summary>
        public PostMatchParent PostMatchParent {
            get { return Value.PostMatchParent; }
        }
    }
}
