using ShootersTech.BabelFish.Requests.Authentication;
using ShootersTech.BabelFish.Responses.Authentication;

namespace ShootersTech.BabelFish.Authentication {
    public class AuthAPIClient : APIClient {

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="apiKey"></param>
        public AuthAPIClient(string apiKey) : base(apiKey) { }

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="xapikey">Your assigned XApiKey</param>
        /// <param name="CustomUserSettings">Dictionary<string,string> of Allowed User Settings</param>
        public AuthAPIClient(string xapikey, Dictionary<string, string> CustomUserSettings) : base(xapikey, CustomUserSettings) { }

        public async Task<GetCognitoLoginResponse> CognitoLoginAsync() {

            GetCognitoLoginResponse response = new GetCognitoLoginResponse(new GetCognitoLoginRequest());

            GetCognitoLoginRequest requestParameters = new GetCognitoLoginRequest();

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }
    }
}
