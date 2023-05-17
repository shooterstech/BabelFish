using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.Requests.AthenaTarget;
using Scopos.BabelFish.Responses.AthenaTarget;
using Scopos.BabelFish.Runtime;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.APIClients {

    /// <summary>
    /// API Client to access and update information about Athena EST Units.
    /// </summary>
    public class AthenaTargetAPIClient : APIClient {

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="xapikey"></param>
        public AthenaTargetAPIClient( string xapikey) : base(xapikey) { }

        public AthenaTargetAPIClient( string xapikey, APIStage apiStage ) : base( xapikey, apiStage ) { }

        public async Task<AthenaEmployLoginCodeAuthenticatedResponse> AthenaEmployLoginCodeAuthenticatedAsync( AthenaEmployLoginCodeAuthenticatedRequest request ) {

            var response = new AthenaEmployLoginCodeAuthenticatedResponse( request );

            await this.CallAPIAsync( request, response ).ConfigureAwait( false );

            return response;
        }

        /*
        public async Task<AthenaLogoutSessionAuthenticatedResponse> AthenaLogoutSessionAuthenticatedAsync( AthenaLogoutSessionAuthenticatedRequest request ) {

        }
        */

    }
}
