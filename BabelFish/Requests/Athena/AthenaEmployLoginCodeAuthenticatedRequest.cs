using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Requests.Athena {
    public class AthenaEmployLoginCodeAuthenticatedRequest : Request {



        public AthenaEmployLoginCodeAuthenticatedRequest( string authCode, UserAuthentication credentials ) : base( "AthenaEmployLoginCode", credentials ) {
            //NOTE: Because this request requires user credentials and a authcode, we're only writing one constructor that includes these two requirements.
            this.RequiresCredentials = true;
            this.SubDomain = APISubDomain.AUTHAPI;
            this.HttpMethod = HttpMethod.Post;

            if (string.IsNullOrEmpty( authCode ))
                throw new ArgumentNullException( "The parameter authCode may not be null or an empty string." );

            AuthCode = authCode;
        }

        /// <summary>
        /// The Orion Account Owner Id to look up. 
        /// </summary>
        /// <example>OrionAcct001234</example>
        public string AuthCode { get; set; }


        /// <inheritdoc />
        public override string RelativePath {

            get { return $"/target/login"; }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();

                if (!string.IsNullOrEmpty( AuthCode )) {
                    parameterList.Add( "auth-code", new List<string> { AuthCode } );
                }

                return parameterList;
            }
        }
    }
}
