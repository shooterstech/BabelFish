using System.Net.Http.Headers;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.ScoposData;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.ImageAPI {
    /// <summary>
    /// Request object for uploading images and moderating them, with authentication.
    /// </summary>
    public class UploadImageAuthenticatedRequest : Request {

        private static ImageClient _imageClient = new ImageClient();
        private static HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadImageAuthenticatedRequest"/> class.
        /// </summary>
        /// <param name="credentials">The user credentials for authentication.</param>
        public UploadImageAuthenticatedRequest( UserAuthentication credentials ) : base( "PatchModerateImage", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            RequiresCredentials = true;
            SubDomain = APISubDomain.AUTHAPI;
        }

        /// <summary>
        /// Convenience constructor for uploading a Club image. Sets the value of ImageCategory to Club, and the Key to the (parameter) license number in string form.
        /// </summary>
        /// <param name="licenseNumber">The license number of the club.</param>
        /// <param name="groupKey">The group key for the image.</param>
        /// <param name="credentials">The user credentials for authentication.</param>
        public UploadImageAuthenticatedRequest( int licenseNumber, ImageGroupKeyType groupKey, UserAuthentication credentials ) : base( "PatchModerateImage", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            RequiresCredentials = true;
            SubDomain = APISubDomain.AUTHAPI;

            ImageCategory = ImageCategory.CLUB;
            PrimaryKey = licenseNumber.ToString();
            SubKey = string.Empty; // SubKey is not used for Match images, so it should be left empty.
            GroupKey = groupKey;
        }

        /// <summary>
        /// Convenience constructor for uploading a User image. Sets the value of ImageCategory to User, and the Key to the (parameter) user ID in string form.
        /// </summary>
        /// <param name="userId">The UUID formatted user ID of the user who owns the image.</param>
        /// <param name="groupKey">The group key for the image.</param>
        /// <param name="credentials">The user credentials for authentication.</param>
        public UploadImageAuthenticatedRequest( string userId, ImageGroupKeyType groupKey, UserAuthentication credentials ) : base( "PatchModerateImage", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            RequiresCredentials = true;
            SubDomain = APISubDomain.AUTHAPI;

            ImageCategory = ImageCategory.USER;
            PrimaryKey = userId;
            SubKey = string.Empty; // SubKey is not used for Match images, so it should be left empty.
            GroupKey = groupKey;
        }

        /// <summary>
        /// Convenience constructor for uploading a Match image. Sets the value of ImageCategory to Match, and the Key to the (parameter) match ID in string form.
        /// </summary>
        /// <param name="matchId"></param>
        /// <param name="groupKey">The group key for the image.</param>
        /// <param name="credentials"></param>
        public UploadImageAuthenticatedRequest( MatchID matchId, ImageGroupKeyType groupKey, UserAuthentication credentials ) : base( "PatchModerateImage", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            RequiresCredentials = true;
            SubDomain = APISubDomain.AUTHAPI;

            ImageCategory = ImageCategory.MATCH;
            PrimaryKey = matchId.ToString();
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
        public UploadImageAuthenticatedRequest( MatchID leagueId, int teamId, ImageGroupKeyType groupKey, UserAuthentication credentials ) : base( "PatchModerateImage", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            RequiresCredentials = true;
            SubDomain = APISubDomain.AUTHAPI;

            if (groupKey == ImageGroupKeyType.BULK) {
                throw new ArgumentException( $"The Bulk group key is not supported for uploading images for League teams. Please use Header or Profile as the group key for League images." );
            }

            ImageCategory = ImageCategory.LEAGUE_TEAM;
            PrimaryKey = leagueId.ToString();
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
        public UploadImageAuthenticatedRequest( MatchID leagueId, MatchID gameId, UserAuthentication credentials ) : base( "PatchModerateImage", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            RequiresCredentials = true;
            SubDomain = APISubDomain.AUTHAPI;

            ImageCategory = ImageCategory.LEAGUE_GAME;
            PrimaryKey = leagueId.ToString();
            SubKey = gameId.ToString();
            GroupKey = ImageGroupKeyType.BULK;
        }

        /// <summary>
        /// References the image file to upload. The file should be in a supported image format (JPEG or PNG). The file path should be valid and accessible by the application.
        /// </summary>
        public FileInfo? ImageFile { get; set; } = null;

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
        public string PrimaryKey { get; set; } = "";

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

        internal string S3Key { get; set; } = "";

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
                if (string.IsNullOrWhiteSpace( S3Key )) {
                    throw new APIRequestParameterException( $"The S3Key property must be set to a valid S3 key for the uploaded image. Usually this value is returned by the GetPresignedUrlRequest." );
                }

                return new Dictionary<string, List<string>>() {
                    { "caption", new List<string>() { Caption } },
                    { "alt-text", new List<string>() { AltText } },
                    { "image-category", new List<string>() { ImageCategory.Description() } },
                    { "primary-key", new List<string>() { PrimaryKey } },
                    { "sub-key", new List<string>() { SubKey } },
                    { "group-key", new List<string>() { GroupKey.Description() } },
                    { "s3-key", new List<string>() { S3Key} }
                };
            }
        }

        public override async Task PreRequestMethodAsync() {

            // Check that the ImageFile is valid and exists.
            if (ImageFile == null || !ImageFile.Exists) {
                throw new APIRequestParameterException( $"The ImageFile property must be set to a valid file path that exists. Currently: {ImageFile?.FullName}" );
            }

            // Check that the ImageFile is of a supported type (JPEG or PNG).
            string extension = ImageFile.Extension?.ToLower() ?? string.Empty;
            if (extension != ".jpg" && extension != ".jpeg" && extension != ".png") {
                throw new APIRequestParameterException( $"The ImageFile must be a JPEG or PNG file. Currently: {ImageFile?.FullName}" );
            }

            byte[] bytes = File.ReadAllBytes( ImageFile.FullName );
            if (bytes == null || bytes.Length == 0) {
                throw new APIRequestParameterException( $"The ImageFile is seemingly empty. Currently: {ImageFile?.FullName}" );
            }

            var presignedUrlRequest = new GetPresignedUrlRequest( ImageFile, this.Credentials );
            var presignedUrlResponse = await _imageClient.GetPresignedUrlAsync( presignedUrlRequest );
            if (!presignedUrlResponse.HasOkStatusCode) {
                throw new APIRequestParameterException( $"Failed to get a presigned URL for the image upload. Status code: {presignedUrlResponse.OverallStatusCode}, Message: {presignedUrlResponse.ExceptionMessage}" );
            }

            string presignedUrl = presignedUrlResponse.PresignedUrl.Url;
            this.S3Key = presignedUrlResponse.PresignedUrl.S3Key;

            // Create the content
            using var content = new ByteArrayContent( bytes );

            // Set the content type 
            if (extension == ".png")
                content.Headers.ContentType = new MediaTypeHeaderValue( "image/png" );
            else
                content.Headers.ContentType = new MediaTypeHeaderValue( "image/jpeg" );

            // PUT request to the presigned URL
            HttpResponseMessage response = await _httpClient.PutAsync( presignedUrl, content );

            if (!response.IsSuccessStatusCode) {
                throw new APIRequestParameterException( $"Failed to upload the image. Status code: {response.StatusCode}, Message: {response.ReasonPhrase}" );
            }

            return;
        }
    }
}
