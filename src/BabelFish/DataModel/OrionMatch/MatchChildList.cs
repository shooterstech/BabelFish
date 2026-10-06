using System.Runtime.Serialization;
using Scopos.BabelFish.Converters.Microsoft;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Represents the data returned by a ListParentMatchChildren API call.
    /// </summary>
    [Serializable]
    public class MatchChildList : ITokenItems<MatchChild> {

        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {
            if (Items == null) {
                Items = new List<MatchChild>();
            }
        }

        /// <inheritdoc />
        public List<MatchChild> Items { get; set; } = new List<MatchChild>();

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
            return $"MatchChildList with {Items.Count} items";
        }
    }
}
