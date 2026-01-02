using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class LeagueSeason {

        public LeagueSeason() {

        }

        /// <summary>
        /// Name of the season, usually a reference to a year, eg "2019"
        /// </summary>
        public string SeasonName { get; set; }

        public int SeasonID { get; set; }
    }
}
