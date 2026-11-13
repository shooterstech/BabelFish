using System.Globalization;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.NewsfeedAPI {
    /// <summary>Creates a personal post or a post for an authorized club.</summary>
    public class CreateNewsfeedPostAuthenticatedRequest : Request {
        /// <summary>Constructs a request using the logged-in user's credentials.</summary>
        public CreateNewsfeedPostAuthenticatedRequest( UserAuthentication credentials ) : base( "CreateNewsfeedPost", credentials ) {
            HttpMethod = HttpMethod.Post;
            IgnoreInMemoryCache = true;
            IgnoreFileSystemCache = true;
        }

        /// <summary>Constructs a post; omit the license number for a personal post.</summary>
        public CreateNewsfeedPostAuthenticatedRequest( string bodyText, UserAuthentication credentials, int? licenseNumber = null ) : this( credentials ) {
            BodyText = bodyText;
            LicenseNumber = licenseNumber;
        }

        /// <summary>Required nonblank text, at most 65,535 UTF-8 bytes. Stored verbatim.</summary>
        public string BodyText { get; set; } = string.Empty;

        /// <summary>Optional positive club license. Club permissions are checked by the server.</summary>
        public int? LicenseNumber { get; set; }

        /// <inheritdoc />
        public override string RelativePath => "/newsfeed/posts";

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                var parameters = new Dictionary<string, List<string>>();
                if (LicenseNumber.HasValue) {
                    if (LicenseNumber.Value <= 0)
                        throw new APIRequestParameterException( "LicenseNumber must be a positive integer." );
                    parameters["license-number"] = new List<string> { LicenseNumber.Value.ToString( CultureInfo.InvariantCulture ) };
                }
                return parameters;
            }
        }

        private void ValidateText() {
            if (string.IsNullOrWhiteSpace( BodyText ))
                throw new APIRequestParameterException( "BodyText must not be blank." );
            try {
                if (new UTF8Encoding( false, true ).GetByteCount( BodyText ) > 65535)
                    throw new APIRequestParameterException( "BodyText must fit within 65,535 UTF-8 bytes." );
            } catch (EncoderFallbackException) {
                throw new APIRequestParameterException( "BodyText must be valid UTF-8 text." );
            }
        }

        /// <inheritdoc />
        public override StringContent PostParameters {
            get {
                ValidateText();
                var body = new Dictionary<string, string> { ["body-text"] = BodyText };
                var json = G_NS.JsonConvert.SerializeObject( body, SerializerOptions.NewtonsoftJsonSerializer );
                return new StringContent( json, Encoding.UTF8, "application/json" );
            }
        }

        /// <inheritdoc />
        public override Task PreRequestMethodAsync() {
            // Reject invalid text before the shared pipeline refreshes user credentials.
            ValidateText();
            _ = QueryParameters;
            return Task.CompletedTask;
        }
    }
}
