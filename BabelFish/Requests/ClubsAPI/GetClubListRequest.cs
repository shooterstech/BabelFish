using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Requests;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.ClubsAPI {
    public class GetClubListRequest : Request, IToken {


        public GetClubListRequest(UserCredentials credentials) : base( "GetClubList", credentials ) {
            //NOTE: Because this request requires user credentials, we're only writing one constructor that includes parameters for crendentials.
            this.RequiresCredentials = true;
            this.SubDomain = Runtime.APISubDomain.AUTHAPI;
        }

        /// <inheritdoc />
        public string Token { get; set; }


        /// <inheritdoc />
        public override string RelativePath {

            get { return $"/clubs"; }
        }

        public override Dictionary<string, List<string>> QueryParameters {
            get {

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();

                if (! string.IsNullOrEmpty( Token ) ) {
                    parameterList.Add( "token", new List<string> { Token } );
                }

                return parameterList;
            }
        }
    }
}
