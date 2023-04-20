using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.ScoreHistory {

    /// <summary>
    /// Represents the data returned by a GetScoreAverage API call.
    /// </summary>
    public class ScoreAverage : ITokenItems<ScoreAverageBase> {

        public ScoreAverage() { }

        public List<ScoreAverageBase> Items { get; set; } = new List<ScoreAverageBase>();

        /// <inheritdoc />
        public string NextToken { get; set; } = string.Empty;

        /// <inheritdoc />
        public int Limit { get; set; } = 50;

        public override string ToString() {
            return $"ScoreAverage with {Items.Count} items";
        }

    }
}
