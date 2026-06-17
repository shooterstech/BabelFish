using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataModel.ScoposData {

    /// <summary>
    /// A ScoposImage represents an image that has been uploaded to the Rezults platform and is associated with a specific club, match, league, or user profile.
    /// his class is used to manage and display images within the Rezults application while ensuring that they are properly categorized and moderated according to the platform's guidelines.
    /// </summary>
    /// <remarks>Name the class ScoposImage instead of just Image to avoid conflicts with the System.Drawing.Image class.</remarks>
    /// <remarks>
    /// External documentation for this class may be found at:
    /// https://docs.google.com/document/d/17M5888Px6ztdQGH6M-5e5W_DfeVK6urVxRG1XVlQ7Oo/edit?usp=sharing
    /// </remarks>
    public class ScoposImage : BaseClass {

        /// <summary>
        /// The unique identifier for the image. This value is assigned by the database when the image is first created.
        /// A value of 0 indicates that the image has not yet been saved to the database.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        public int ImageId { get; set; } = 0;

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
        /// The full URL path to the image. This is the path that can be used to access the image directly via HTTP.
        /// <para>The value is assigned by the PatchImageModeration API call.</para>
        /// </summary>
        public string UrlPath { get; set; } = string.Empty;

        /// <summary>
        /// ENUM: Club, Match, League, or User.
        /// This value is used to determine the context in which the image is being used and to apply any relevant business rules or logic.
        /// </summary>
        public ImageCategory ImageCategory { get; set; } = ImageCategory.CLUB;

        /// <summary>
        /// Value is dependent on the value of <see cref="ImageCategory"/>.
        /// <list type="bullet">
        /// <item>Club: The key will be the Orion Account (the owner) license number in string form.</item>
        /// <item>Match: The key will be the parent ID (which is a <see cref="MatchID"/>) in string form.</item>
        /// <item>League: The key will be the league ID (which is a <see cref="MatchID"/>) in string form.</item>
        /// <item>User: The key will be the UUID formatted user ID of the user who owns it.</item>
        /// </list>
        /// </summary>
        public string Key { get; set; } = "";

        /// <summary>
        /// Value is dependent on the value of <see cref="ImageCategory"/> and <see cref="GroupKey"/>.
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
        /// ImageGroupKey helps to categorize images based on their intended use within Rezults. It specifies where the image is
        /// meant to be displayed, such as in headers, profiles, or as part of a bulk collection of images.
        /// </summary>
        public ImageGroupKeyType GroupKey { get; set; } = ImageGroupKeyType.HEADER;

        /// <summary>
        /// The user ID of the person who uploaded the image. This is a UUID formatted string that uniquely identifies the user (aka Scopos Account).
        /// </summary>
        public string UserId { get; set; } = "";

        /// <summary>
        /// The internal S3 key for the image. This is the key used to store and retrieve the image from Amazon S3 storage.
        /// <para>The value is assigned by the PatchImageModeration API call.</para>
        /// </summary>
        public string S3Key { get; set; } = "";

        /// <summary>
        /// The file type of the image. The PostImageModeration API call accepts only JPEG and PNG file types, and may convert other values.
        /// </summary>
        public ImageFileType FileType { get; set; } = ImageFileType.JPEG;

        /// <summary>
        /// Boolean flag indicating whether the image has been resized by the PatchImageModeration API call after it was uploaded.
        /// </summary>
        public bool Resized { get; set; } = false;

        /// <summary>
        /// New, Approved, Rejected, or Flagged.
        /// <para>This value is set by the PatchImageModeration API call after the image is uploaded.</para>
        /// </summary>
        public ImageSafeToShowStatus SafeToShowStatus { get; set; } = ImageSafeToShowStatus.NEW;

        /// <summary>
        /// Notes from the moderation process. This field can contain information about why an image was rejected or flagged, or any other relevant notes from the moderation team.
        /// </summary>
        public string ModerationNotes { get; set; } = "";

        /// <summary>
        /// The date and time when the image was created. This value is assigned when the image is first uploaded by the PostImageModeration API call and is stored in UTC format.
        /// </summary>
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    }
}
