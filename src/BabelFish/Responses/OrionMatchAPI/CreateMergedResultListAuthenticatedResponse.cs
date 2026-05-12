using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class CreateMergedResultListAuthenticatedResponse : Response<MergedResultListWrapper> {

        public CreateMergedResultListAuthenticatedResponse( CreateMergedResultListAuthenticatedRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value.MergedResultList.
        /// </summary>
        public MergedResultList MergedResultList {
            get { return Value.MergedResultList; }
        }
    }
}
