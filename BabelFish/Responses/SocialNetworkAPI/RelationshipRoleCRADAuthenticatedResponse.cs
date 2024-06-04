

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.SocialNetwork;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Requests.SocialNetworkAPI;

namespace Scopos.BabelFish.Responses.SocialNetworkAPI
{
    public class RelationshipRoleCRADAuthenticatedResponse : Response<RelationshipRoleWrapper>
    {

        public RelationshipRoleCRADAuthenticatedResponse(RelationshipRoleCRADBaseRequest request) : base()
        {
            Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public SocialRelationship SocialRelationship
        {
            get { return Value.SocialRelationship; }
        }

    }
}
