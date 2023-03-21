using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Requests;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Requests.ClubsAPI {
    public class GetClubListAuthenticatedRequest : Request, ITokenRequest {


        public GetClubListAuthenticatedRequest( UserAuthentication credentials ) : base( "GetClubList", credentials ) {
            //NOTE: Because this request requires user credentials, we're only writing one constructor that includes parameters for crendentials.
            this.RequiresCredentials = true;
            this.SubDomain = APISubDomain.AUTHAPI;
        }

        /// <inheritdoc />
        public string Token { get; set; }

        /// <inheritdoc />
        public int Limit { get; set; } = 0;

        /// <inheritdoc />
        public override Request Copy() {
            var newRequest = new GetClubListAuthenticatedRequest( Credentials );
            newRequest.Token = this.Token;

            return newRequest;
        }


        /// <inheritdoc />
        public override string RelativePath {

            get { return $"/clubs"; }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();

                if (! string.IsNullOrEmpty( Token ) ) {
                    parameterList.Add( "token", new List<string> { Token } );
                }
                if (Limit > 0)
                    parameterList.Add( "limit", new List<string> { Limit.ToString() } );

                return parameterList;
            }
        }
    }
}
