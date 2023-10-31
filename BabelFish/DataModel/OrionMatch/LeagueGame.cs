using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;
using Scopos.BabelFish.Converters;
using Scopos.BabelFish.DataModel.OrionMatch;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.OrionMatch {

	/// <summary>
	/// A LeagueGame is a special type of Match. Competed between two teams within a League.
	/// </summary>
	public class LeagueGame {

		/// <summary>
		/// The unique Match ID for this game. 
		/// </summary>
		public string GameID { get; set; }

		/// <summary>
		/// Human readable name for this game.
		/// </summary>
		public string GameName { get; set; }

		/// <summary>
		/// The Virtual type of league game. Usually Not Set, Local, Virtual, Bye Week, or Cancelled.
		/// </summary>
		public LeagueVirtualType Virtual { get; set; }

		/// <summary>
		/// The date this league game started.
		/// </summary>
		[JsonConverter(typeof(DateConverter))]
		public DateTime StartDate { get; set; }

		/// <summary>
		/// The date this league game ended.
		/// </summary>
        [JsonConverter( typeof( DateConverter ) )]
        public DateTime EndDate { get; set; }

		public LeagueTeamResult HomeTeam { get; set; }

		public LeagueTeamResult AwayTeam { get; set; }
    }
}
