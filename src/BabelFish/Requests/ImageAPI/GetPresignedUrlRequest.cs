using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.ImageAPI {
    internal class GetPresignedUrlRequest : Request {

        /// <summary>
        /// Private constructor for the GetPresignedUrlRequest class. This constructor is used internally to set up the request with the necessary parameters.
        /// </summary>
        /// <remarks>Uses HTTP GET, requires credentials, targets the AUTHAPI subdomain, and bypasses file
        /// system and in-memory caching because presigned URLs are short-lived.</remarks>
        /// <param name="credentials">Authentication credentials used to authorize the request.</param>
        private GetPresignedUrlRequest( UserAuthentication credentials, FileInfo file ) : base( "GetPresignedUrl", credentials ) {
            HttpMethod = new HttpMethod( "GET" );
            RequiresCredentials = true;
            SubDomain = APIClients.APISubDomain.AUTHAPI;

            if (file == null || !file.Exists) {
                throw new APIRequestParameterException( $"The file parameter must be a valid file path that exists. Provided: {file?.FullName}" );
            }

            this.File = file;

            // We dont' want caching for this request, since the presigned URL is only valid for a short time.
            IgnoreFileSystemCache = true;
            IgnoreInMemoryCache = true;
        }

        /// <summary>
        /// Initializes a new instance of the request for generating a presigned URL to upload an image file.
        /// </summary>
        /// <remarks>Sets the upload type to image and assigns the provided file.</remarks>
        /// <param name="imageFile">Image file to upload.</param>
        /// <param name="credentials">Authentication credentials used to authorize the request.</param>
        internal GetPresignedUrlRequest( FileInfo imageFile, UserAuthentication credentials ) : this( credentials, imageFile ) {

            // the imageFile must have an extension that is either .jpg, .jpeg, or .png
            var exstension = this.File.Extension.ToLower();
            if (exstension != ".jpg" && exstension != ".jpeg" && exstension != ".png") {
                throw new APIRequestParameterException( $"The image file must have an extension of .jpg, .jpeg, or .png. Provided file: {this.File.FullName}" );
            }

            this.ImageUploadType = ImageUploadType.IMAGE;
        }

        /// <summary>
        /// Initializes a request for a presigned URL to upload a match project file.
        /// </summary>
        /// <remarks>Sets the upload type to match project and associates the request with the specified
        /// match.</remarks>
        /// <param name="matchProjectFile">Match project file to upload.</param>
        /// <param name="matchId">Identifier of the match associated with the upload.</param>
        /// <param name="credentials">Authentication information for the request.</param>
        internal GetPresignedUrlRequest( FileInfo matchProjectFile, MatchID matchId, UserAuthentication credentials ) : this( credentials, matchProjectFile ) {
            if (matchId == null || matchId == MatchID.DEFAULT) {
                throw new APIRequestParameterException( "A valid MatchID must be provided for a match project upload." );
            }

            // The matchProjectFile must have an extension that is either .json or .jsonl
            var extension = this.File.Extension.ToLower();
            if (extension != ".json" && extension != ".jsonl" && extension != ".orion") {
                throw new APIRequestParameterException( $"The match project file must have an extension of .json, .jsonl, or .orion. Provided file: {this.File.FullName}" );
            }

            this.ImageUploadType = ImageUploadType.MATCH_PROJECT;
            this.MatchId = matchId;
        }

        /// <summary>
        /// Initializes a request for a presigned URL to upload a Reconfigurable Rulebook Definition file.
        /// </summary>
        /// <param name="definitionFile">Definition file to upload.</param>
        /// <param name="definitionType">Definition category associated with the image upload.</param>
        /// <param name="credentials">Authenticated user credentials for the request.</param>
        internal GetPresignedUrlRequest( FileInfo definitionFile, DefinitionType definitionType, UserAuthentication credentials ) : this( credentials, definitionFile ) {

            // The definitionFile must have an extension that is either .json
            var extension = this.File.Extension.ToLower();
            if (extension != ".json") {
                throw new APIRequestParameterException( $"The definition file must have an extension of .json . Provided file: {this.File.FullName}" );
            }

            this.ImageUploadType = ImageUploadType.DEFINITION;
            this.DefinitionType = definitionType;
        }

        public override string RelativePath {
            get { return "/image/presign-url"; }
        }

        private ImageUploadType ImageUploadType { get; set; } = ImageUploadType.IMAGE;
        private MatchID MatchId { get; set; } = MatchID.DEFAULT;
        private DefinitionType DefinitionType { get; set; } = DefinitionType.ATTRIBUTE;

        /// <summary>
        /// References the image file to upload. The file should be in a supported image format (JPEG or PNG). The file path should be valid and accessible by the application.
        /// Value is set by the constructor based on the type of upload (image, match project, or definition).
        /// </summary>
        private FileInfo File { get; set; }

        public override Dictionary<string, List<string>> QueryParameters {
            get {

                // Check that the File is valid and exists.
                if (File == null || !File.Exists) {
                    throw new APIRequestParameterException( $"The File property must be set to a valid file path that exists. Currently: {File?.FullName}" );
                }

                switch (ImageUploadType) {
                    case ImageUploadType.IMAGE:
                        return new Dictionary<string, List<string>>() {
                            { "filename", new List<string>() { File.Name } },
                            { "upload-file-type", new List<string>() { "image" } }
                        };
                    case ImageUploadType.MATCH_PROJECT:
                        return new Dictionary<string, List<string>>() {
                            { "filename", new List<string>() { File.Name } },
                            { "upload-file-type", new List<string>() { "match" } },
                            { "match-id", new List<string>() { MatchId.ToString() } }
                        };
                    case ImageUploadType.DEFINITION:
                        return new Dictionary<string, List<string>>() {
                            { "filename", new List<string>() { File.Name } },
                            { "upload-file-type", new List<string>() { "definition" } },
                            { "definition-type", new List<string>() { DefinitionType.Description() } }
                        };
                    default:
                        throw new APIRequestParameterException( $"Unsupported ImageUploadType: {ImageUploadType}" );
                }
            }
        }
    }
}
