using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json.Linq;
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

        public string ActiveId { get; set; }
        public string PassiveId { get; set; }

        public SocialRelationshipName RelationshipName { get; set; }

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/social-network/{EnumHelper.GetAttributeOfType<EnumMemberAttribute>(RelationshipName).Value}"; }
        }

        public override Dictionary<string, List<string>> QueryParameters
        {
            get
            {

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();
                if (!string.IsNullOrEmpty(ActiveId))
                    parameterList.Add("active-id", new List<string>() { ActiveId });
                if (!string.IsNullOrEmpty(PassiveId))
                    parameterList.Add("passive-id", new List<string>() { PassiveId });

                return parameterList;
            }
        }

    }
}
