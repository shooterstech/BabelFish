using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NLog;

namespace Scopos.BabelFish.DataModel.OrionMatch {
	public class LeagueConfiguration {

		/// <summary>
		/// The AttributeValueAppellation (name) to use when looking up Event and Stage Style mappings
		/// </summary>
		public string AttributeValueAppellation { get; set; } = string.Empty;

		/// <summary>
		/// The SetName of the Course of Fire definition that is used to conduct all league games.
		/// </summary>
		public string CourseOfFireDef { get; set; } = string.Empty;

		/// <summary>
		/// Indicates how league teams are ranked.
		/// </summary>
        [JsonConverter( typeof( StringEnumConverter ) )]
        public LeagueRankingRuleType LeagueRankingRules { get; set; } = LeagueRankingRuleType.WIN_LOSS_RECORD;

		/// <summary>
		/// If the LeagueRankkingRules value is League Points, this LeaguePointsFactor indicates how much
		/// each win is worth.
		/// </summary>
		public int LeaguePointsFactor { get; set; }

		/// <summary>
		/// Number of team members that count towards the team's score total.
		/// </summary>
		public int NumberOfTeamMembers { get; set; }

		/// <summary>
		/// The total number of team members allowed to compete on a team.
		/// </summary>
		public int NumberOfTeamMembersMax { get; set; }

		/// <summary>
		/// SetName of the ScoreConfig used in this match.
		/// NOTE: The name of the ScoreFormatCollection is specified in the Course of Fire 
		/// </summary>
		public string ScoreConfigName { get; set; }

		/// <summary>
		/// Name of the TargetCollection used in this match.
		/// </summary>
		public string TargetCollectionName { get; set; }
	}
}
