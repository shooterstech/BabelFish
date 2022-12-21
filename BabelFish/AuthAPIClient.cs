using ShootersTech.BabelFish.Requests.Authentication;
using ShootersTech.BabelFish.Responses.Authentication;

namespace ShootersTech.BabelFish.Authentication {

    [Obsolete("Let EKA know if anyone is using this class.")]
    public class AuthAPIClient : APIClient {

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="apiKey"></param>
        [Obsolete("Let EKA know if anyone is using this constructor.")]
        public AuthAPIClient(string apiKey) : base(apiKey) { }

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="xapikey">Your assigned XApiKey</param>
        /// <param name="CustomUserSettings">Dictionary<string,string> of Allowed User Settings</param>
        [Obsolete( "Let EKA know if anyone is using this constructor." )]
        public AuthAPIClient(string xapikey, Dictionary<string, string> CustomUserSettings) : base(xapikey, CustomUserSettings) { }

        [Obsolete( "Let EKA know if anyone is using this method." )]
        public async Task<GetCognitoLoginResponse> CognitoLoginAsync() {

            GetCognitoLoginResponse response = new GetCognitoLoginResponse(new GetCognitoLoginRequest());

            GetCognitoLoginRequest requestParameters = new GetCognitoLoginRequest();

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }
    }
}
