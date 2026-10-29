using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters.Microsoft;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataModel.Image {
    /// <summary>
    /// Defined the DataModel for a <see cref="Scopos.BabelFish.Requests.ImageAPI.GetImagesRequest"/> API call. This is a list of ScoposImageAbbr data objects, with a NextToken for pagination.
    /// </summary>
    public class ImageList :
        ITokenItems<ScoposImageAbbr>,
        G_STJ_SER.IJsonOnDeserialized {

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageList"/> class.
        /// </summary>
        public ImageList() {
            Items = new List<ScoposImageAbbr>();
        }

        /// <summary>
        /// Called when the object is deserialized by System.Text.Json. Ensures that the Items list is initialized to an empty list if it is null.
        /// </summary>
        public void OnDeserialized() {
            if (Items == null)
                Items = new List<ScoposImageAbbr>();
        }

        /// <summary>
        /// Called when the object is deserialized by Newtonsoft.Json. Ensures that the Items list is initialized to an empty list if it is null.
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) => OnDeserialized();

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
            return $"ImageList with {Items.Count} items";
        }
    }
}
