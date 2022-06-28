using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using ShootersTech.Responses;
using ShootersTech.DataModel.ScoreHistory;
using ShootersTech.Requests.ScoreHistoryAPI;
using ShootersTech.Responses.ScoreHistoryAPI;

namespace ShootersTech {
    public class ScoreAPIClient : APIClient {

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="apiKey"></param>
        public ScoreAPIClient(string apiKey) : base(apiKey) { }

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="xapikey">Your assigned XApiKey</param>
        /// <param name="CustomUserSettings">Dictionary<string,string> of Allowed User Settings</param>
        public ScoreAPIClient(string xapikey, Dictionary<string, string> CustomUserSettings) : base(xapikey, CustomUserSettings) { }


        private async Task<GetScoreHistoryResponse> GetScoreHistoryAsync(GetScoreHistoryRequest requestParameters)
        {

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }

    }
}
