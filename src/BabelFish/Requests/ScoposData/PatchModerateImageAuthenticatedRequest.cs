using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.ScoposData {
    public class PatchModerateImageAuthenticatedRequest : Request {

        public PatchModerateImageAuthenticatedRequest( UserAuthentication credentials ) : base( "PatchModerateImage", credentials ) {
            HttpMethod = new HttpMethod( "PATCH" );
            SubDomain = APISubDomain.INTERNAL;
        }

        public PatchModerateImageAuthenticatedRequest( UserAuthentication credentials, string s3Key ) : this( credentials ) {
            S3Key = s3Key;
        }

        /// <summary>
        /// The S3 key of the image to resize and moderate.
        /// </summary>
        public string S3Key { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string RelativePath {
            get { return "/image/moderate"; }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                if (string.IsNullOrWhiteSpace( S3Key )) {
                    throw new ArgumentNullException( nameof( S3Key ), "The image S3 key must be set to moderate an image." );
                }

                return new Dictionary<string, List<string>>() {
                    { "s3-key", new List<string>() { S3Key } }
                };
            }
        }
    }
}
