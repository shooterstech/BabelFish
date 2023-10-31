using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Describes the result of one team that competed in one league game.
    /// </summary>
    public class LeagueTeamResult {

        public LeagueTeam Team { get; set; }

        public Score Score { get; set; }

        public string Result { get; set; }

    }
}
