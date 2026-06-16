using Scopos.BabelFish.DataModel;

namespace Scopos.BabelFish.DataModel.Common {
    public class Image : BaseClass {

        [G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_STJ_SER.JsonPropertyName( "S3Key" )]
        [G_NS.JsonProperty( "S3Key", Order = 1 )]
        public string S3Key { get; set; } = "";

        [G_STJ_SER.JsonPropertyOrder( 2 )]
        [G_STJ_SER.JsonPropertyName( "Resized" )]
        [G_NS.JsonProperty( "Resized", Order = 2 )]
        public bool Resized { get; set; } = false;

        [G_STJ_SER.JsonPropertyOrder( 3 )]
        [G_STJ_SER.JsonPropertyName( "Caption" )]
        [G_NS.JsonProperty( "Caption", Order = 3 )]
        public string Caption { get; set; } = "";

        [G_STJ_SER.JsonPropertyOrder( 4 )]
        [G_STJ_SER.JsonPropertyName( "AltText" )]
        [G_NS.JsonProperty( "AltText", Order = 4 )]
        public string AltText { get; set; } = "";

        [G_STJ_SER.JsonPropertyOrder( 5 )]
        [G_STJ_SER.JsonPropertyName( "Key" )]
        [G_NS.JsonProperty( "Key", Order = 5 )]
        public string Key { get; set; } = "";

        [G_STJ_SER.JsonPropertyOrder( 6 )]
        [G_STJ_SER.JsonPropertyName( "SubKey" )]
        [G_NS.JsonProperty( "SubKey", Order = 6 )]
        public string SubKey { get; set; } = "";

        [G_STJ_SER.JsonPropertyOrder( 7 )]
        [G_STJ_SER.JsonPropertyName( "GroupKey" )]
        [G_NS.JsonProperty( "GroupKey", Order = 7 )]
        public string GroupKey { get; set; } = "";

        [G_STJ_SER.JsonPropertyOrder( 8 )]
        [G_STJ_SER.JsonPropertyName( "UserId" )]
        [G_NS.JsonProperty( "UserId", Order = 8 )]
        public string UserId { get; set; } = "";

        [G_STJ_SER.JsonPropertyOrder( 9 )]
        [G_STJ_SER.JsonPropertyName( "SafeToShow" )]
        [G_NS.JsonProperty( "SafeToShow", Order = 9 )]
        public bool SafeToShow { get; set; } = false;
    }
}
