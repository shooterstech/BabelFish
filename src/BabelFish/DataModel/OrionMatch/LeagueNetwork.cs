using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class LeagueNetwork {


        public LeagueNetwork() {
            Seasons = new List<LeagueSeason>();
        }

        /// <summary>
        /// The name of this LeagueNetwork
        /// </summary>
        public string LeagueNetworkName { get; set; }

        /// <summary>
        /// Globally unique ID
        /// </summary>
        public int LeagueNetworkID { get; set; }

        public List<LeagueSeason> Seasons { get; set; }

        public LeagueSeason AddSeason( string seasonName ) {
            LeagueSeason ls = new LeagueSeason();
            int maxID = 0;
            foreach (var s in this.Seasons)
                if (s.SeasonID > maxID)
                    maxID = s.SeasonID;

            ls.SeasonName = seasonName;
            ls.SeasonID = maxID + 1;

            this.Seasons.Add( ls );

            return ls;
        }

        /// <summary>
        /// The GMT time this LeagueNetwork was last updated
        /// </summary>
        public DateTime LastUpdated { get; set; }

        public string JSONVersion { get; set; }
    }
}
