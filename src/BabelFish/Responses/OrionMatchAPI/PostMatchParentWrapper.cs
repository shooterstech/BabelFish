using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class PostMatchParentWrapper : BaseClass {

        public PostMatchParent PostMatchParent { get; set; } = new PostMatchParent();
    }
}
