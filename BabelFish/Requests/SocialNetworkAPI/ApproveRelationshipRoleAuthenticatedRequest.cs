using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Requests.SocialNetworkAPI
{
    public class ApproveRelationshipRoleAuthenticatedRequest : RelationshipRoleCRADBaseRequest
    {
        public ApproveRelationshipRoleAuthenticatedRequest(UserAuthentication credentials) : base("RelationshipRoleA", credentials)
        {
            HttpMethod = HttpMethod.Put;

        }

    }
}
