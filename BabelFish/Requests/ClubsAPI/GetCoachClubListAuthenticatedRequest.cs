using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Requests.ClubsAPI
{
    public class GetCoachClubListAuthenticatedRequest : Request, ITokenRequest
    {
        public GetCoachClubListAuthenticatedRequest(UserAuthentication credentials) : base("GetCoachClubList", credentials)
        {
            this.RequiresCredentials = true;
            HttpMethod = HttpMethod.Get;
            this.SubDomain = APISubDomain.AUTHAPI;
        }

        /// <inheritdoc />
        public string Token { get; set; }


        /// <inheritdoc />
        public int Limit { get; set; } = 50;

        /// <inheritdoc />
        public override string RelativePath
        {

            get { return $"/clubs/coach/me"; }
        }

        /// <inheritdoc />
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

                return parameterList;
            }
        }

    }
}



