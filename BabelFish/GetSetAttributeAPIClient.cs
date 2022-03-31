using BabelFish.Requests.GetSetAttributeAPI;
using BabelFish.Responses.GetSetAttributeAPI;

namespace BabelFish {
    public class GetSetAttributeAPIClient : APIClient {

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="apiKey"></param>
        private GetSetAttributeAPIClient(string apiKey) : base(apiKey) { }

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="xapikey">Your assigned XApiKey</param>
        /// <param name="CustomUserSettings">Dictionary<string,string> of Allowed User Settings</param>
        public GetSetAttributeAPIClient(string xapikey, Dictionary<string, string> CustomUserSettings) : base(xapikey, CustomUserSettings) { }

        /// <summary>
        /// Get Attribute Value API
        /// </summary>
        /// <param name="AttributeNames">List<string> of valid Attribute Names</string></param>
        /// <returns>List of Attribute objects</returns>
        public async Task<GetAttributeResponse> GetAttributeValueAsync(List<string> AttributeNames) //, GetAttributeResponse response)
        {
            GetAttributeResponse response = new GetAttributeResponse();

            GetAttributeRequest requestParameters = new GetAttributeRequest(AttributeNames);

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }

    }
}
