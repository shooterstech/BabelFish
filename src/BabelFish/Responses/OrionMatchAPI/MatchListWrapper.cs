using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {

    /// <summary>
    /// Helper class that creates the added structure needed for deserializing a MatchList API response.
    /// </summary>
    public class MatchListWrapper : BaseClass {

        public MatchList MatchList { get; set; } = new MatchList();

        public override string ToString() {
            return $"MatchList with {MatchList.Items.Count} items";
        }
    }
}
