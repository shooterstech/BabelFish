using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.Newsfeed;

namespace Scopos.BabelFish.Responses.NewsfeedAPI {
    /// <summary>The named payload returned when a post is soft-deleted.</summary>
    public class DeletedPostWrapper : BaseClass {
        /// <summary>The complete post including stored deletion audit fields; absent on errors.</summary>
        public Post? DeletedPost { get; set; }
    }
}
