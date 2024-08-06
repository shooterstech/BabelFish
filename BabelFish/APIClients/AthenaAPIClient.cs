using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.Requests.Athena;
using Scopos.BabelFish.Responses.Athena;
using Scopos.BabelFish.Requests.Athena;
using Scopos.BabelFish.Runtime;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.APIClients
{

    /// <summary>
    /// API Client to access and update information about Athena EST Units.
    /// </summary>
    public class AthenaAPIClient : APIClient<AthenaAPIClient> {

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="xapikey"></param>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        public AthenaAPIClient( ) : base() {

            //AthenaLoginAPIClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
        }


        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        public AthenaAPIClient( APIStage apiStage ) : base( apiStage ) {

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

        public async Task<RemoveThingOwnershipResponse> RemoveThingOwnershipAsync(RemoveThingOwnershipRequest request)
        {

            var response = new RemoveThingOwnershipResponse(request);

            await this.CallAPIAsync(request, response).ConfigureAwait(false);

            return response;
        }

    }
}
