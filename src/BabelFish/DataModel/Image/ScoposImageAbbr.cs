namespace Scopos.BabelFish.DataModel.Image {

    /// <summary>
    /// A ScoposImageAbbr represents a simplified version (the pertinent information) of an image that has been uploaded to the Rezults platform
    /// and is associated with a specific club, match, league, or user profile.
    /// <para>Values are expected to be read only. Changing them will not affect the underlying data in the database.</para>
    /// </summary>
    public class ScoposImageAbbr {


        /// <summary>
        /// The unique identifier for the image. This value is assigned by the database when the image is first created.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        public string ImageId { get; set; } = string.Empty;

        /// <summary>
        /// The caption for the image. This is a user-friendly description of the image that can be displayed in the UI.
        /// Value is written by the person who uploaded the image.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        public string Caption { get; set; } = "";

        /// <summary>
        /// The alt text for the image. This is a text description of the image that can be used for accessibility purposes and as a fallback if the image cannot be displayed.
        /// Value is written by the person who uploaded the image.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 3 )]
        [G_NS.JsonProperty( Order = 3 )]
        public string AltText { get; set; } = "";

        /// <summary>
        /// The full URL path to the image. This is the path that can be used to access the image directly via HTTP.
        /// <para>The value is assigned by the PatchImageModeration API call.</para>
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 4 )]
        [G_NS.JsonProperty( Order = 4 )]
        public string UrlPath { get; set; } = string.Empty;
    }
}
