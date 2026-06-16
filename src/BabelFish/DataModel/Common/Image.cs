namespace Scopos.BabelFish.DataModel.Common {
    /// <summary>
    /// Represents an image record returned by Scopos REST API image calls.
    /// </summary>
    public class Image : BaseClass {

        public Image() { }

        [G_STJ_SER.JsonPropertyName( "S3Key" )]
        [G_NS.JsonProperty( "S3Key" )]
        public string S3Key { get; set; } = string.Empty;

        [G_STJ_SER.JsonPropertyName( "Resized" )]
        [G_NS.JsonProperty( "Resized" )]
        public bool Resized { get; set; } = false;

        [G_STJ_SER.JsonPropertyName( "AltText" )]
        [G_NS.JsonProperty( "AltText" )]
        public string AltText { get; set; } = string.Empty;

        [G_STJ_SER.JsonPropertyName( "Key" )]
        [G_NS.JsonProperty( "Key" )]
        public string Key { get; set; } = string.Empty;

        [G_STJ_SER.JsonPropertyName( "SubKey" )]
        [G_NS.JsonProperty( "SubKey" )]
        public string SubKey { get; set; } = string.Empty;

        [G_STJ_SER.JsonPropertyName( "GroupKey" )]
        [G_NS.JsonProperty( "GroupKey" )]
        public string GroupKey { get; set; } = string.Empty;

        [G_STJ_SER.JsonPropertyName( "UserId" )]
        [G_NS.JsonProperty( "UserId" )]
        public string UserId { get; set; } = string.Empty;

        [G_STJ_SER.JsonPropertyName( "SafeToShow" )]
        [G_NS.JsonProperty( "SafeToShow" )]
        public bool SafeToShow { get; set; } = false;
    }
}
