using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.BabelFish.Requests.Authentication
{
    public class GetCognitoLoginRequest : Request
    {
        public GetCognitoLoginRequest()
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
