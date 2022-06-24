using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using BabelFish.Helpers;
using BabelFish.Responses;
using BabelFish.DataModel.Score;
using BabelFish.Requests.ScoreAPI;
using BabelFish.Responses.ScoreAPI;

namespace BabelFish {
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

        /// <summary>
        /// Generic function to call specific Score Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="style"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<GetScoreResponse<T>> GetScoreAsync<T>(ScoreStyle scoreStyle, Dictionary<string, List<string>> incomingParameters, bool authenticate, GetScoreResponse<T> response)
        {
            GetScoreRequest requestParameters = new GetScoreRequest(scoreStyle, incomingParameters, authenticate);

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Event Style Score History.
        /// Request Parameters:
        /// user-id, start-date, end-date, event-style-def, include-related, limit, continuation-token
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns></returns>
        public async Task<GetScoreResponse<DataModel.Score.EventStyleHistory>> GetEventStyleScoreHistoryAsync(Dictionary<string, List<string>> requestParameters)
        {
            //FOR NOW.....
            bool withAutnetication = false;

            var scoreType = ScoreStyle.EventStyleHistory;

            GetScoreResponse<DataModel.Score.EventStyleHistory> response = new GetScoreResponse<DataModel.Score.EventStyleHistory>(scoreType);

            return await GetScoreAsync(scoreType, requestParameters, withAutnetication, response).ConfigureAwait(false);
        }

        /// <summary>
        /// Stage Style Score History.
        /// Request Parameters:
        /// user-id, start-date, end-date, event-style-def, include-related, limit, continuation-token
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns></returns>
        public async Task<GetScoreResponse<DataModel.Score.StageStyleHistory>> GetStageStyleScoreHistoryAsync(Dictionary<string, List<string>> requestParameters)
        {
            //FOR NOW.....
            bool withAutnetication = false;

            var scoreType = ScoreStyle.StageStyleHistory;

            GetScoreResponse<DataModel.Score.StageStyleHistory> response = new GetScoreResponse<DataModel.Score.StageStyleHistory>(scoreType);

            return await GetScoreAsync(scoreType, requestParameters, withAutnetication, response).ConfigureAwait(false);
        }

        /// <summary>
        /// Event Style Score Average.
        /// Request Parameters:
        /// user-id, start-date, end-date, event-style-def, limit, continuation-token
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns></returns>
        public async Task<GetScoreResponse<DataModel.Score.EventStyleAverage>> GetEventStyleScoreAverageAsync(Dictionary<string, List<string>> requestParameters)
        {
            //FOR NOW.....
            bool withAutnetication = false;

            var scoreType = ScoreStyle.EventStyleAverage;

            GetScoreResponse<DataModel.Score.EventStyleAverage> response = new GetScoreResponse<DataModel.Score.EventStyleAverage>(scoreType);

            return await GetScoreAsync(scoreType, requestParameters, withAutnetication, response).ConfigureAwait(false);
        }
    }
}
