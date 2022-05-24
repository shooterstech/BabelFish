using BabelFish.DataModel.Authentication;

namespace BabelFish.Responses.Authentication
{
    public class GetCognitoLoginResponse : Response<AuthTokens>
    {
        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public AuthTokens AuthTokens
        {
            get { return Value; }
        }
    }
}
