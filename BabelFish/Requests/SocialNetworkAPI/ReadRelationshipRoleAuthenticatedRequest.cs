using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Requests.SocialNetworkAPI
{
    public class ReadRelationshipRoleAuthenticatedRequest : RelationshipRoleCRADBaseRequest
    {
        public ReadRelationshipRoleAuthenticatedRequest(UserAuthentication credentials) : base("CreateRelationshipRole", credentials)
        {
            HttpMethod = HttpMethod.Get;

        }

    }
}
