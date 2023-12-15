using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    /*
     * Choosing to keep LeagueTeamDetail in the Scopos.BabelFish.Responses.OrionMatchAPI namespace, instead of
     * Scopos.BabelFish.DataModel.OrionMatch, as these properties are mostly 'helpeer' properties that the 
     * API includes. And do not describe unique data model values.
     * 
     * EKA - Dec 2023
     */

    public class LeagueTeamDetail : LeagueBase {

        public LeagueTeam LeagueTeam { get; set; }
    }
}
