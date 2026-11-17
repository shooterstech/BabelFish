using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.Newsfeed;

namespace Scopos.BabelFish.Responses.NewsfeedAPI {
    /// <summary>The named payload returned when a post is created.</summary>
    public class PostWrapper : BaseClass {
        /// <summary>The created post; absent on error responses.</summary>
        public Post? Post { get; set; }
    }
}
