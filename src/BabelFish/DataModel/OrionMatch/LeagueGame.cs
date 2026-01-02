using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;
using Scopos.BabelFish.Converters.Microsoft;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataModel.OrionMatch {

	/// <summary>
	/// A LeagueGame is a special type of Match. Competed between two teams within a League.
	/// </summary>
	public class LeagueGame {

		/// <summary>
		/// The unique Match ID for this game. 
		/// </summary>
		public MatchID GameID { get; set; }

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
        [G_STJ_SER.JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The date this league game ended.
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The URL to the post-game press release.
        /// <para>An empty string means a press release does not exist.</para>
        /// </summary>
        public string PressReleaseUrl { get; set; }

        /// <summary>
        /// Human readable Name of the League Network
        /// </summary>
        public string LeagueNetworkName { get; set; }

        public int LeagueNetworkID { get; set; }

        /// <summary>
        /// Human readable name of the League's Season
        /// </summary>
        public string SeasonName { get; set; }

        public int SeasonID { get; set; }

        /// <summary>
        /// Human readabel name of the League
        /// </summary>
        public string LeagueName { get; set; }

        /// <summary>
        /// The scheibentoni ID of the league
        /// </summary>
        public MatchID LeagueID { get; set; }


        public LeagueTeamResult HomeTeam { get; set; }

		public LeagueTeamResult AwayTeam { get; set; }

        public string ReportText { get; set; }

        public string ReportHTML { get; set; }

        public override string ToString() {
			return GameName;
        }

		/// <summary>
		/// logical boolean for if we show the leaderboard link
		/// </summary>
		public bool ShowLeaderboard { 
			get {
				if ( (Virtual == LeagueVirtualType.VIRTUAL || Virtual == LeagueVirtualType.LOCAL) && (HomeTeam.Result == "" && AwayTeam.Result == "") ) 
					return true; 
				return false; 
			} 
		}
    }
}
