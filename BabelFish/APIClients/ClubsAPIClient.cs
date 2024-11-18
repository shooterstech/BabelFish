using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.Requests.ClubsAPI;
using Scopos.BabelFish.Responses.ClubsAPI;
using Scopos.BabelFish.Runtime;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.APIClients {

    /// <summary>
    /// API Client to access information about an Orion Acct, club ownership, and club teams.
    /// An Orion Acct is the same as a Club. And a Club is the same as an Orion Acct
    /// </summary>
    public class ClubsAPIClient : APIClient<ClubsAPIClient> {

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="xapikey"></param>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        public ClubsAPIClient() : base() {

            IgnoreInMemoryCache = false;

            //ClubsAPIClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
        }

        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        public ClubsAPIClient( APIStage apiStage ) : base( apiStage ) {

            IgnoreInMemoryCache = false;

            //ClubsAPIClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
        }

        /// <summary>
        /// GetClubList returns a list of clubs (aka Orion Accounts) the logged in user is associated with as an Admin / member / etc.
        /// Generally this ia a parameterless call.
        /// </summary>
        public async Task<GetClubListAuthenticatedResponse> GetClubListAuthenticatedAsync( UserAuthentication credentials ) {

            var request = new GetClubListAuthenticatedRequest( credentials );

            return await GetClubListAuthenticatedAsync(request);
        }

        /// <summary>
        /// GetClubList returns a list of clubs (aka Orion Accounts) the logged in user is associated with as an Admin / member / etc.
        /// Generally this ia a parameterless call, but may also include a Token value, with the list of clubs is too large
        /// to return in a single response. 
        /// </summary>
        /// <param name="request"></param>
        public async Task<GetClubListAuthenticatedResponse> GetClubListAuthenticatedAsync( GetClubListAuthenticatedRequest request ) {

            var response = new GetClubListAuthenticatedResponse( request );

            await this.CallAPIAsync( request, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// GetClubList returns a list of clubs (aka Orion Accounts) the logged in user is associated with as an Admin / member / etc.
        /// Generally this ia a parameterless call.
        /// </summary>
        /// <param name="currentlyShooting">If true, limits the returned list of clubs, to only the clubs that are currently shooting</param>
        public async Task<GetClubListPublicResponse> GetClubListPublicAsync( bool currentlyShooting = false  ) {

            var request = new GetClubListPublicRequest( );
            request.CurrentlyShooting = currentlyShooting;

            return await GetClubListPublicAsync( request );
        }

        /// <summary>
        /// GetClubList returns a list of clubs (aka Orion Accounts) the logged in user is associated with as an Admin / member / etc.
        /// Generally this ia a parameterless call, but may also include a Token value, with the list of clubs is too large
        /// to return in a single response. 
        /// </summary>
        /// <param name="request"></param>
        public async Task<GetClubListPublicResponse> GetClubListPublicAsync( GetClubListPublicRequest request ) {

            var response = new GetClubListPublicResponse( request );

            await this.CallAPIAsync( request, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Returns detailed information about an Orion account (aka a clubs account).
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetClubDetailAuthenticatedResponse> GetClubDetailAuthenticatedAsync( GetClubDetailAuthenticatedRequest request ) {

            var response = new GetClubDetailAuthenticatedResponse( request );

            await this.CallAPIAsync( request, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Returns detailed information about an Orion account (aka a clubs account), identified by
        /// the passed on owner-id (e.g. OrionAcct001234).
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public async Task<GetClubDetailAuthenticatedResponse> GetClubDetailAuthenticatedAsync( string ownerId, UserAuthentication credentials ) {

            var request = new GetClubDetailAuthenticatedRequest( ownerId, credentials );

            return await GetClubDetailAuthenticatedAsync( request );
        }

        /// <summary>
        /// Returns detailed information about an Orion account (aka a clubs account).
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetClubDetailPublicResponse> GetClubDetailPublicAsync( GetClubDetailPublicRequest request ) {

            var response = new GetClubDetailPublicResponse( request );

            await this.CallAPIAsync( request, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Returns detailed information about an Orion account (aka a clubs account), identified by
        /// the passed on owner-id (e.g. OrionAcct001234).
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public async Task<GetClubDetailPublicResponse> GetClubDetailPublicAsync( string ownerId ) {

            var request = new GetClubDetailPublicRequest( ownerId );

            return await GetClubDetailPublicAsync( request );
        }

        public async Task<CoachAssignmentCRUDAuthenticatedResponse> GetCoachAssignmentAuthenticatedAsync(GetCoachAssignmentAuthenticatedRequest request)
        {

            var response = new CoachAssignmentCRUDAuthenticatedResponse(request);

            await this.CallAPIAsync(request, response).ConfigureAwait(false);

            return response;
        }

        public async Task<CoachAssignmentCRUDAuthenticatedResponse> GetCoachAssignmentAuthenticatedAsync(int licenseNumber, UserAuthentication credentials)
        {

            var request = new GetCoachAssignmentAuthenticatedRequest(licenseNumber, credentials);

            return await GetCoachAssignmentAuthenticatedAsync(request);
        }

        public async Task<CoachAssignmentCRUDAuthenticatedResponse> CreateCoachAssignmentAuthenticatedAsync(CreateCoachAssignmentAuthenticatedRequest request)
        {

            var response = new CoachAssignmentCRUDAuthenticatedResponse(request);

            await this.CallAPIAsync(request, response).ConfigureAwait(false);

            return response;
        }

        public async Task<CoachAssignmentCRUDAuthenticatedResponse> DeleteCoachAssignmentAuthenticatedAsync(DeleteCoachAssignmentAuthenticatedRequest request)
        {

            var response = new CoachAssignmentCRUDAuthenticatedResponse(request);

            await this.CallAPIAsync(request, response).ConfigureAwait(false);

            return response;
        }

        public async Task<GetCoachClubListPublicResponse> GetCoachClubListPublicAsync(GetCoachClubListPublicRequest request)
        {
            var response = new GetCoachClubListPublicResponse(request);
            await this.CallAPIAsync(request, response).ConfigureAwait(false);
            return response;
        }

        public async Task<GetCoachClubListPublicResponse> GetCoachClubListPublicAsync(string userId)
        {
            var request = new GetCoachClubListPublicRequest(userId);
            return await GetCoachClubListPublicAsync(request);
        }

        public async Task<GetCoachClubListAuthenticatedResponse> GetCoachClubListAuthenticatedAsync(GetCoachClubListAuthenticatedRequest request)
        {
            var response = new GetCoachClubListAuthenticatedResponse(request);
            await this.CallAPIAsync(request, response).ConfigureAwait(false);
            return response;
        }

    }
}
