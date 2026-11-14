using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class AddMergedResultListMemberAuthenticatedResponse : Response<ResultListMemberWrapper> {

        public AddMergedResultListMemberAuthenticatedResponse( AddMergedResultListMemberAuthenticatedRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value.ResultListMember.
        /// </summary>
        public ResultListMember ResultListMember {
            get { return Value.ResultListMember; }
        }
    }
}
