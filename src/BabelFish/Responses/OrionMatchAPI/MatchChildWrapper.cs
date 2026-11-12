using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {

    /// <summary>
    /// Helper class that creates the added structure needed for deserializing a MatchChild API response.
    /// </summary>
    public class MatchChildWrapper : BaseClass {

        public MatchChild MatchChild { get; set; } = new MatchChild();

        public override string ToString() {
            return $"MatchChild {MatchChild.Name}";
        }
    }
}
