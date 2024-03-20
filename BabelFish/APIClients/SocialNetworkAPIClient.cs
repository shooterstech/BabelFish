using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.DataModel.SocialNetwork;
using Scopos.BabelFish.Requests.SocialNetworkAPI;
using Scopos.BabelFish.Responses.SocialNetworkAPI;
using Scopos.BabelFish.Runtime;

namespace Scopos.BabelFish.APIClients
{
    public class SocialNetworkAPIClient : APIClient<SocialNetworkAPIClient>
    {
        public SocialNetworkAPIClient(string apiKey) : base(apiKey)
        {

            //SocialNetworkAPIClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
        }

        public SocialNetworkAPIClient(string apiKey, APIStage apiStage) : base(apiKey, apiStage)
        {

            //SocialNetworkAPIClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
        }


        public async Task<RelationshipRoleCRADAuthenticatedResponse> CreateRelationshipRoleAuthenticatedAsync( CreateRelationshipRoleAuthenticatedRequest requestParameters)
        {
            var response = new RelationshipRoleCRADAuthenticatedResponse(requestParameters);
            
            await this.CallAPIAsync(requestParameters, response).ConfigureAwait(false);

            return response;
        }

        public async Task<RelationshipRoleCRADAuthenticatedResponse> ReadRelationshipRoleAuthenticatedAsync(ReadRelationshipRoleAuthenticatedRequest requestParameters)
        {
            var response = new RelationshipRoleCRADAuthenticatedResponse(requestParameters);

            await this.CallAPIAsync(requestParameters, response).ConfigureAwait(false);

            return response;
        }

        public async Task<RelationshipRoleCRADAuthenticatedResponse> ApproveRelationshipRoleAuthenticatedAsync(ApproveRelationshipRoleAuthenticatedRequest requestParameters)
        {
            var response = new RelationshipRoleCRADAuthenticatedResponse(requestParameters);

            await this.CallAPIAsync(requestParameters, response).ConfigureAwait(false);

            return response;
        }

        public async Task<RelationshipRoleCRADAuthenticatedResponse> DeleteRelationshipRoleAuthenticatedAsync(DeleteRelationshipRoleAuthenticatedRequest requestParameters)
        {
            var response = new RelationshipRoleCRADAuthenticatedResponse(requestParameters);

            await this.CallAPIAsync(requestParameters, response).ConfigureAwait(false);

            return response;
        }

        public async Task<ListSocialRelationshipsAuthenticatedResponse> ListSocialRelationshipsAuthenticatedAsync(ListSocialRelationshipsAuthenticatedRequest requestParameters)
        {
            var response = new ListSocialRelationshipsAuthenticatedResponse(requestParameters);

            await this.CallAPIAsync(requestParameters, response).ConfigureAwait(false);

            return response;
        }
    }

}