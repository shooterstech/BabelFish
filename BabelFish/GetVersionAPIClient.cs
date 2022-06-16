using BabelFish.Requests.Misc;
using BabelFish.Responses.Misc;
using BabelFish.Helpers;

namespace BabelFish.GetVersionAPI
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
        /// <param name="services"></param>
        /// <param name="level"></param>
        /// <returns>VersionList object</returns>
        public async Task<GetVersionResponse> GetVersionAsync(List<VersionService> services, VersionLevel level) {

            GetVersionResponse response = new GetVersionResponse();

            GetVersionRequest requestParameters = new GetVersionRequest(services, level);

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// GetVersion API for one service
        /// </summary>
        /// <param name="service"></param>
        /// <param name="level"></param>
        /// <returns>VersionList object</returns>
        public async Task<GetVersionResponse> GetVersionAsync(VersionService service, VersionLevel level)
        {
            GetVersionResponse response = new GetVersionResponse();

            response = await GetVersionAsync(new List<VersionService>() { service }, level).ConfigureAwait(false);

            return response;
        }

    }
}
