using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters.Microsoft;

namespace Scopos.BabelFish.DataModel.ScoreHistory {

    /// <summary>
    /// Represents the data returned by a Get score History API Call
    /// </summary>
    public class ScoreHistoryList : ITokenItems<ScoreHistoryBase> {

        /// <summary>
        /// Public base constructor.
        /// </summary>
        public ScoreHistoryList() { }

        public List<ScoreHistoryBase> Items { get; set; } = new List<ScoreHistoryBase>();

        /// <inheritdoc />
		[JsonConverter( typeof( NextTokenConverter ) )]
        public string NextToken { get; set; } = string.Empty;

        /// <inheritdoc />
        public int Limit { get; set; } = 50;

        /// <inheritdoc />
        public bool HasMoreItems {
            get {
                return !string.IsNullOrEmpty( NextToken );
            }
        }

        public override string ToString() {
            return $"ScoreHistory with {Items.Count} items";
        }

    }
}
