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
        public ClubsAPIClient( string xapikey) : base(xapikey) {

            //ClubsAPIClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
        }

        public ClubsAPIClient( string xapikey, APIStage apiStage ) : base( xapikey, apiStage ) {

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
        public async Task<GetClubDetailPublicResponse> GetClubDetailPublicAsync( string ownerId, UserAuthentication credentials ) {

            var request = new GetClubDetailPublicRequest( ownerId );

            return await GetClubDetailPublicAsync( request );
        }

    }
}
