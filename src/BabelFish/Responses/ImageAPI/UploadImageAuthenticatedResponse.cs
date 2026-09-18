using Scopos.BabelFish.DataModel.Image;
using Scopos.BabelFish.Requests.ImageAPI;
using Scopos.BabelFish.Responses.ScoposData;

namespace Scopos.BabelFish.Responses.ImageAPI {
    public class UploadImageAuthenticatedResponse : Response<ImageWrapper> {

        public UploadImageAuthenticatedResponse( UploadImageAuthenticatedRequest request ) : base() {
            Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value.Image.
        /// </summary>
        public ScoposImage Image {
            get { return Value.Image; }
        }
    }
}
