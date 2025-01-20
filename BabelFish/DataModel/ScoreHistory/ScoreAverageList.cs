using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.ScoreHistory {

    /// <summary>
    /// Represents the data returned by a GetScoreAverage API call.
    /// </summary>
    public class ScoreAverageList : ITokenItems<ScoreAverageBase> {

        public ScoreAverageList() { }

        public List<ScoreAverageBase> Items { get; set; } = new List<ScoreAverageBase>();

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
            return $"ScoreAverage with {Items.Count} items";
        }

    }
}
