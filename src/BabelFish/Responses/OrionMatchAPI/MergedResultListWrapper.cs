using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class MergedResultListWrapper : BaseClass {

        public MergedResultList MergedResultList { get; set; } = new MergedResultList();
    }
}
