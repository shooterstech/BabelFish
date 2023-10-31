using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {

    /// <summary>
    /// Helper class that creates the added structure in the data model needed for Deserialzing a ResultList object from json.
    /// </summary>
    public class LeagueGameListWrapper : BaseClass {

        public LeagueGameList LeagueGames = new LeagueGameList();

        public override string ToString() {
            return $"LeagueGames Search Result for {LeagueGames.LeagueName}";
        }
    }
}
