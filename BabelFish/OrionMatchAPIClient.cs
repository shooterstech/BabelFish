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

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="xapikey"></param>
        public OrionMatchAPIClient(string xapikey) : base(xapikey) { }

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="xapikey">Your assigned XApiKey</param>
        /// <param name="userName">Your username if Auth function</param>
        /// <param name="passWord">Your password if Auth function</param>
        /// <param name="sendinUserSettings">Dictionary<string,string> of Allowed User Settings</param>
        public OrionMatchAPIClient(string xapikey, string userName = "", string passWord = "", Dictionary<string, string>? sendinUserSettings = null) : base(xapikey, userName, passWord, sendinUserSettings) { }

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
        public async Task<GetSquaddingListResponse> GetSquaddingListAsync(string matchid, string squaddinglistname, bool withAuthentication = false )
        {
            var request = new GetSquaddingListRequest(matchid, squaddinglistname);

            request.WithAuthentication = withAuthentication;
            
            return await GetSquaddingListAsync(request).ConfigureAwait(false);
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

        public async Task<GetMatchSearchResponse> GetMatchSearchAsync(GetMatchSearchRequest requestParameters) {
            GetMatchSearchResponse response = new GetMatchSearchResponse();

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }
        public async Task<GetMatchSearchResponse> GetMatchSearchAsync(int DistanceSearch, string StartingDate, 
            string EndingDate, string ShootingStyle, int NumberOfMatchesToReturn, double Longitude, double Latitude, bool withAuthentication = false)
        {
            var request = new GetMatchSearchRequest(DistanceSearch, StartingDate,
                EndingDate, ShootingStyle, NumberOfMatchesToReturn, Longitude, Latitude);
            request.WithAuthentication = withAuthentication;

            return await GetMatchSearchAsync(request).ConfigureAwait(false);
        }
    }
}
