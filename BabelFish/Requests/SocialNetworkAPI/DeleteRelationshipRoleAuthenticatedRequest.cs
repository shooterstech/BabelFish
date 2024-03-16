using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Requests.SocialNetworkAPI
{
    public class DeleteRelationshipRoleAuthenticatedRequest : RelationshipRoleCRADBaseRequest
    {
        public DeleteRelationshipRoleAuthenticatedRequest(UserAuthentication credentials) : base("RelationshipRoleD", credentials)
        {
            HttpMethod = HttpMethod.Delete;

        }

    }
}

