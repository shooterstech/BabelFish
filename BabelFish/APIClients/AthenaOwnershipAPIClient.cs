using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Requests.AthenaOwnership;
using Scopos.BabelFish.Responses.AthenaOwnership;

namespace Scopos.BabelFish.APIClients
{
    public class AthenaOwnershipAPIClient : APIClient<AthenaOwnershipAPIClient>
    {
        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="xapikey"></param>
        public AthenaOwnershipAPIClient(string xapikey) : base(xapikey)
        {

            //AthenaOwnershipAPIClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
        }

        public AthenaOwnershipAPIClient(string xapikey, APIStage apiStage) : base(xapikey, apiStage)
        {

            //AthenaOwnershipAPIClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
        }

        public async Task<RemoveThingOwnershipResponse> RemoveThingOwnershipAsync(RemoveThingOwnershipRequest request)
        {

            var response = new RemoveThingOwnershipResponse(request);

            await this.CallAPIAsync(request, response).ConfigureAwait(false);

            return response;
        }
    }
}
