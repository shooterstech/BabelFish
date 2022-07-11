using ShootersTech.Requests.Misc;
using ShootersTech.Responses.Misc;
using ShootersTech.Helpers;

namespace ShootersTech.GetVersionAPI
{
    public class GetVersionAPIClient : APIClient {

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="apiKey"></param>
        public GetVersionAPIClient(string apiKey) : base(apiKey) { }

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="xapikey">Your assigned XApiKey</param>
        /// <param name="CustomUserSettings">Dictionary<string,string> of Allowed User Settings</param>
        public GetVersionAPIClient(string xapikey, Dictionary<string, string> CustomUserSettings) : base(xapikey, CustomUserSettings) { }

        /// <summary>
        /// GetVersion API for multiple services
        /// </summary>
        /// <param name="requestParameters">GetVersionRequest object</param>
        /// <returns>List<VersionInfo> object</returns>
        public async Task<GetVersionResponse> GetVersionAsync(GetVersionRequest requestParameters) {

            GetVersionResponse response = new GetVersionResponse(requestParameters);

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// GetVersion API for one service
        /// </summary>
        /// <param name="service">VersionService enum</param>
        /// <param name="level">VersionLevel enum</param>
        /// <returns>List<VersionInfo> object</returns>
        public async Task<GetVersionResponse> GetVersionAsync(VersionService service, VersionLevel level) {
            GetVersionRequest requestParameters = new GetVersionRequest()
            {
                services = new List<VersionService>() { service },
                level = level
            };

            return await GetVersionAsync(requestParameters).ConfigureAwait(false);
        }

    }
}
