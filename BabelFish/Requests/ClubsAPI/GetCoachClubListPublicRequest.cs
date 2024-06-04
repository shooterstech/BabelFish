using Scopos.BabelFish.APIClients;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Requests.ClubsAPI
{
    public class GetCoachClubListPublicRequest : Request, ITokenRequest
    {
        public GetCoachClubListPublicRequest() : base("GetCoachClubList")
        {
            this.RequiresCredentials = false;
            HttpMethod = HttpMethod.Get;
            this.SubDomain = APISubDomain.API;
        }

        public GetCoachClubListPublicRequest(string userId) : base("GetCoachClubList")
        {
            this.RequiresCredentials = false;
            HttpMethod = HttpMethod.Get;
            this.SubDomain = APISubDomain.API;
            UserID = userId;
        }

        /// <inheritdoc />
        public string Token { get ; set; }
        
        /// <inheritdoc />
        public string UserID { get; set; }

        /// <inheritdoc />
        public int Limit { get; set; } = 50;

        /// <inheritdoc />
        public override string RelativePath
        {

            get { return $"/clubs/coach/{UserID}"; }
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
