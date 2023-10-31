using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.OrionMatch {
	public class LeagueGameList : ITokenItems<LeagueGame> {


        /// <summary>
        /// Unique LeagueID for this League.
        /// 
        /// This is a required field.
        /// </summary>
        public string LeagueID { get; set; } = string.Empty;

        public string LeagueName { get; set;} = string.Empty;

        public int LeagueNetworkID { get; set; }

        public string LeagueNetworkName { get; set; }

        public int SeasonID { get; set; }

        public string SeasonName { get; set; }

        public LeagueSeasonType SeasonType { get; set; }

        public LeagueConfiguration Configuration { get; set; }

        public List<LeagueGame> Items { get; set; } = new List<LeagueGame>();

		/// <inheritdoc />
		public string NextToken { get; set; } = string.Empty;

		/// <inheritdoc />
		public int Limit { get; set; } = 50;

        /// <inheritdoc />
        public bool HasMoreItems {
            get {
                return !string.IsNullOrEmpty(NextToken);
            }
        }
	}
}
