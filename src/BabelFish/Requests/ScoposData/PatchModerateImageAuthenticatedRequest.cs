using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.ScoposData;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.ScoposData {
    /// <summary>
    /// Request object for moderating images with authentication.
    /// </summary>
    public class PatchModerateImageAuthenticatedRequest : Request {

        /// <summary>
        /// Initializes a new instance of the <see cref="PatchModerateImageAuthenticatedRequest"/> class.
        /// </summary>
        /// <param name="credentials">The user credentials for authentication.</param>
        public PatchModerateImageAuthenticatedRequest( UserAuthentication credentials ) : base( "PatchModerateImage", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            RequiresCredentials = true;
            SubDomain = APISubDomain.INTERNAL;
        }

        /// <summary>
        /// Convenience constructor for uploading a Club image. Sets the value of ImageCategory to Club, and the Key to the (parameter) license number in string form.
        /// </summary>
        /// <param name="licenseNumber">The license number of the club.</param>
        /// <param name="groupKey">The group key for the image.</param>
        /// <param name="credentials">The user credentials for authentication.</param>
        public PatchModerateImageAuthenticatedRequest( int licenseNumber, ImageGroupKeyType groupKey, UserAuthentication credentials ) : base( "PatchModerateImage", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            RequiresCredentials = true;
            SubDomain = APISubDomain.INTERNAL;

            ImageCategory = ImageCategory.CLUB;
            Key = licenseNumber.ToString();
            SubKey = string.Empty; // SubKey is not used for Match images, so it should be left empty.
            GroupKey = groupKey;
        }

        /// <summary>
        /// Convenience constructor for uploading a User image. Sets the value of ImageCategory to User, and the Key to the (parameter) user ID in string form.
        /// </summary>
        /// <param name="userId">The UUID formatted user ID of the user who owns the image.</param>
        /// <param name="groupKey">The group key for the image.</param>
        /// <param name="credentials">The user credentials for authentication.</param>
        public PatchModerateImageAuthenticatedRequest( string userId, ImageGroupKeyType groupKey, UserAuthentication credentials ) : base( "PatchModerateImage", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            RequiresCredentials = true;
            SubDomain = APISubDomain.INTERNAL;

            ImageCategory = ImageCategory.USER;
            Key = userId;
            SubKey = string.Empty; // SubKey is not used for Match images, so it should be left empty.
            GroupKey = groupKey;
        }

        /// <summary>
        /// Convenience constructor for uploading a Match image. Sets the value of ImageCategory to Match, and the Key to the (parameter) match ID in string form.
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="groupKey">The group key for the image.</param>
        /// <param name="credentials"></param>
        public PatchModerateImageAuthenticatedRequest( MatchID matchId, ImageGroupKeyType groupKey, UserAuthentication credentials ) : base( "PatchModerateImage", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            RequiresCredentials = true;
            SubDomain = APISubDomain.INTERNAL;

            ImageCategory = ImageCategory.MATCH;
            Key = matchId.ToString();
            SubKey = string.Empty; // SubKey is not used for Match images, so it should be left empty.
            GroupKey = groupKey;
        }

        /// <summary>
        /// Convenience constructor for uploading a League team image. Sets the value of ImageCategory to LEAGUE, and the Key to the (parameter)
        /// league ID in string form, and the SubKey to the (parameter) team ID in string form.
        /// </summary>
        /// <param name="leagueId">The ID of the league.</param>
        /// <param name="teamId">The ID of the team.</param>
        /// <param name="groupKey">The group key for the image.</param>
        /// <param name="credentials">The user credentials for authentication.</param>
        /// <exception cref="ArgumentException">Thrown when the Bulk group key is used for a League team image.</exception>
        public PatchModerateImageAuthenticatedRequest( MatchID leagueId, int teamId, ImageGroupKeyType groupKey, UserAuthentication credentials ) : base( "PatchModerateImage", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            RequiresCredentials = true;
            SubDomain = APISubDomain.INTERNAL;

            if (groupKey == ImageGroupKeyType.BULK) {
                throw new ArgumentException( $"The Bulk group key is not supported for uploading images for League teams. Please use Header or Profile as the group key for League images." );
            }

            ImageCategory = ImageCategory.LEAGUE;
            Key = leagueId.ToString();
            SubKey = teamId.ToString();
            GroupKey = groupKey;
        }

        /// <summary>
        /// Convenience constructor for uploading a League game image. Sets the value of ImageCategory to LEAGUE, GroupKey to Bulk, the Key to the (parameter)
        /// league ID in string form, and the SubKey to the (parameter) game ID in string form.
        /// </summary>
        /// <param name="leagueId">The Match ID of the league.</param>
        /// <param name="gameId">The Match ID of the game.</param>
        /// <param name="credentials">The user credentials for authentication.</param>
        public PatchModerateImageAuthenticatedRequest( MatchID leagueId, MatchID gameId, UserAuthentication credentials ) : base( "PatchModerateImage", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            RequiresCredentials = true;
            SubDomain = APISubDomain.INTERNAL;

            ImageCategory = ImageCategory.LEAGUE;
            Key = leagueId.ToString();
            SubKey = gameId.ToString();
            GroupKey = ImageGroupKeyType.BULK;
        }

        /// <summary>
        /// Gets or sets the image bytes.
        /// </summary>
        public byte[] ImageBytes { get; set; } = Array.Empty<byte>();

        /// <summary>
        /// Gets the content type based on the file type.
        /// </summary>
        public string ContentType {
            get {

                return FileType switch {
                    ImageFileType.JPEG => "image/jpeg",
                    ImageFileType.PNG => "image/png",
                    _ => "application/octet-stream"
                };
            }
        }

        /// <summary>
        /// Gets or sets the type of the image file.
        /// </summary>
        public ImageFileType FileType { get; set; } = ImageFileType.JPEG;

        /// <summary>
        /// Gets or sets the category of the image.
        /// </summary>
        public ImageCategory ImageCategory { get; set; } = ImageCategory.CLUB;

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

        /// <inheritdoc />
        public override string RelativePath {
            get { return "/image/moderate"; }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                return new Dictionary<string, List<string>>() {
                    { "caption", new List<string>() { Caption } },
                    { "alt-text", new List<string>() { AltText } },
                    { "file-type", new List<string>() { FileType.Description() } },
                    { "image-category", new List<string>() { ImageCategory.Description() } },
                    { "primary-key", new List<string>() { Key } },
                    { "sub-key", new List<string>() { SubKey } },
                    { "group-key", new List<string>() { GroupKey.Description() } }
                };
            }
        }

        /// <inheritdoc />
        public override HttpContent PostContent {
            get {
                var content = new ByteArrayContent( ImageBytes );
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue( ContentType );
                return content;
            }
        }
    }
}
