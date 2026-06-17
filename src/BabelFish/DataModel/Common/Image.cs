using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataModel.Common {
    public class Image : BaseClass {

        /// <summary>
        /// The unique identifier for the image. This value is assigned by the database when the image is first created.
        /// A value of 0 indicates that the image has not yet been saved to the database.
        /// </summary>
        public int ImageId { get; set; } = 0;

        /// <summary>
        /// The full URL path to the image. This is the path that can be used to access the image directly via HTTP.
        /// <para>The value is assigned by the PatchImageModeration API call.</para>
        /// </summary>
        public string UrlPath { get; set; } = string.Empty;

        /// <summary>
        /// The internal S3 key for the image. This is the key used to store and retrieve the image from Amazon S3 storage.
        /// <para>The value is assigned by the PatchImageModeration API call.</para>
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        public string S3Key { get; set; } = "";

        /// <summary>
        /// The caption for the image. This is a user-friendly description of the image that can be displayed in the UI.
        /// Value is written by the person who uploaded the image.
        /// </summary>
        public string Caption { get; set; } = "";

        /// <summary>
        /// The alt text for the image. This is a text description of the image that can be used for accessibility purposes and as a fallback if the image cannot be displayed.
        /// Value is written by the person who uploaded the image.
        /// </summary>
        public string AltText { get; set; } = "";

        /// <summary>
        /// Boolean flag indicating whether the image has been resized by the PatchImageModeration API call after it was uploaded.
        /// </summary>
        public bool Resized { get; set; } = false;

        /// <summary>
        /// ENUM: Club, Match, League, or User.
        /// This value is used to determine the context in which the image is being used and to apply any relevant business rules or logic.
        /// </summary>
        public string ImageType { get; set; } = "Club";

        /// <summary>
        /// Value is dependent on the value of <see cref="ImageType"/>.
        /// <list type="bullet">
        /// <item>Club: The key will be the Orion Account (the owner) license number in string form.</item>
        /// <item>Match: The key will be the parent ID (which is a <see cref="MatchID"/>) in string form.</item>
        /// <item>League: The key will be the league ID (which is a <see cref="MatchID"/>) in string form.</item>
        /// <item>User: The key will be the UUID formatted user ID of the user who owns it.</item>
        /// </list>
        /// </summary>
        public string Key { get; set; } = "";

        /// <summary>
        /// Value is dependent on the value of <see cref="ImageType"/> and <see cref="GroupKey"/>.
        /// <list type="bullet">
        /// <item>Club: Not used.</item>
        /// <item>Match: Not used.</item>
        /// <item>League:
        ///   <list type="bullet">
        ///   <item>Header: The SubKey will be the team id of the team the photo represents, in string form.</item>
        ///   <item>Profile: The SubKey will be the team id of the team the photo represents, in string form.</item>
        ///   <item>Bulk: The SubKey will be the parent id (which is a <see cref="MatchID"/>) of the game of which the photo was taken, in string form.</item>
        ///   </list>
        /// </item>
        /// <item>User: Not used.</item>
        /// </list>
        /// </summary>
        public string SubKey { get; set; } = "";

        /// <summary>
        /// Enum: Header, Profile, or Bulk.
        /// </summary>
        public string GroupKey { get; set; } = "";

        /// <summary>
        /// The user ID of the person who uploaded the image. This is a UUID formatted string that uniquely identifies the user (aka Scopos Account).
        /// </summary>
        public string UserId { get; set; } = "";

        public string FileType { get; set; } = ".jpg";

        /// <summary>
        /// New, Approved, Rejected, or Flagged.
        /// <para>This value is set by the PatchImageModeration API call after the image is uploaded.</para>
        /// </summary>
        public string SafeToShowStatus { get; set; } = "New";
    }
}
