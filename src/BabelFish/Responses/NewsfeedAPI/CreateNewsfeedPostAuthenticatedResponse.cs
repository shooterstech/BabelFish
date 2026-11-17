using Scopos.BabelFish.DataModel.Newsfeed;
using Scopos.BabelFish.Requests.NewsfeedAPI;

namespace Scopos.BabelFish.Responses.NewsfeedAPI {
    /// <summary>The result of creating a newsfeed post.</summary>
    public class CreateNewsfeedPostAuthenticatedResponse : Response<PostWrapper> {
        /// <summary>Attaches the originating request.</summary>
        public CreateNewsfeedPostAuthenticatedResponse( CreateNewsfeedPostAuthenticatedRequest request ) : base() {
            Request = request;
        }

        /// <summary>The created post, or null when no successful payload is available.</summary>
        public Post? Post => Value?.Post;
    }
}
