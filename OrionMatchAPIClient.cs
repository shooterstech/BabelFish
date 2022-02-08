using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabelFish.DataModel.OrionMatch;
using BabelFish.Requests.OrionMatchAPI;
using BabelFish.Responses.OrionMatchAPI;

namespace BabelFish {
    public class OrionMatchAPIClient : APIClient {

        public OrionMatchAPIClient(string xapikey) : base(xapikey) { }

        /// <summary>
        /// Get Match Detail API
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns></returns>
        public async Task<GetMatchResponse> GetMatchDetailAsync( GetMatchRequest requestParameters ) 
        {
            GetMatchResponse response = new GetMatchResponse();

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }
        /// <summary>
        /// Get Match Detail API
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns></returns>
        public async Task<GetMatchResponse> GetMatchDetailAsync(string matchid)
        {
            return await GetMatchDetailAsync(new GetMatchRequest(matchid)).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Result List API
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns></returns>
        public async Task<GetResultListResponse> GetResultListAsync( GetResultListRequest requestParameters ) {
            GetResultListResponse response = new GetResultListResponse();

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }
        /// <summary>
        /// Get Result List API
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="listname"></param>
        /// <returns></returns>
        public async Task<GetResultListResponse> GetResultListAsync(string matchid, string listname)
        {
            return await GetResultListAsync(new GetResultListRequest(matchid, listname)).ConfigureAwait(false);
        }

        public object GetSquaddingList( object requestParameters ) {
            throw new NotImplementedException();
        }

        public object GetResultCourseOfFireDetail( object requestParameters ) {
            throw new NotImplementedException();
        }

        public object GetMatchLocations( object requestParameters ) {
            throw new NotImplementedException();
        }

        public object MatchSearch( object requestParameters ) {
            throw new NotImplementedException();
        }
    }
}
