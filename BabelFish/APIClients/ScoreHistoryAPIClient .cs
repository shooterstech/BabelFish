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
    public class ScoreHistoryAPIClient : APIClient {

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="apiKey"></param>
        public ScoreHistoryAPIClient( string apiKey ) : base( apiKey ) { }

        public ScoreHistoryAPIClient( string apiKey, APIStage apiStage ) : base( apiKey, apiStage ) { }


        public async Task<GetScoreHistoryAuthenticatedResponse> GetScoreHistoryAuthenticatedAsync( GetScoreHistoryAuthenticatedRequest requestParameters ) {
            var response = new GetScoreHistoryAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        public async Task<GetScoreAverageAuthenticatedResponse> GetScoreAverageAuthenticatedAsync( GetScoreAverageAuthenticatedRequest requestParameters ) {

            var response = new GetScoreAverageAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
        }


        public async Task<GetScoreHistoryPublicResponse> GetScoreHistoryPublicAsync( GetScoreHistoryPublicRequest requestParameters ) {
            var response = new GetScoreHistoryPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        public async Task<GetScoreAveragePublicResponse> GetScoreAveragePublicAsync( GetScoreAveragePublicRequest requestParameters ) {

            var response = new GetScoreAveragePublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        public async Task<PostScoreHistoryResponse> PostScoreHistoryAsync( PostScoreHistoryRequest requestParameters ) {

            var response = new PostScoreHistoryResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        public async Task<PatchScoreHistoryResponse> PatchScoreHistoryAsync( PatchScoreHistoryRequest requestParameters ) {

            var response = new PatchScoreHistoryResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
        }
        


    }
}
