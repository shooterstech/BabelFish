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
        public OrionMatchAPIClient(string xapikey, string userName = "", string passWord = "") : base(xapikey, userName, passWord) { }

        /// <summary>
        /// Get Match Detail API
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns></returns>
        public async Task<GetMatchResponse> GetMatchDetailAsync( GetMatchRequest requestParameters ) {
            GetMatchResponse response = new GetMatchResponse();

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }
        /// <summary>
        /// Get Match Detail API
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns></returns>
        public async Task<GetMatchResponse> GetMatchDetailAsync(string matchid, bool withAuthentication = false) {
            var request = new GetMatchRequest(matchid);
            request.WithAuthentication = withAuthentication;
            return await GetMatchDetailAsync(request).ConfigureAwait(false);
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
        public async Task<GetResultListResponse> GetResultListAsync(string matchid, string listname) {
            return await GetResultListAsync(new GetResultListRequest(matchid, listname)).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Squadding List API
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns></returns>
        public async Task<GetSquaddingListResponse> GetSquaddingListAsync(GetSquaddingListRequest requestParameters) {
            GetSquaddingListResponse response = new GetSquaddingListResponse();

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }
        /// <summary>
        /// Get Squadding List API
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="squaddinglistname"></param>
        /// <returns></returns>
        public async Task<GetSquaddingListResponse> GetSquaddingListAsync(string matchid, string squaddinglistname)
        {
            return await GetSquaddingListAsync(new GetSquaddingListRequest(matchid, squaddinglistname)).ConfigureAwait(false);
        }

        /// <summary>
        /// Get Course Of Fire Detail API
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns></returns>
        public async Task<GetResultCOFDetailResponse> GetResultCourseOfFireDetail(GetResultCOFDetailRequest requestParameters) {
            GetResultCOFDetailResponse response = new GetResultCOFDetailResponse();

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Get Course Of Fire Detail API
        /// </summary>
        /// <param name="resultCOFID"></param>
        /// <returns></returns>
        public async Task<GetResultCOFDetailResponse> GetResultCourseOfFireDetail(string resultCOFID) {
            return await GetResultCourseOfFireDetail(new GetResultCOFDetailRequest(resultCOFID)).ConfigureAwait(false);
        }

        public object GetMatchLocations( object requestParameters ) {
            throw new NotImplementedException();
        }

        public object MatchSearch( object requestParameters ) {
            throw new NotImplementedException();
        }
    }
}
