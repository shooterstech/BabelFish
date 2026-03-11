using System.Runtime.Serialization;
using Scopos.BabelFish.Converters.Microsoft;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// Represents the data returned by a Tournament Search API call.
    /// </summary>
    [Serializable]
    public class TournamentSearchList : ITokenItems<Tournament> {

        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {
            if (Items == null) {
                Items = new List<Tournament>();
            }
        }

        /// <summary>
        /// Total number of items matched before paging is applied.
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <inheritdoc />
        public List<Tournament> Items { get; set; } = new List<Tournament>();

        /// <inheritdoc />
        [G_STJ_SER.JsonConverter( typeof( NextTokenConverter ) )]
        public string NextToken { get; set; } = string.Empty;

        /// <inheritdoc />
        public int Limit { get; set; } = 10;

        /// <inheritdoc />
        public bool HasMoreItems {
            get {
                return !string.IsNullOrEmpty( NextToken );
            }
        }

        public override string ToString() {
            return $"TournamentSearch with {Items.Count} items";
        }
    }
}
