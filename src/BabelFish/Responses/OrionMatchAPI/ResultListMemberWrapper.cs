using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class ResultListMemberWrapper : BaseClass {

        public ResultListMember ResultListMember { get; set; } = new ResultListMember();
    }
}
