using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;
using Scopos.BabelFish.Responses.ScoreHistoryAPI;
using Scopos.BabelFish.Runtime;

namespace Scopos.BabelFish.APIClients {
    public class ScoreHistoryAPIClient : APIClient<ScoreHistoryAPIClient> {

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="apiKey"></param>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        public ScoreHistoryAPIClient() : base() {

            //ScoreHistoryAPIClient does not support file system cache
            LocalStoreDirectory = null;

            //EKA NOTE 8/2024: We have experienced problems with using file system caching with score history. Turning off for now, until we can learn more about it.
            IgnoreFileSystemCache = false;

            //We do want (some) in memory cache. 
            IgnoreInMemoryCache = false;
        }

        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        public ScoreHistoryAPIClient( APIStage apiStage ) : base( apiStage ) {

            //ScoreHistoryAPIClient does not support file system cache
            LocalStoreDirectory = null;

			//EKA NOTE 8/2024: We have experienced problems with using file system caching with score history. Turning off for now, until we can learn more about it.
			IgnoreFileSystemCache = false;

			//We do want (some) in memory cache. 
			IgnoreInMemoryCache = false;
        }


        public async Task<GetScoreHistoryAuthenticatedResponse> GetScoreHistoryAuthenticatedAsync( GetScoreHistoryAuthenticatedRequest requestParameters ) {
            var response = new GetScoreHistoryAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
		}


		public async Task<GetScoreHistoryPublicResponse> GetScoreHistoryPublicAsync( GetScoreHistoryPublicRequest requestParameters ) {
			var response = new GetScoreHistoryPublicResponse( requestParameters );

			await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

			return response;
		}

		public async Task<GetScoreHistoryAbstractResponse> GetScoreHistoryAsync( GetScoreHistoryAbstractRequest requestParameters ) {
            if (requestParameters is GetScoreHistoryPublicRequest)
                return await GetScoreHistoryPublicAsync( (GetScoreHistoryPublicRequest)requestParameters );
            else if (requestParameters is GetScoreHistoryAuthenticatedRequest)
                return await GetScoreHistoryAuthenticatedAsync( (GetScoreHistoryAuthenticatedRequest)requestParameters );
            else
                throw new ArgumentException( $"Passed in argument requestParameters is of unexpected type ${requestParameters.GetType()}.");
		}

		public async Task<GetScoreAverageAuthenticatedResponse> GetScoreAverageAuthenticatedAsync( GetScoreAverageAuthenticatedRequest requestParameters ) {

            var response = new GetScoreAverageAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

		public async Task<GetScoreAveragePublicResponse> GetScoreAveragePublicAsync( GetScoreAveragePublicRequest requestParameters ) {

            var response = new GetScoreAveragePublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
		}

		public async Task<GetScoreAverageAbstractResponse> GetScoreAverageAsync( GetScoreHistoryAbstractRequest requestParameters ) {
			if (requestParameters is GetScoreAveragePublicRequest)
				return await GetScoreAveragePublicAsync( (GetScoreAveragePublicRequest)requestParameters );
			else if (requestParameters is GetScoreAverageAuthenticatedRequest)
				return await GetScoreAverageAuthenticatedAsync( (GetScoreAverageAuthenticatedRequest)requestParameters );
			else
				throw new ArgumentException( $"Passed in argument requestParameters is of unexpected type ${requestParameters.GetType()}." );
		}

		/// <summary>
		/// Allows a user to manually enter a new score to be included in score history related calculations. User-entered 
		/// score history will always have visibility protected.
		/// NOTE: To edit an existing score use .PatchScoreHistoryAsync()
		/// </summary>
		/// <param name="requestParameters"></param>
		/// <returns></returns>
		public async Task<PostScoreHistoryResponse> PostScoreHistoryAsync( PostScoreHistoryRequest requestParameters ) {

            var response = new PostScoreHistoryResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

		/// <summary>
		/// Allows a user to modify an existing manually entered score.
        /// NOTE: To create a new score use .PostScoreHistoryAsync()
		/// </summary>
		/// <param name="requestParameters"></param>
		/// <returns></returns>
		public async Task<PatchScoreHistoryResponse> PatchScoreHistoryAsync( PatchScoreHistoryRequest requestParameters ) {

            var response = new PatchScoreHistoryResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

		/// <summary>
		/// Removes the caller's User Id from the score identified by the passed in Result COF ID. This does not 
        /// delete the score from a match, but rather only removes the link between the user Id and Result COF ID.
		/// </summary>
		/// <param name="requestParameters"></param>
		/// <returns></returns>
		public async Task<DeleteScoreHistoryResponse> DeleteScoreHistoryAsync(DeleteScoreHistoryRequest requestParameters)
        {

            var response = new DeleteScoreHistoryResponse(requestParameters);

            await this.CallAPIAsync(requestParameters, response).ConfigureAwait(false);

            return response;
        }



    }
}
