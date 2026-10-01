using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters.Microsoft;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataModel.Image {
    public class ImageList : ITokenItems<ScoposImageAbbr> {

        public ImageList() {
            Items = new List<ScoposImageAbbr>();
        }

        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {
            if (Items == null)
                Items = new List<ScoposImageAbbr>();
        }

        /// <summary>
        /// A list of ScoposImageAbbr data objects.
        /// </summary>        
        public List<ScoposImageAbbr> Items { get; set; }

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

        /// <inheritdoc />
        public override string ToString() {
            return $"BulkImageList with {Items.Count} items";
        }
    }
}
