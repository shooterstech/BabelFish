using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabelFish.DataModel.Credentials;

namespace BabelFish.Responses.Credentials
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