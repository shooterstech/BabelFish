using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel;

namespace Scopos.BabelFish.DataModel.OrionMatch {
	public class LeagueGameSearch : ITokenItems<LeagueGame> {

		public string LeagueName { get; set; }

		public List<LeagueGame> Items { get; set; } = new List<LeagueGame>();

		/// <inheritdoc />
		public string NextToken { get; set; } = string.Empty;

		/// <inheritdoc />
		public int Limit { get; set; } = 50;
	}
}
