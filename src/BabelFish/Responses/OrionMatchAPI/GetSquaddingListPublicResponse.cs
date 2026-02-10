using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class GetSquaddingListPublicResponse : GetSquaddingListAbstractResponse {

        public GetSquaddingListPublicResponse( GetSquaddingListPublicRequest request ) : base() {
            this.Request = request;
        }
    }
}
