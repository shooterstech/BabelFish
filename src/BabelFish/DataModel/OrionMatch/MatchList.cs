using System.Runtime.Serialization;
using Scopos.BabelFish.Converters.Microsoft;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Represents the data returned by a ListMatches API call.
    /// </summary>
    [Serializable]
    public class MatchList : ITokenItems<MatchAbbr> {

        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {
            if (Items == null) {
                Items = new List<MatchAbbr>();
            }
        }

        /// <summary>
        /// Total number of items matched before paging is applied.
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <inheritdoc />
        public List<MatchAbbr> Items { get; set; } = new List<MatchAbbr>();

        /// <inheritdoc />
        [G_STJ_SER.JsonConverter( typeof( NextTokenConverter ) )]
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
            return $"MatchList with {Items.Count} items";
        }
    }
}
