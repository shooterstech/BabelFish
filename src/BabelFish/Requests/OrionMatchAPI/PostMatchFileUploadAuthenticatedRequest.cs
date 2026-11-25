using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class PostMatchFileUploadAuthenticatedRequest : Request {

        internal APIClientEnums ApiStage { get; set; } = APIClientEnums.PRODUCTION;
        private static HttpClient _httpClient = new HttpClient();

        public PostMatchFileUploadAuthenticatedRequest( UserAuthentication credentials, IMatchObjectFile objectToUpload ) : base( "PostMatchFileUpload", credentials ) {
            // Check that the object to upload is not null
            if (objectToUpload is null) {
                throw new ArgumentNullException( nameof( objectToUpload ), "The object to upload cannot be null." );
            }

            // Check that the object has been saved to a file and that the file exists
            var relativePath = objectToUpload.GetRelativePath();
            if (string.IsNullOrWhiteSpace( relativePath ) || !File.Exists( relativePath )) {
                throw new FileNotFoundException( $"The file to upload does not exist. Relative path provided: {relativePath}" );
            }

            HttpMethod = new HttpMethod( "POST" );
            RequiresCredentials = true;
            SubDomain = APISubDomain.AUTHAPI;
            ObjectToUpload = objectToUpload;
        }

        /// <summary>
        /// The match file that needs to be uploaded. These files should all implement the <see cref="ISaveToFile"/> interface, which includes
        /// the path and file names to the file.
        /// </summary>
        public IMatchObjectFile ObjectToUpload { get; private set; }

        /// <inheritdoc />
        public override string RelativePath {
            get {
                return $"/match/{ObjectToUpload.MatchID}";
            }
        }
    }
}
