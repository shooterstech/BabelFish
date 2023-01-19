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
    public class ClubsAPIClient : APIClient {

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="xapikey"></param>
        public ClubsAPIClient( string xapikey) : base(xapikey) { }

        public ClubsAPIClient( string xapikey, APIStage apiStage ) : base( xapikey, apiStage ) { }

        /// <summary>
        /// GetClubList returns a list of clubs (aka Orion Accounts) the logged in user is associated with as an Admin / member / etc.
        /// Generally this ia a parameterless call.
        /// </summary>
        public async Task<GetClubListResponse> GetClubListAsync( UserCredentials credentials) {

            var request = new GetClubListRequest( credentials );

            return await GetClubListAsync(request);
        }

        /// <summary>
        /// GetClubList returns a list of clubs (aka Orion Accounts) the logged in user is associated with as an Admin / member / etc.
        /// Generally this ia a parameterless call, but may also include a Token value, with the list of clubs is too large
        /// to return in a single response. 
        /// </summary>
        /// <param name="request"></param>
        public async Task<GetClubListResponse> GetClubListAsync( GetClubListRequest request ) {

            var response = new GetClubListResponse( request );

            await this.CallAPI( request, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Returns detailed information about an Orion account (aka a clubs account).
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetClubDetailResponse> GetClubDetailAsync( GetClubDetailRequest request ) {

            var response = new GetClubDetailResponse( request );

            await this.CallAPI( request, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Returns detailed information about an Orion account (aka a clubs account), identified by the 
        /// passed in orionLicenseNumber
        /// </summary>
        /// <param name="orionLicenseNumber"></param>
        /// <returns></returns>
        public async Task<GetClubDetailResponse> GetClubDetailAsync( int orionLicenseNumber, UserCredentials credentials ) {

            var ownerId = $"OrionAcct{orionLicenseNumber:06d}";
            return await GetClubDetailAsync( ownerId, credentials );
        }

        /// <summary>
        /// Returns detailed information about an Orion account (aka a clubs account), identified by
        /// the passed on owner-id (e.g. OrionAcct001234).
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public async Task<GetClubDetailResponse> GetClubDetailAsync( string ownerId, UserCredentials credentials ) {

            var request = new GetClubDetailRequest( ownerId, credentials );

            return await GetClubDetailAsync( request );
        }

    }
}
