using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class UploadMatchFileAuthenticatedResponse : Response<UploadedMatchFileWrapper> {

        public UploadMatchFileAuthenticatedResponse( UploadMatchFileAuthenticatedRequest request ) : base() {
            Request = request;
        }
    }
}
