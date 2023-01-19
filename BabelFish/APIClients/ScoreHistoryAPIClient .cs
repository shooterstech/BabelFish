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
        public ScoreHistoryAPIClient( string apiKey) : base(apiKey) { }

        public ScoreHistoryAPIClient( string apiKey, APIStage apiStage ) : base( apiKey, apiStage ) { }


        public async Task<GetScoreHistoryResponse> GetScoreHistoryAsync(GetScoreHistoryRequest requestParameters)
        {
            var response = new GetScoreHistoryResponse( requestParameters );

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }

        public async Task<GetScoreAverageResponse> GetScoreAverageAsync( GetScoreAverageRequest requestParameters ) {

            var response = new GetScoreAverageResponse( requestParameters );

            await this.CallAPI( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

    }
}
