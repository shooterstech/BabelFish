using Scopos.BabelFish.DataModel.Newsfeed;
using Scopos.BabelFish.Requests.NewsfeedAPI;

namespace Scopos.BabelFish.Responses.NewsfeedAPI {
    /// <summary>The result of soft-deleting a newsfeed post.</summary>
    public class DeleteNewsfeedPostAuthenticatedResponse : Response<DeletedPostWrapper> {
        /// <summary>Attaches the originating request.</summary>
        public DeleteNewsfeedPostAuthenticatedResponse( DeleteNewsfeedPostAuthenticatedRequest request ) : base() {
            Request = request;
        }

        /// <summary>The complete deleted post, or null when no successful payload is available.</summary>
        public Post? DeletedPost => Value?.DeletedPost;
    }
}
