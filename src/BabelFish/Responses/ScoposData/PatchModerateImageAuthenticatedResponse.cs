using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.Requests.ScoposData;

namespace Scopos.BabelFish.Responses.ScoposData {
    public class PatchModerateImageAuthenticatedResponse : Response<ImageWrapper> {

        public PatchModerateImageAuthenticatedResponse( PatchModerateImageAuthenticatedRequest request ) : base() {
            Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value.Image.
        /// </summary>
        public Image Image {
            get { return Value.Image; }
        }
    }
}
