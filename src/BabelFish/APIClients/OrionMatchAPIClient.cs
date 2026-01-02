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
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        public OrionMatchAPIClient() : base() {
            //enable in memory cache
			IgnoreInMemoryCache = false;

            //OrionMatchAPIClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
        }

        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        public OrionMatchAPIClient( APIStage apiStage ) : base( apiStage ) {
            //enable in memory cache
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
        public async Task<GetMatchPublicResponse> GetMatchPublicAsync( GetMatchPublicRequest requestParameters ) {

            GetMatchPublicResponse response = new GetMatchPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Get Match Detail API
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns>Match Object</returns>
        public async Task<GetMatchPublicResponse> GetMatchPublicAsync( MatchID matchid ) {
            var request = new GetMatchPublicRequest( matchid );

            return await GetMatchPublicAsync( request );
        }

        /// <summary>
        /// Get Match Detail API
        /// </summary>
        /// <param name="requestParameters">GetMatchRequest object</param>
        /// <returns>Match Object</returns>
        public async Task<GetMatchAuthenticatedResponse> GetMatchAuthenticatedAsync( GetMatchAuthenticatedRequest requestParameters ) {

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
        public async Task<GetMatchAuthenticatedResponse> GetMatchAuthenticatedAsync( MatchID matchid, UserAuthentication credentials ) {
            var request = new GetMatchAuthenticatedRequest( matchid, credentials );

            return await GetMatchAuthenticatedAsync( request );
        }

        /// <summary>
        /// Function that abstracts the Public vs Authenticated calls. If credentials is null, then a PublicAPI call is made.
        /// If credentials if not null, then an Authenticated API call is made.
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public async Task<GetMatchAbstractResponse> GetMatchAsync( MatchID matchid, UserAuthentication? credentials = null) {
            if (credentials == null) {
                return await GetMatchPublicAsync( matchid );
            } else {
                return await GetMatchAuthenticatedAsync( matchid, credentials );
            }
        }

        public async Task<GetMatchAbstractResponse> GetMatchAsync( GetMatchAbstractRequest requestParameters ) {
            if (requestParameters is GetMatchPublicRequest)
                return await this.GetMatchPublicAsync(( GetMatchPublicRequest )requestParameters );
            else if (requestParameters is GetMatchAuthenticatedRequest)
                return await this.GetMatchAuthenticatedAsync(( GetMatchAuthenticatedRequest )requestParameters );
            else
                //We shouldn't ever get here
                throw new ArgumentException( $"requestParameters is of unexpected type ${requestParameters.GetType()}." );
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
        public async Task<GetResultListAbstractResponse> GetResultListAsync( MatchID matchid, string listname, UserAuthentication? credentials = null ) {
            if (credentials == null) {
                return await GetResultListPublicAsync( matchid, listname );
            } else {
                return await GetResultListAuthenticatedAsync( matchid, listname, credentials );
            }
        }

        public async Task<GetResultListAbstractResponse> GetResultListAsync( GetResultListAbstractRequest requestParameters ) {
            if (requestParameters is GetResultListPublicRequest)
                return await this.GetResultListPublicAsync( (GetResultListPublicRequest)requestParameters );
            else if (requestParameters is GetResultListAuthenticatedRequest)
                return await this.GetResultListAuthenticatedAsync( (GetResultListAuthenticatedRequest)requestParameters );
            else
                //We shouldn't ever get here
                throw new ArgumentException( $"requestParameters is of unexpected type ${requestParameters.GetType()}." );
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

        public async Task<GetSquaddingListAbstractResponse> GetSquaddingListAsync( MatchID matchId, string squaddinglistname, UserAuthentication credentials = null ) {
            if (credentials == null) {
                return await GetSquaddingListPublicAsync( matchId, squaddinglistname );
            } else {
                return await GetSquaddingListAuthenticatedAsync( matchId, squaddinglistname, credentials );
            }
        }

        public async Task<GetSquaddingListAbstractResponse> GetSquaddingListAsync( GetSquaddingListAbstractRequest requestParameters ) {
            if (requestParameters is GetSquaddingListPublicRequest)
                return await this.GetSquaddingListPublicAsync( (GetSquaddingListPublicRequest)requestParameters );
            else if (requestParameters is GetSquaddingListAuthenticatedRequest)
                return await this.GetSquaddingListAuthenticatedAsync( (GetSquaddingListAuthenticatedRequest)requestParameters );
            else
                //We shouldn't ever get here
                throw new ArgumentException( $"requestParameters is of unexpected type ${requestParameters.GetType()}." );
        }

        #endregion

        #region Get Result COF
        /// <summary>
        /// Get Course Of Fire Detail API
        /// </summary>
        /// <param name="requestParameters">GetResultCOFDetailRequest object</param>
        /// <returns>ResultCOF Object</returns>
        public async Task<GetResultCOFPublicResponse> GetResultCourseOfFireDetailPublicAsync( GetResultCOFPublicRequest requestParameters ) {
            GetResultCOFPublicResponse response = new GetResultCOFPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response ).ConfigureAwait( false );
            await response.PostResponseProcessingAsync().ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Course Of Fire Detail API
        /// </summary>
        /// <param name="resultCOFID"></param>
        /// <returns>ResultCOF Object</returns>
        public async Task<GetResultCOFPublicResponse> GetResultCourseOfFireDetailPublicAsync( string resultCOFID ) {
            return await GetResultCourseOfFireDetailPublicAsync( new GetResultCOFPublicRequest( resultCOFID ) );
        }

        /// <summary>
        /// Get Course Of Fire Detail API
        /// </summary>
        /// <param name="requestParameters">GetResultCOFDetailRequest object</param>
        /// <returns>ResultCOF Object</returns>
        public async Task<GetResultCOFAuthenticatedResponse> GetResultCourseOfFireDetailAuthenticatedAsync( GetResultCOFAuthenticatedRequest requestParameters ) {
            GetResultCOFAuthenticatedResponse response = new GetResultCOFAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );
			await response.PostResponseProcessingAsync().ConfigureAwait( false );

			return response;
        }

        /// <summary>
        /// Get Course Of Fire Detail API
        /// </summary>
        /// <param name="resultCOFID"></param>
        /// <returns>ResultCOF Object</returns>
        public async Task<GetResultCOFAuthenticatedResponse> GetResultCourseOfFireDetailAuthenticatedAsync( string resultCOFID, UserAuthentication credentials ) {
            return await GetResultCourseOfFireDetailAuthenticatedAsync( new GetResultCOFAuthenticatedRequest( resultCOFID, credentials ) );
        }

        /// <summary>
        /// Function that abstracts the Public vs Authenticated calls. If credentials is null, then a PublicAPI call is made.
        /// If credentials if not null, then an Authenticated API call is made.
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public async Task<GetResultCOFAbstractResponse> GetResultCourseOfFireDetailAsync( string resultCOFID, UserAuthentication? credentials = null ) {
            if (credentials == null) {
                return await GetResultCourseOfFireDetailPublicAsync( resultCOFID );
            } else {
                return await GetResultCourseOfFireDetailAuthenticatedAsync( resultCOFID, credentials );
            }
        }

        public async Task<GetResultCOFAbstractResponse> GetResultCourseOfFireDetailAsync( GetResultCOFAbstractRequest requestParameters) {
            if (requestParameters is GetResultCOFPublicRequest) 
                return await this.GetResultCourseOfFireDetailPublicAsync( (GetResultCOFPublicRequest)(requestParameters));
            else if (requestParameters is GetResultCOFAuthenticatedRequest)
                return await this.GetResultCourseOfFireDetailAuthenticatedAsync( (GetResultCOFAuthenticatedRequest)requestParameters );
            else
                //We shouldn't ever get here
                throw new ArgumentException( $"requestParameters is of unexpected type ${requestParameters.GetType()}." );
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

        public async Task<MatchSearchAbstractResponse> GetMatchSearchAsync( MatchSearchAbstractRequest requestParameters ) {
            if (requestParameters is MatchSearchPublicRequest )
                return await this.GetMatchSearchPublicAsync((MatchSearchPublicRequest)requestParameters);
            else if (requestParameters is MatchSearchAuthenticatedRequest)
                return await this.GetMatchSearchAuthenticatedAsync((MatchSearchAuthenticatedRequest)requestParameters);
            else
                //We shouldn't ever get here
                throw new ArgumentException( $"requestParameters is of unexpected type ${requestParameters.GetType()}." );
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

        public async Task<GetMatchParticipantListAbstractResponse> GetMatchParticipantListAsync( MatchID matchId, UserAuthentication credentials = null ) {
            if (credentials == null)
                return await this.GetMatchParticipantListPublicAsync( matchId );
            else
                return await this.GetMatchParticipantListAuthenticatedAsync( matchId, credentials );
        }

        public async Task<GetMatchParticipantListAbstractResponse> GetMatchParticipantListAsync( GetMatchParticipantListAbstractRequest requestParameters ) {
            if (requestParameters is GetMatchParticipantListPublicRequest)
                return await this.GetMatchParticipantListPublicAsync( (GetMatchParticipantListPublicRequest)(requestParameters) );
            else if (requestParameters is GetMatchParticipantListAuthenticatedRequest)
                return await this.GetMatchParticipantListAuthenticatedAsync( (GetMatchParticipantListAuthenticatedRequest)(requestParameters) );
            else
                //We shouldn't ever get here
                throw new ArgumentException( $"requestParameters is of unexpected type ${requestParameters.GetType()}." );
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
		public async Task<GetLeaguePublicResponse> GetLeagueDetailPublicAsync( MatchID ? leagueId ) {
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
		public async Task<GetLeagueGamesPublicResponse> GetLeagueGamesPublicAsync( MatchID ? leagueId ) {
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
        public async Task<GetLeagueTeamsPublicResponse> GetLeagueTeamsPublicAsync( MatchID ? leagueId ) {
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
        public async Task<GetLeagueTeamDetailPublicResponse> GetLeagueTeamDetailPublicAsync( MatchID leagueId, int teamId ) {
            var request = new GetLeagueTeamDetailPublicRequest( leagueId, teamId );

            return await GetLeagueTeamDetailPublicAsync( request ).ConfigureAwait( false );
        }

		public async Task<GetPressReleaseGenerationAuthenticatedResponse> GetPressReleaseGenerationAuthenticatedAsync(GetPressReleaseGenerationAuthenticatedRequest requestParameters)
		{
			GetPressReleaseGenerationAuthenticatedResponse response = new GetPressReleaseGenerationAuthenticatedResponse(requestParameters);

			await this.CallAPIAsync(requestParameters, response);

			return response;
		}

        public async Task<PostSendPressReleaseEmailAuthenticatedResponse> PostSendPressReleaseEmailAsync( PostSendPressReleaseEmailAuthenticatedRequest requestParameters ) {
            PostSendPressReleaseEmailAuthenticatedResponse response = new PostSendPressReleaseEmailAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;

        }

        #endregion

        #region Tournament API Calls

        /// <summary>
        /// Get Tournament Detail API
        /// </summary>
        /// <param name="requestParameters">GetMatchRequest object</param>
        /// <returns>Tournament Object</returns>
        public async Task<GetTournamentPublicResponse> GetTournamentPublicAsync( GetTournamentPublicRequest requestParameters ) {

            GetTournamentPublicResponse response = new GetTournamentPublicResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Get Tournament Detail API
        /// </summary>
        /// <param name="matchid"></param>
        /// <returns>Tournament Object</returns>
        public async Task<GetTournamentPublicResponse> GetTournamentPublicAsync( MatchID tournamentId ) {
            var request = new GetTournamentPublicRequest( tournamentId );

            return await GetTournamentPublicAsync( request );
        }

        /// <summary>
        /// Get Tournament Detail API
        /// </summary>
        /// <param name="requestParameters">GetTournamentRequest object</param>
        /// <returns>Tournament Object</returns>
        public async Task<GetTournamentAuthenticatedResponse> GetTournamentAuthenticatedAsync( GetTournamentAuthenticatedRequest requestParameters ) {

            GetTournamentAuthenticatedResponse response = new GetTournamentAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        /// <summary>
        /// Get Tournament Detail API
        /// </summary>
        /// <param name="matchid"></param>
        /// <param name="withAuthentication">default false</param>
        /// <returns>Tournament Object</returns>
        public async Task<GetTournamentAuthenticatedResponse> GetTournamentAuthenticatedAsync( MatchID tournamentId, UserAuthentication credentials ) {
            var request = new GetTournamentAuthenticatedRequest( tournamentId, credentials );

            return await GetTournamentAuthenticatedAsync( request );
        }

        /// <summary>
        /// Function that abstracts the Public vs Authenticated calls. If credentials is null, then a PublicAPI call is made.
        /// If credentials if not null, then an Authenticated API call is made.
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public async Task<GetTournamentAbstractResponse> GetTournamentAsync( MatchID tournamentId, UserAuthentication? credentials = null ) {
            if (credentials == null) {
                return await GetTournamentPublicAsync( tournamentId );
            } else {
                return await GetTournamentAuthenticatedAsync( tournamentId, credentials );
            }
        }

        public async Task<GetTournamentAbstractResponse> GetTournamentAsync( GetTournamentAbstractRequest requestParameters ) {
            if (requestParameters is GetTournamentPublicRequest)
                return await this.GetTournamentPublicAsync( (GetTournamentPublicRequest)requestParameters );
            else if (requestParameters is GetTournamentAuthenticatedRequest)
                return await this.GetTournamentAuthenticatedAsync( (GetTournamentAuthenticatedRequest)requestParameters );
            else
                //We shouldn't ever get here
                throw new ArgumentException( $"requestParameters is of unexpected type ${requestParameters.GetType()}." );
        }
        #endregion

        #region Post a Shot API Calls

        public async Task<PostShotDataAuthenticatedResponse> PostShotDataAuthenticatedAsync( PostShotDataAuthenticatedRequest requestParameters ) {
            PostShotDataAuthenticatedResponse response = new PostShotDataAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );

            return response;
        }

        #endregion
    }
}
