using Scopos.BabelFish.Requests.ImageAPI;
using Scopos.BabelFish.Responses.ImageAPI;

namespace Scopos.BabelFish.APIClients {
    public class ImageClient : APIClient<ImageClient> {

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        public ImageClient() : base() {

            //ScoposDataClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;

            //We do want in memory cache
            IgnoreInMemoryCache = false;
        }

        /// <summary>
        /// Constructor for the ImageClient class, that allows specifying the API stage (production, staging, etc.) to use.
        /// </summary>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        public ImageClient( APIStage apiStage ) : base( apiStage ) {

            //ScoposDataClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;

            //We do want in memory cache
            IgnoreInMemoryCache = false;
        }

        /// <summary>
        /// Calls the internal image moderation API request.
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns></returns>
        public async Task<UploadImageAuthenticatedResponse> UploadImageAuthenticatedAsync( UploadImageAuthenticatedRequest requestParameters ) {

            UploadImageAuthenticatedResponse response = new UploadImageAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        internal async Task<GetPresignedUrlResponse> GetPresignedUrlAsync( GetPresignedUrlRequest requestParameters ) {
            GetPresignedUrlResponse response = new GetPresignedUrlResponse( requestParameters );
            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );
            return response;
        }

        /// <summary>
        /// Calls the GetImages API request to retrieve images for a Match, League Team, Club, or athlete. This is a public API and does not require authentication.
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns></returns>
        public async Task<GetImagesResponse> GetImagesPublicAsync( GetImagesRequest requestParameters ) {

            GetImagesResponse response = new GetImagesResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
        }
    }
}
