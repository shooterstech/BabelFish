using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Responses.OrionMatchAPI;
using Scopos.BabelFish.Runtime;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.APIClients {
    public class OrionMatchAPIClient : APIClient<OrionMatchAPIClient> {

        /// <summary>
        /// Default constructor.
        /// Assumes Production stage level.
        /// </summary>
        /// <param name="xapikey"></param>
        public OrionMatchAPIClient( string xapikey ) : base( xapikey ) {
			IgnoreInMemoryCache = false;

            //OrionMatchAPIClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
        }

        public OrionMatchAPIClient( string xapikey, APIStage apiStage ) : base( xapikey, apiStage ) {
			IgnoreInMemoryCache = false;

            //OrionMatchAPIClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
        }

        #region Match API Calls
        /// <summary>
        /// Get Match Detail API
        /// </summary>
        /// <param name="requestParameters">GetMatchRequest object</param>
        /// <returns>Match Object</returns>
        public async Task<GetMatchPublicResponse> GetMatchDetailPublicAsync( GetMatchPublicRequest requestParameters ) {

            GetMatchPublicResponse response = new GetMatchPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Get Match Detail API
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns>Match Object</returns>
        public async Task<GetMatchPublicResponse> GetMatchDetailPublicAsync( MatchID matchid ) {
            var request = new GetMatchPublicRequest( matchid );

            return await GetMatchDetailPublicAsync( request );
        }

        /// <summary>
        /// Get Match Detail API
        /// </summary>
        /// <param name="requestParameters">GetMatchRequest object</param>
        /// <returns>Match Object</returns>
        public async Task<GetMatchAuthenticatedResponse> GetMatchDetailAuthenticatedAsync( GetMatchAuthenticatedRequest requestParameters ) {

            GetMatchAuthenticatedResponse response = new GetMatchAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Get Match Detail API
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="withAuthentication">default false</param>
        /// <returns>Match Object</returns>
        public async Task<GetMatchAuthenticatedResponse> GetMatchDetailAuthenticatedAsync( MatchID matchid, UserAuthentication credentials ) {
            var request = new GetMatchAuthenticatedRequest( matchid, credentials );

            return await GetMatchDetailAuthenticatedAsync( request );
        }

        /// <summary>
        /// Function that abstracts the Public vs Authenticated calls. If credentials is null, then a PublicAPI call is made.
        /// If credentials if not null, then an Authenticated API call is made.
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public async Task<GetMatchResponse> GetMatchDetailAsync( MatchID matchid, UserAuthentication? credentials = null) {
            if (credentials == null) {
                return await GetMatchDetailPublicAsync( matchid );
            } else {
                return await GetMatchDetailAuthenticatedAsync( matchid, credentials );
            }
        }

        #endregion

        #region Get Result List
        /// <summary>
        /// Get Result List API
        /// </summary>
        /// <param name="requestParameters">GetResultListRequest object</param>
        /// <returns>ResultList Object</returns>
        public async Task<GetResultListPublicResponse> GetResultListPublicAsync( GetResultListPublicRequest requestParameters ) {
            GetResultListPublicResponse response = new GetResultListPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );
			await response.PostResponseProcessingAsync().ConfigureAwait( false );

			return response;
        }

        /// <summary>
        /// Get Result List API
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="listname"></param>
        /// <returns>ResultList Object</returns>
        public async Task<GetResultListPublicResponse> GetResultListPublicAsync( MatchID matchid, string listname ) {
            return await GetResultListPublicAsync( new GetResultListPublicRequest( matchid, listname ) ).ConfigureAwait( false );
        }

        /// <summary>
        /// Get Result List API
        /// </summary>
        /// <param name="requestParameters">GetResultListRequest object</param>
        /// <returns>ResultList Object</returns>
        public async Task<GetResultListAuthenticatedResponse> GetResultListAuthenticatedAsync( GetResultListAuthenticatedRequest requestParameters ) {
            GetResultListAuthenticatedResponse response = new GetResultListAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );
			await response.PostResponseProcessingAsync().ConfigureAwait( false );

			return response;
        }

        /// <summary>
        /// Get Result List API
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="listname"></param>
        /// <returns>ResultList Object</returns>
        public async Task<GetResultListAuthenticatedResponse> GetResultListAuthenticatedAsync( MatchID matchid, string listname, UserAuthentication credentials ) {
            return await GetResultListAuthenticatedAsync( new GetResultListAuthenticatedRequest( matchid, listname, credentials ) ).ConfigureAwait( false );
        }

        /// <summary>
        /// Function that abstracts the Public vs Authenticated calls. If credentials is null, then a PublicAPI call is made.
        /// If credentials if not null, then an Authenticated API call is made.
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public async Task<GetResultListResponse> GetResultLIstAsync( MatchID matchid, string listname, UserAuthentication? credentials = null ) {
            if (credentials == null) {
                return await GetResultListPublicAsync( matchid, listname );
            } else {
                return await GetResultListAuthenticatedAsync( matchid, listname, credentials );
            }
        }
        #endregion

        #region Get Squadding List
        /// <summary>
        /// Get Squadding List API
        /// </summary>
        /// <param name="requestParameters">GetSquaddingListRequest object</param>
        /// <returns>Squadding Object</returns>
        public async Task<GetSquaddingListPublicResponse> GetSquaddingListPublicAsync( GetSquaddingListPublicRequest requestParameters ) {

            GetSquaddingListPublicResponse response = new GetSquaddingListPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Get Squadding List API
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="squaddinglistname"></param>
        /// <returns>Squadding Object</returns>
        public async Task<GetSquaddingListPublicResponse> GetSquaddingListPublicAsync( MatchID matchid, string squaddinglistname ) {
            var request = new GetSquaddingListPublicRequest( matchid, squaddinglistname );

            return await GetSquaddingListPublicAsync( request );
        }

        /// <summary>
        /// Get Squadding List API, limit the response's length by the passed in relayName
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="squaddinglistname"></param>
        /// <param name="relayName"></param>
        /// <returns>Squadding Object</returns>
        public async Task<GetSquaddingListPublicResponse> GetSquaddingListPublicAsync( MatchID matchid, string squaddinglistname, string relayName ) {
            var request = new GetSquaddingListPublicRequest( matchid, squaddinglistname );
            request.RelayName = relayName;

            return await GetSquaddingListPublicAsync( request );
        }

        /// <summary>
        /// Get Squadding List API
        /// </summary>
        /// <param name="requestParameters">GetSquaddingListRequest object</param>
        /// <returns>Squadding Object</returns>
        public async Task<GetSquaddingListAuthenticatedResponse> GetSquaddingListAuthenticatedAsync( GetSquaddingListAuthenticatedRequest requestParameters ) {

            GetSquaddingListAuthenticatedResponse response = new GetSquaddingListAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Get Squadding List API
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="squaddinglistname"></param>
        /// <returns>Squadding Object</returns>
        public async Task<GetSquaddingListAuthenticatedResponse> GetSquaddingListAuthenticatedAsync( MatchID matchid, string squaddinglistname, UserAuthentication credentials ) {
            var request = new GetSquaddingListAuthenticatedRequest( matchid, squaddinglistname, credentials );

            return await GetSquaddingListAuthenticatedAsync( request );
        }

        /// <summary>
        /// Get Squadding List API, limit the response's length by the passed in relayName
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="squaddinglistname"></param>
        /// <param name="relayName"></param>
        /// <returns>Squadding Object</returns>
        public async Task<GetSquaddingListAuthenticatedResponse> GetSquaddingListAuthenticatedAsync( MatchID matchid, string squaddinglistname, string relayName, UserAuthentication credentials ) {
            var request = new GetSquaddingListAuthenticatedRequest( matchid, squaddinglistname, credentials );
            request.RelayName = relayName;

            return await GetSquaddingListAuthenticatedAsync( request );
        }

        #endregion

        #region Get Result COF
        /// <summary>
        /// Get Course Of Fire Detail API
        /// </summary>
        /// <param name="requestParameters">GetResultCOFDetailRequest object</param>
        /// <returns>ResultCOF Object</returns>
        public async Task<GetResultCOFDetailPublicResponse> GetResultCourseOfFireDetailPublicAsync( GetResultCOFDetailPublicRequest requestParameters ) {
            GetResultCOFDetailPublicResponse response = new GetResultCOFDetailPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );
            await response.PostResponseProcessingAsync().ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Course Of Fire Detail API
        /// </summary>
        /// <param name="resultCOFID"></param>
        /// <returns>ResultCOF Object</returns>
        public async Task<GetResultCOFDetailPublicResponse> GetResultCourseOfFireDetailPublicAsync( string resultCOFID ) {
            return await GetResultCourseOfFireDetailPublicAsync( new GetResultCOFDetailPublicRequest( resultCOFID ) );
        }

        /// <summary>
        /// Get Course Of Fire Detail API
        /// </summary>
        /// <param name="requestParameters">GetResultCOFDetailRequest object</param>
        /// <returns>ResultCOF Object</returns>
        public async Task<GetResultCOFDetailAuthenticatedResponse> GetResultCourseOfFireDetailAuthenticatedAsync( GetResultCOFDetailAuthenticatedRequest requestParameters ) {
            GetResultCOFDetailAuthenticatedResponse response = new GetResultCOFDetailAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );
			await response.PostResponseProcessingAsync().ConfigureAwait( false );

			return response;
        }

        /// <summary>
        /// Get Course Of Fire Detail API
        /// </summary>
        /// <param name="resultCOFID"></param>
        /// <returns>ResultCOF Object</returns>
        public async Task<GetResultCOFDetailAuthenticatedResponse> GetResultCourseOfFireDetailAuthenticatedAsync( string resultCOFID, UserAuthentication credentials ) {
            return await GetResultCourseOfFireDetailAuthenticatedAsync( new GetResultCOFDetailAuthenticatedRequest( resultCOFID, credentials ) );
        }

        /// <summary>
        /// Function that abstracts the Public vs Authenticated calls. If credentials is null, then a PublicAPI call is made.
        /// If credentials if not null, then an Authenticated API call is made.
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public async Task<GetResultCOFDetailResponse> GetResultCourseOfFireDetailAsync( string resultCOFID, UserAuthentication? credentials = null ) {
            if (credentials == null) {
                return await GetResultCourseOfFireDetailPublicAsync( resultCOFID );
            } else {
                return await GetResultCourseOfFireDetailAuthenticatedAsync( resultCOFID, credentials );
            }
        }
        #endregion

        #region Match Search
        /// <summary>
        /// Get Match Search API
        /// </summary>
        /// <param name="requestParameters">GetMatchSearchRequest object</param>
        /// <returns>List<Match> Object</returns>
        public async Task<MatchSearchPublicResponse> GetMatchSearchPublicAsync( MatchSearchPublicRequest requestParameters ) {
            MatchSearchPublicResponse response = new MatchSearchPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Get Match Search API
        /// </summary>
        /// <param name="requestParameters">GetMatchSearchRequest object</param>
        /// <returns>List<Match> Object</returns>
        public async Task<MatchSearchAuthenticatedResponse> GetMatchSearchAuthenticatedAsync( MatchSearchAuthenticatedRequest requestParameters ) {
            MatchSearchAuthenticatedResponse response = new MatchSearchAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }
        #endregion

        #region Match Participant List
        /// <summary>
        /// Requests a list of Match Participants for a specified match.
        /// </summary>
        /// <param name="requestParameters">GetParticipantListRequest object</param>
        /// <returns>Match Participant List Object</returns>
        public async Task<GetMatchParticipantListPublicResponse> GetMatchParticipantListPublicAsync( GetMatchParticipantListPublicRequest requestParameters ) {
            GetMatchParticipantListPublicResponse response = new GetMatchParticipantListPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Requests a list of Match Participants for a specified match.
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns>Match Participant List Object</returns>
        public async Task<GetMatchParticipantListPublicResponse> GetMatchParticipantListPublicAsync( MatchID matchid ) {
            var request = new GetMatchParticipantListPublicRequest( matchid );

            return await GetMatchParticipantListPublicAsync( request );
        }

        /// <summary>
        /// Requests a list of Match Participants for a specified match. Limited by Match Particpants with the specified role
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="role"></param>
        /// <returns>Match Participant List Object</returns>
        public async Task<GetMatchParticipantListPublicResponse> GetMatchParticipantListPublicAsync( MatchID matchid, MatchParticipantRole role ) {
            var request = new GetMatchParticipantListPublicRequest( matchid );
            request.Role = role;

            return await GetMatchParticipantListPublicAsync( request );
        }

        /// <summary>
        /// Requests a list of Match Participants for a specified match.
        /// </summary>
        /// <param name="requestParameters">GetParticipantListRequest object</param>
        /// <returns>Match Participant List Object</returns>
        public async Task<GetMatchParticipantListAuthenticatedResponse> GetMatchParticipantListAuthenticatedAsync( GetMatchParticipantListAuthenticatedRequest requestParameters ) {
            GetMatchParticipantListAuthenticatedResponse response = new GetMatchParticipantListAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Requests a list of Match Participants for a specified match.
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns>Match Participant List Object</returns>
        public async Task<GetMatchParticipantListAuthenticatedResponse> GetMatchParticipantListAuthenticatedAsync( MatchID matchid, UserAuthentication credentials ) {
            var request = new GetMatchParticipantListAuthenticatedRequest( matchid, credentials );

            return await GetMatchParticipantListAuthenticatedAsync( request );
        }

        /// <summary>
        /// Requests a list of Match Participants for a specified match. Limited by Match Particpants with the specified role
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="role"></param>
        /// <returns>Match Participant List Object</returns>
        public async Task<GetMatchParticipantListAuthenticatedResponse> GetMatchParticipantListAuthenticatedAsync( MatchID matchid, MatchParticipantRole role, UserAuthentication credentials ) {
            var request = new GetMatchParticipantListAuthenticatedRequest( matchid, credentials );
            request.Role = role;

            return await GetMatchParticipantListAuthenticatedAsync( request );
        }

		#endregion

		#region League API CAlls
		/// <summary>
		/// Get League Detail API
		/// </summary>
		/// <param name="requestParameters">GetMatchRequest object</param>
		public async Task<GetLeaguePublicResponse> GetLeagueDetailPublicAsync( GetLeaguePublicRequest requestParameters ) {

			GetLeaguePublicResponse response = new GetLeaguePublicResponse( requestParameters );

			await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

			return response;
		}

		/// <summary>
		/// Get League Detail API
		/// </summary>
		/// <param name="leagueId"></param>
		public async Task<GetLeaguePublicResponse> GetLeagueDetailPublicAsync( string leagueId ) {
			var request = new GetLeaguePublicRequest( leagueId );

			return await GetLeagueDetailPublicAsync( request ).ConfigureAwait( false );
		}

		/// <summary>
		/// Get League Games API
		/// </summary>
		/// <param name="requestParameters">GetMatchRequest object</param>
		public async Task<GetLeagueGamesPublicResponse> GetLeagueGamesPublicAsync( GetLeagueGamesPublicRequest requestParameters ) {

			GetLeagueGamesPublicResponse response = new GetLeagueGamesPublicResponse( requestParameters );

			await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

			return response;
		}

		/// <summary>
		/// Get League Games API
		/// </summary>
		/// <param name="leagueId"></param>
		public async Task<GetLeagueGamesPublicResponse> GetLeagueGamesPublicAsync( string leagueId ) {
			var request = new GetLeagueGamesPublicRequest( leagueId );

			return await GetLeagueGamesPublicAsync( request ).ConfigureAwait( false );
        }

        /// <summary>
        /// Get League Teams API
        /// </summary>
        /// <param name="requestParameters">GetLeagueTeamsPublicRequest object</param>
        public async Task<GetLeagueTeamsPublicResponse> GetLeagueTeamsPublicAsync( GetLeagueTeamsPublicRequest requestParameters ) {

            GetLeagueTeamsPublicResponse response = new GetLeagueTeamsPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get League Teams API
        /// </summary>
        /// <param name="leagueId"></param>
        public async Task<GetLeagueTeamsPublicResponse> GetLeagueTeamsPublicAsync( string leagueId ) {
            var request = new GetLeagueTeamsPublicRequest( leagueId );

            return await GetLeagueTeamsPublicAsync( request ).ConfigureAwait( false );
        }

        /// <summary>
        /// Get League Team Detail API
        /// </summary>
        /// <param name="requestParameters">GetLeagueTeamsPublicRequest object</param>
        public async Task<GetLeagueTeamDetailPublicResponse> GetLeagueTeamDetailPublicAsync( GetLeagueTeamDetailPublicRequest requestParameters ) {

            GetLeagueTeamDetailPublicResponse response = new GetLeagueTeamDetailPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get League Teams API
        /// </summary>
        /// <param name="leagueId"></param>
        /// <param name="teamId"></param>
        public async Task<GetLeagueTeamDetailPublicResponse> GetLeagueTeamDetailPublicAsync( string leagueId, int teamId ) {
            var request = new GetLeagueTeamDetailPublicRequest( leagueId, teamId );

            return await GetLeagueTeamDetailPublicAsync( request ).ConfigureAwait( false );
        }
        #endregion

    }
}
