using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Responses.OrionMatchAPI;
using Scopos.BabelFish.Runtime;

namespace Scopos.BabelFish.APIClients {
    public class OrionMatchAPIClient : APIClient {

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="xapikey"></param>
        public OrionMatchAPIClient( string xapikey ) : base( xapikey ) { }

        public OrionMatchAPIClient( string xapikey, APIStage apiStage ) : base( xapikey, apiStage ) { }

        /// <summary>
        /// Get Match Detail API
        /// </summary>
        /// <param name="requestParameters">GetMatchRequest object</param>
        /// <returns>Match Object</returns>
        public async Task<GetMatchResponse> GetMatchDetailAsync( GetMatchRequest requestParameters ) {

            GetMatchResponse response = new GetMatchResponse( requestParameters );

            await this.CallAPI( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Match Detail API
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="withAuthentication">default false</param>
        /// <returns>Match Object</returns>
        public async Task<GetMatchResponse> GetMatchDetailAsync( string matchid, bool withAuthentication = false ) {
            var request = new GetMatchRequest( matchid );

            return await GetMatchDetailAsync( request ).ConfigureAwait( false );
        }

        /// <summary>
        /// Get Result List API
        /// </summary>
        /// <param name="requestParameters">GetResultListRequest object</param>
        /// <returns>ResultList Object</returns>
        public async Task<GetResultListResponse> GetResultListAsync( GetResultListRequest requestParameters ) {
            GetResultListResponse response = new GetResultListResponse( requestParameters );

            await this.CallAPI( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Result List API
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="listname"></param>
        /// <returns>ResultList Object</returns>
        public async Task<GetResultListResponse> GetResultListAsync( string matchid, string listname ) {
            return await GetResultListAsync( new GetResultListRequest( matchid, listname ) ).ConfigureAwait( false );
        }

        /// <summary>
        /// Get Squadding List API
        /// </summary>
        /// <param name="requestParameters">GetSquaddingListRequest object</param>
        /// <returns>Squadding Object</returns>
        public async Task<GetSquaddingListResponse> GetSquaddingListAsync( GetSquaddingListRequest requestParameters ) {

            GetSquaddingListResponse response = new GetSquaddingListResponse( requestParameters );

            await this.CallAPI( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Squadding List API
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="squaddinglistname"></param>
        /// <param name="withAuthentication">default false</param>
        /// <returns>Squadding Object</returns>
        public async Task<GetSquaddingListResponse> GetSquaddingListAsync( string matchid, string squaddinglistname, bool withAuthentication = false ) {
            var request = new GetSquaddingListRequest( matchid, squaddinglistname );

            return await GetSquaddingListAsync( request ).ConfigureAwait( false );
        }

        /// <summary>
        /// Get Course Of Fire Detail API
        /// </summary>
        /// <param name="requestParameters">GetResultCOFDetailRequest object</param>
        /// <returns>ResultCOF Object</returns>
        public async Task<GetResultCOFDetailResponse> GetResultCourseOfFireDetail( GetResultCOFDetailRequest requestParameters ) {
            GetResultCOFDetailResponse response = new GetResultCOFDetailResponse( requestParameters );

            await this.CallAPI( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Course Of Fire Detail API
        /// </summary>
        /// <param name="resultCOFID"></param>
        /// <returns>ResultCOF Object</returns>
        public async Task<GetResultCOFDetailResponse> GetResultCourseOfFireDetail( string resultCOFID ) {
            return await GetResultCourseOfFireDetail( new GetResultCOFDetailRequest( resultCOFID ) ).ConfigureAwait( false );
        }

        /// <summary>
        /// Get Match Locations API
        /// </summary>
        /// <returns>List<MatchLocation> Object</returns>
        public async Task<GetMatchLocationsResponse> GetMatchLocationsAsync() {
            GetMatchLocationsResponse response = new GetMatchLocationsResponse( new GetMatchLocationsRequest() );

            var request = new GetMatchLocationsRequest();

            await this.CallAPI( request, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Match Search API
        /// </summary>
        /// <param name="requestParameters">GetMatchSearchRequest object</param>
        /// <returns>List<Match> Object</returns>
        public async Task<GetMatchSearchResponse> GetMatchSearchAsync( GetMatchSearchRequest requestParameters ) {
            GetMatchSearchResponse response = new GetMatchSearchResponse( requestParameters );

            await this.CallAPI( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Participant List API
        /// </summary>
        /// <param name="requestParameters">GetParticipantListRequest object</param>
        /// <returns>Match Participant List Object</returns>
        public async Task<GetParticipantListResponse> GetMatchParticipantListAsync( GetParticipantListRequest requestParameters ) {
            GetParticipantListResponse response = new GetParticipantListResponse( requestParameters );

            await this.CallAPI( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Participant List API
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns>Match Participant List Object</returns>
        public async Task<GetParticipantListResponse> GetMatchParticipantListAsync( string matchid ) {
            var request = new GetParticipantListRequest( matchid );

            return await GetMatchParticipantListAsync( request ).ConfigureAwait( false );
        }

    }
}
