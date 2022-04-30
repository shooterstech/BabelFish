using System;
using System.Collections.Generic;
using System.Text;

namespace BabelFish.Requests.Authentication
{
    public class CognitoLoginRequest : Request
    {
        public CognitoLoginRequest()
        {
            WithAuthentication = true;
            IsShootersTechURI = false;
        }

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return ""; }
        }
    }
}
