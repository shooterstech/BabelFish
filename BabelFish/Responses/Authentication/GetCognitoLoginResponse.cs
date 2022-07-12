using ShootersTech.BabelFish.DataModel.Authentication;
using ShootersTech.BabelFish.Requests.Authentication;

namespace ShootersTech.BabelFish.Responses.Authentication
{
    public class GetCognitoLoginResponse : Response<AuthTokens>
    {

        public GetCognitoLoginResponse( GetCognitoLoginRequest request) {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public AuthTokens AuthTokens
        {
            get { return Value; }
        }
    }
}
