using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using ShootersTech.BabelFish.Responses;
using ShootersTech.BabelFish.DataModel.ScoreHistory;
using ShootersTech.BabelFish.Requests.ScoreHistoryAPI;
using ShootersTech.BabelFish.Responses.ScoreHistoryAPI;

namespace ShootersTech.BabelFish.ScoreHistoryAPI {
    public class ScoreHistoryAPIClient : APIClient {

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="apiKey"></param>
        public ScoreHistoryAPIClient( string apiKey) : base(apiKey) { }

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="xapikey">Your assigned XApiKey</param>
        /// <param name="CustomUserSettings">Dictionary<string,string> of Allowed User Settings</param>
        public ScoreHistoryAPIClient( string xapikey, Dictionary<string, string> CustomUserSettings) : base(xapikey, CustomUserSettings) { }


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
