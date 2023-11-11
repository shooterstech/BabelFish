using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.Requests.AthenaLogin;
using Scopos.BabelFish.Responses.AthenaLogin;
using Scopos.BabelFish.Runtime;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.APIClients {

    /// <summary>
    /// API Client to access and update information about Athena EST Units.
    /// </summary>
    public class AthenaLoginAPIClient : APIClient {

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="xapikey"></param>
        public AthenaLoginAPIClient( string xapikey) : base(xapikey) {

            //AthenaLoginAPIClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
        }

        public AthenaLoginAPIClient( string xapikey, APIStage apiStage ) : base( xapikey, apiStage ) {

            //AthenaLoginAPIClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
        }


        public async Task<AthenaEmployLoginCodeAuthenticatedResponse> AthenaEmployLoginCodeAuthenticatedAsync( AthenaEmployLoginCodeAuthenticatedRequest request ) {

            var response = new AthenaEmployLoginCodeAuthenticatedResponse( request );

            await this.CallAPIAsync( request, response ).ConfigureAwait( false );

            return response;
        }

        public async Task<AthenaLogoutSessionAuthenticatedResponse> AthenaLogoutSessionAuthenticatedAsync(AthenaLogoutSessionAuthenticatedRequest request)
        {

            var response = new AthenaLogoutSessionAuthenticatedResponse(request);

            await this.CallAPIAsync(request, response).ConfigureAwait(false);

            return response;
        }

        public async Task<AthenaListActiveSessionsAuthenticatedResponse> AthenaListActiveSessionsAuthenticatedAsync(AthenaListActiveSessionsAuthenticatedRequest request)
        {

            var response = new AthenaListActiveSessionsAuthenticatedResponse(request);

            await this.CallAPIAsync(request, response).ConfigureAwait(false);

            return response;
        }

    }
}
