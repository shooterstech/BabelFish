using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootersTech.BabelFish.DataModel.Authentication.Credentials;

namespace ShootersTech.BabelFish.Responses.Authentication.Credentials
{
    public class GetCredentialsResponse : Response<Credential>
    {
        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public Credential Credential
        {
            get { return Value; }
        }
    }
}