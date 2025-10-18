using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {

    /// <summary>
    /// Helper class that creates the added structure in the data model needed for Deserialzing a Tournament object from json.
    /// </summary>
    public class TournamentWrapper : BaseClass {
        public Tournament Tournament { get; set; } = new Tournament();

        public override string ToString() {
            return $"Tournament {Tournament.MatchName}";
        }
    }

}
