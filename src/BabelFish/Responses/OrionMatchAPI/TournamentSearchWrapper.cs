using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {

    /// <summary>
    /// Helper class that creates the added structure in the data model needed for deserializing
    /// tournament search results from json.
    /// </summary>
    public class TournamentSearchWrapper : BaseClass {

        public TournamentSearchList TournamentSearchList { get; set; } = new TournamentSearchList();

        public override string ToString() {
            return $"Tournament Search results of length {TournamentSearchList.Items.Count}";
        }
    }
}
