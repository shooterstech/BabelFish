using System.Net.Http.Headers;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.ImageAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class UploadMatchFileAuthenticatedRequest : Request {

        internal APIClientEnums ApiStage { get; set; } = APIClientEnums.PRODUCTION;
        private static HttpClient _httpClient = new HttpClient();

        public UploadMatchFileAuthenticatedRequest( UserAuthentication credentials, IMatchObjectFile objectToUpload, DirectoryInfo projectDirectory ) : base( "PostMatchFileUpload", credentials ) {
            // Check that the object to upload is not null
            if (objectToUpload is null) {
                throw new ArgumentNullException( nameof( objectToUpload ), "The object to upload cannot be null." );
            }


            // Check that the object has been saved to a file and that the file exists
            var relativePath = objectToUpload.GetRelativePath();
            var fullPath = Path.Combine( projectDirectory.FullName, relativePath );
            if (string.IsNullOrWhiteSpace( relativePath ) || !File.Exists( fullPath )) {
                throw new FileNotFoundException( $"The file to upload does not exist. Path provided: {fullPath}" );
            }

            HttpMethod = new HttpMethod( "POST" );
            RequiresCredentials = true;
            SubDomain = APISubDomain.AUTHAPI;
            ObjectToUpload = objectToUpload;
            ProjectDirectory = projectDirectory;
        }

        /// <summary>
        /// The match file that needs to be uploaded. These files should all implement the <see cref="ISaveToFile"/> interface, which includes
        /// the path and file names to the file.
        /// </summary>
        public IMatchObjectFile ObjectToUpload { get; private set; }

        internal string S3Key { get; set; } = "";

        internal DirectoryInfo ProjectDirectory { get; set; }

        /// <inheritdoc />
        public override string RelativePath {
            get {
                return $"/match/{ObjectToUpload.MatchID}";
            }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                if (string.IsNullOrWhiteSpace( S3Key )) {
                    throw new APIRequestParameterException( $"The S3Key property must be set to a valid S3 key for the uploaded image. Usually this value is returned by the GetPresignedUrlRequest." );
                }

                return new Dictionary<string, List<string>>() {
                    { "relative-file-name", new List<string>() { ObjectToUpload.GetRelativePath() } },
                    { "match-id", new List<string>() { ObjectToUpload.MatchID.ToString() } },
                    { "s3-key", new List<string>() { S3Key} }
                };
            }
        }

        /// <inheritdoc />
        public override async Task PreRequestMethodAsync() {

            var fileToUploadPath = Path.Combine( ProjectDirectory.FullName, ObjectToUpload.GetRelativePath() );
            var fileInfo = new FileInfo( fileToUploadPath );
            // Check that the file to upload is valid and exists. While we did check this in the constructor, we need to check again here in case the file was deleted or moved after the request was created.
            if (!fileInfo.Exists) {
                throw new APIRequestParameterException( $"The file to upload must be set to a valid file path that exists. Currently: {fileInfo?.FullName}" );
            }

            // Check that the file to upload is of a supported type (JSON, JSONL, or ORION). NOTE a .orion file is a json formatted file.
            string extension = fileInfo.Extension?.ToLower() ?? string.Empty;
            if (extension != ".json" && extension != ".jsonl" && extension != ".orion") {
                throw new APIRequestParameterException( $"The match file to upload must be a JSON, JSONL, or ORION file. Currently: {fileInfo?.FullName}" );
            }

            byte[] bytes = File.ReadAllBytes( fileToUploadPath );
            if (bytes == null || bytes.Length == 0) {
                throw new APIRequestParameterException( $"The file to upload is seemingly empty. Currently: {fileToUploadPath}" );
            }

            var presignedUrlRequest = new GetPresignedUrlRequest( fileInfo, ObjectToUpload.MatchID, this.Credentials );
            var presignedUrlResponse = await new ImageClient( ApiStage ).GetPresignedUrlAsync( presignedUrlRequest );
            if (!presignedUrlResponse.HasOkStatusCode) {
                throw new APIRequestParameterException( $"Failed to get a presigned URL for the file upload. Status code: {presignedUrlResponse.OverallStatusCode}, Message: {presignedUrlResponse.ExceptionMessage}" );
            }

            string presignedUrl = presignedUrlResponse.PresignedUrl.Url;
            this.S3Key = presignedUrlResponse.PresignedUrl.S3Key;

            // Create the content
            using var content = new ByteArrayContent( bytes );

            // Set the content type, since the files are all JSON formatted, we will set the content type to application/json. This is important for the server to correctly interpret the uploaded file.
            content.Headers.ContentType = new MediaTypeHeaderValue( "application/json" );

            // PUT request to the presigned URL
            HttpResponseMessage response = await _httpClient.PutAsync( presignedUrl, content );

            if (!response.IsSuccessStatusCode) {
                throw new APIRequestParameterException( $"Failed to upload the file. Status code: {response.StatusCode}, Message: {response.ReasonPhrase}" );
            }
        }
    }
}
