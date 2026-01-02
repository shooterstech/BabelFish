using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.Requests.Authentication.Credentials {

    public class GetCredentialsRequest : Request {
        public GetCredentialsRequest( string userName = "", string passWord = "" ) : base( "GetCognitoCredentials" ) {
            Username = userName;
            Password = passWord;

        }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/credentials"; }
        }

        public override Dictionary<string, List<string>> QueryParameters {
            get {
                return new Dictionary<string, List<string>>()
                    {{"username", new List<string> {Username}}, {"password", new List<string> {Password}}};
            }
        }
    }
}