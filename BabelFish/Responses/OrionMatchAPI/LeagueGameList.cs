using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.OrionMatch;
using System.Text.Json;
using Scopos.BabelFish.Converters;
using System.Text.Json.Serialization;


namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    /*
     * Choosing to keep LeagueGameList in the Scopos.BabelFish.Responses.OrionMatchAPI namespace, instead of
     * Scopos.BabelFish.DataModel.OrionMatch, as these properties are mostly 'helpeer' properties that the 
     * API includes. And do not describe unique data model values.
     * 
     * EKA - Dec 2023
     */

    public class LeagueGameList : LeagueBase, ITokenItems<LeagueGame> {


        public List<LeagueGame> Items { get; set; } = new List<LeagueGame>();

        /// <inheritdoc />
        [JsonConverter( typeof( NextTokenConverter ) )]
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
