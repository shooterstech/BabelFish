using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Represents one of the two teams playing in a LeagueGame, including their results and reference back to their LeagueTeam.
    /// </summary>
    public class LeagueTeamResult {

        public LeagueTeam Team { get; set; }

        public Scopos.BabelFish.DataModel.Athena.Score Score { get; set; }

        public string Result { get; set; }

    }
}
