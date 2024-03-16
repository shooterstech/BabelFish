using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Requests.SocialNetworkAPI
{
    public class CreateRelationshipRoleAuthenticatedRequest: RelationshipRoleCRADBaseRequest
    {
        public CreateRelationshipRoleAuthenticatedRequest(UserAuthentication credentials) : base("CreateRelationshipRole", credentials) {
            HttpMethod = HttpMethod.Post;
            
        }

    }
}
