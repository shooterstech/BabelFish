using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Requests.AthenaLogin
{
    public class AthenaLogoutSessionAuthenticatedRequest : Request
    {



        public AthenaLogoutSessionAuthenticatedRequest(UserAuthentication credentials) : base("AthenaLogoutSession", credentials)
        {
            //NOTE: Because this request requires user credentials and a authcode, we're only writing one constructor that includes these two requirements.
            this.RequiresCredentials = true;
            this.SubDomain = APISubDomain.AUTHAPI;
            this.HttpMethod = HttpMethod.Post;
        }

        public List<string> ThingNames { get; set; }


        /// <inheritdoc />
        public override string RelativePath
        {

            get { return $"/target/login"; }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters
        {
            get
            {

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();
                if (ThingNames != null && ThingNames.Count > 0)
                    parameterList.Add("thing-name", ThingNames);


                return parameterList;
            }
        }
    }
}
