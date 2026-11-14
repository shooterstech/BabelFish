using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.ImageAPI {
    internal class GetPresignedUrlRequest : Request {

        internal GetPresignedUrlRequest( UserAuthentication credentials ) : base( "GetPresignedUrl", credentials ) {
            HttpMethod = new HttpMethod( "GET" );
            RequiresCredentials = true;
            SubDomain = APIClients.APISubDomain.AUTHAPI;

            // We dont' want caching for this request, since the presigned URL is only valid for a short time.
            IgnoreFileSystemCache = true;
            IgnoreInMemoryCache = true;
        }

        internal GetPresignedUrlRequest( FileInfo imageFile, UserAuthentication credentials ) : this( credentials ) {
            HttpMethod = new HttpMethod( "GET" );
            RequiresCredentials = true;
            SubDomain = APIClients.APISubDomain.AUTHAPI;

            // We dont' want caching for this request, since the presigned URL is only valid for a short time.
            IgnoreFileSystemCache = true;
            IgnoreInMemoryCache = true;

            ImageFile = imageFile;
        }

        public override string RelativePath {
            get { return "/image/presign-url"; }
        }

        /// <summary>
        /// References the image file to upload. The file should be in a supported image format (JPEG or PNG). The file path should be valid and accessible by the application.
        /// </summary>
        public FileInfo? ImageFile { get; set; } = null;

        public override Dictionary<string, List<string>> QueryParameters {
            get {

                // Check that the ImageFile is valid and exists.
                if (ImageFile == null || !ImageFile.Exists) {
                    throw new APIRequestParameterException( $"The ImageFile property must be set to a valid file path that exists. Currently: {ImageFile?.FullName}" );
                }

                return new Dictionary<string, List<string>>() {
                    { "filename", new List<string>() { ImageFile.Name } }
                };
            }
        }
    }
}
