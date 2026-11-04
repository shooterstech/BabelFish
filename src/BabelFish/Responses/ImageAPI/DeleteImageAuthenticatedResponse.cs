using Scopos.BabelFish.DataModel.Image;
using Scopos.BabelFish.Requests.ImageAPI;

namespace Scopos.BabelFish.Responses.ImageAPI {
    /// <summary>
    /// Response to an authenticated image deletion request.
    /// </summary>
    public class DeleteImageAuthenticatedResponse : Response<DeletedImageWrapper> {
        /// <summary>
        /// Constructs a response for the originating request.
        /// </summary>
        /// <param name="request">The originating deletion request.</param>
        public DeleteImageAuthenticatedResponse( DeleteImageAuthenticatedRequest request ) : base() {
            Request = request;
        }

        /// <summary>
        /// Deletion confirmation, or null when no successful payload is available.
        /// </summary>
        public ScoposImageAbbr? DeletedImage => Value?.DeletedImage;
    }
}
