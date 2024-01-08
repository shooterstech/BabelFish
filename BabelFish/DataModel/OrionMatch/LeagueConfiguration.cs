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

		public string AttributeValueAppellation { get; set; } = string.Empty;

		public string CourseOfFireDef { get; set; } = string.Empty;

		public string LeagueRankingRules { get; set; } = string.Empty;

		public int LeaguePointsFactor { get; set; }

		public int NumberOfTeamMembers { get; set; }

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
