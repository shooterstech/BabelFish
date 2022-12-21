using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootersTech.BabelFish.DataModel.OrionMatch;
using ShootersTech.BabelFish.Requests.ClubsAPI;
using ShootersTech.BabelFish.Requests.OrionMatchAPI;
using ShootersTech.BabelFish.Responses.ClubsAPI;
using ShootersTech.BabelFish.Responses.OrionMatchAPI;

namespace ShootersTech.BabelFish.ClubsAPI {

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

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="xapikey">Your assigned XApiKey</param>
        /// <param name="CustomUserSettings">Dictionary<string,string> of Allowed User Settings</param>
        public ClubsAPIClient( string xapikey, Dictionary<string, string> CustomUserSettings) : base(xapikey, CustomUserSettings) { }

        /// <summary>
        /// GetClubList returns a list of clubs (aka Orion Accounts) the logged in user is associated with as an Admin / member / etc.
        /// Generally this ia a parameterless call.
        /// </summary>
        public async Task<GetClubListResponse> GetClubListAsync() {

            var request = new GetClubListRequest();

            return await GetClubListAsync(request);
        }

        /// <summary>
        /// GetClubList returns a list of clubs (aka Orion Accounts) the logged in user is associated with as an Admin / member / etc.
        /// Generally this ia a parameterless call, but may also include a Token value, with the list of clubs is too large
        /// to return in a single response. 
        /// </summary>
        /// <param name="request"></param>
        public async Task<GetClubListResponse> GetClubListAsync( GetClubListRequest request ) {

            //GetClubList requires authentication
            request.WithAuthentication = true;

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

            //GetClubDetail requires authentication
            request.WithAuthentication = true;

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
        public async Task<GetClubDetailResponse> GetClubDetailAsync( int orionLicenseNumber ) {

            var ownerId = $"OrionAcct{orionLicenseNumber:06d}";
            return await GetClubDetailAsync( ownerId );
        }

        /// <summary>
        /// Returns detailed information about an Orion account (aka a clubs account), identified by
        /// the passed on owner-id (e.g. OrionAcct001234).
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public async Task<GetClubDetailResponse> GetClubDetailAsync( string ownerId ) {

            var request = new GetClubDetailRequest( ownerId );

            return await GetClubDetailAsync( request );
        }

    }
}
