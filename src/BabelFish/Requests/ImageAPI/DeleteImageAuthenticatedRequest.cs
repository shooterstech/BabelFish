using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.ImageAPI {
    /// <summary>
    /// Deletes an uploaded image and its stored file. Authorization is checked by the server.
    /// </summary>
    public class DeleteImageAuthenticatedRequest : Request {
        /// <summary>
        /// Constructs an authenticated image deletion request.
        /// </summary>
        /// <param name="credentials">The logged-in user's credentials.</param>
        public DeleteImageAuthenticatedRequest( UserAuthentication credentials ) : base( "DeleteImage", credentials ) {
            HttpMethod = HttpMethod.Delete;
            SubDomain = APISubDomain.AUTHAPI;
            IgnoreInMemoryCache = true;
            IgnoreFileSystemCache = true;
        }

        /// <summary>
        /// Constructs a deletion request for the uploaded image's UUID.
        /// </summary>
        /// <param name="imageId">The uploaded image's UUID.</param>
        /// <param name="credentials">The logged-in user's credentials.</param>
        public DeleteImageAuthenticatedRequest( string imageId, UserAuthentication credentials ) : this( credentials ) {
            ImageId = imageId;
        }

        /// <summary>
        /// The uploaded image's UUID, required before sending the request.
        /// </summary>
        public string ImageId { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string RelativePath {
            get {
                if (!Guid.TryParse( ImageId, out var imageId ))
                    throw new APIRequestParameterException( "ImageId must be a valid UUID." );
                return $"/image/{imageId:D}";
            }
        }
    }
}
