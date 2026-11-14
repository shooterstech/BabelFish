using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {

    /// <summary>
    /// Helper class that creates the added structure needed for deserializing a MatchChildList API response.
    /// </summary>
    public class MatchChildListWrapper : BaseClass {

        public MatchChildList MatchChildList { get; set; } = new MatchChildList();

        public override string ToString() {
            return $"MatchChildList with {MatchChildList.Items.Count} items";
        }
    }
}
