using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Authentication;

namespace Scopos.BabelFish.Responses.Authentication.Credentials {
    public class GetCredentialsResponse : Response<AuthTokens> {

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public AuthTokens Credential {
            get { return Value; }
        }
    }
}