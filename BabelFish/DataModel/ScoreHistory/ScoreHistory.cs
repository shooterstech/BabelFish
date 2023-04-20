using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.ScoreHistory {

    /// <summary>
    /// Represents the data returned by a Get score History API Call
    /// </summary>
    public class ScoreHistory : ITokenItems<ScoreHistoryBase> {

        /// <summary>
        /// Public base constructor.
        /// </summary>
        public ScoreHistory() { }

        public List<ScoreHistoryBase> Items { get; set; } = new List<ScoreHistoryBase>();

        /// <inheritdoc />
        public string NextToken { get; set; } = string.Empty;

        /// <inheritdoc />
        public int Limit { get; set; } = 50;

        public override string ToString() {
            return $"ScoreHistory with {Items.Count} items";
        }

    }
}
