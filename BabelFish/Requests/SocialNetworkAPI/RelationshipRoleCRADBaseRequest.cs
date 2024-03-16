using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.SocialNetwork;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.SocialNetworkAPI
{
    public class RelationshipRoleCRADBaseRequest : Request 
    {
        public RelationshipRoleCRADBaseRequest(string operationId, UserAuthentication credentials) : base(operationId, credentials) {
            this.RequiresCredentials = true;
            this.SubDomain = APISubDomain.AUTHAPI;
        }

        public string ActiveId { get; set; } = "";
        public string PassiveId { get; set; } = "";

        public SocialRelationshipName RelationshipName { get; set; }

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/social-network/{EnumHelper.GetAttributeOfType<EnumMemberAttribute>(RelationshipName).Value}"; }
        }

    }
}
