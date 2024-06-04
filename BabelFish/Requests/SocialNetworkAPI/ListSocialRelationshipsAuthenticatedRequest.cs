using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.SocialNetwork;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Requests;
using Scopos.BabelFish.Runtime.Authentication;
using System.Data;
using System.Runtime.Serialization;

namespace Scopos.BabelFish.Requests.SocialNetworkAPI
{
    public class ListSocialRelationshipsAuthenticatedRequest : Request, ITokenRequest
    {
        public ListSocialRelationshipsAuthenticatedRequest(UserAuthentication credentials) : base("ListSocialRelationships", credentials)
        {
            this.RequiresCredentials = true;
            this.SubDomain = APISubDomain.AUTHAPI;
            HttpMethod = HttpMethod.Get;
        }

        public bool AsActive { get; set; } = false;
        public bool AsPassive { get; set; } = false;
        public bool IncomingRequests { get; set; } = false;
        public bool OutgoingRequests { get; set; } = false;
        public string Token { get; set; } = string.Empty;
        public int Limit { get; set; } = 0;

        public SocialRelationshipName RelationshipName { get; set; }

        public override Request Copy()
        {
            var newRequest = new ListSocialRelationshipsAuthenticatedRequest(Credentials);
            newRequest.AsActive = AsActive;
            newRequest.AsPassive = AsPassive;
            newRequest.IncomingRequests = IncomingRequests;
            newRequest.OutgoingRequests = OutgoingRequests;
            newRequest.Token = Token;
            newRequest.Limit = Limit;
            newRequest.RelationshipName = RelationshipName;
            return newRequest;
        }

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/social-network/{EnumHelper.GetAttributeOfType<EnumMemberAttribute>(RelationshipName).Value}/list"; }
        }


        public override Dictionary<string, List<string>> QueryParameters
        {
            get
            {

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();

                if (!string.IsNullOrEmpty(Token))
                {
                    parameterList.Add("token", new List<string> { Token });
                }

                if (Limit > 0)
                    parameterList.Add("limit", new List<string> { Limit.ToString() });

                parameterList.Add("as-active", new List<string>() { AsActive.ToString() });
                parameterList.Add("as-passive", new List<string>() { AsPassive.ToString() });
                parameterList.Add("incoming-requests", new List<string>() { IncomingRequests.ToString() });
                parameterList.Add("outgoing-requests", new List<string>() { OutgoingRequests.ToString() });

                return parameterList;
            }
        }

    }
}