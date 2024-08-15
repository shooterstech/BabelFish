using Scopos.BabelFish.Requests.AthenaOwnerAPI;
using Scopos.BabelFish.Responses.AthenaOwnerAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.APIClients
{
    public class AthenaOwnerAPIClient: APIClient<AthenaOwnerAPIClient>
    {
        public AthenaOwnerAPIClient(string apiKey) : base(apiKey)
        {

            //AthenaOwnerAPIClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
        }

        public AthenaOwnerAPIClient(string apiKey, APIStage apiStage) : base(apiKey, apiStage)
        {

            //AthenaOwnerAPIClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
        }

        public async Task<GetUserOwnershipValuesAuthenticatedResponse> GetUserOwnershipValuesAuthenticatedAsync(GetUserOwnershipValuesAuthenticatedRequest request)
        {
            var response = new GetUserOwnershipValuesAuthenticatedResponse(request);

            await this.CallAPIAsync(request, response).ConfigureAwait(false);

            return response;
        }
    }
}
