using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Requests.Athena
{
    public class GetUserOwnershipValuesAuthenticatedRequest : Request
    {
        public GetUserOwnershipValuesAuthenticatedRequest(UserAuthentication credentials) : base("GetAttributeValue", credentials)
        {
            //NOTE: Because this request requires user credentials, we're only writing one constructor that includes parameters for crendentials.
            this.RequiresCredentials = true;
            this.SubDomain = APISubDomain.AUTHAPI;
        }

        public override string RelativePath
        {
            get { return $"/users/shared-key"; }
        }
    }
}
