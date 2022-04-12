using BabelFish.Requests.GetSetAttributeValueAPI;
using BabelFish.Responses.GetSetAttributeValueAPI;

namespace BabelFish {
    public class GetSetAttributeValueAPIClient : APIClient {

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="apiKey"></param>
        private GetSetAttributeValueAPIClient(string apiKey) : base(apiKey) { }

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="xapikey">Your assigned XApiKey</param>
        /// <param name="CustomUserSettings">Dictionary<string,string> of Allowed User Settings</param>
        public GetSetAttributeValueAPIClient(string xapikey, Dictionary<string, string> CustomUserSettings) : base(xapikey, CustomUserSettings) { }

        /// <summary>
        /// Get Attribute Value API
        /// </summary>
        /// <param name="AttributeNames">List<string> of valid Attribute Names</string></param>
        /// <returns>List of Attribute objects</returns>
        public async Task<GetAttributeValueResponse> GetAttributeValueAsync(List<string> AttributeNames) //, GetAttributeResponse response)
        {
            GetAttributeValueResponse response = new GetAttributeValueResponse();

            GetAttributeValueRequest requestParameters = new GetAttributeValueRequest(AttributeNames);

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }

        public async Task<GetValidateUserIDResponse> GetValidateUserIDAsync(string userID)
        {
            GetValidateUserIDResponse response = new GetValidateUserIDResponse();

            GetValidateUserIDRequest requestParameters = new GetValidateUserIDRequest(userID);

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }

        //public async Task<SetAttributeValueResponse> SetAttributeValueAsync(List<Dictionary<string, dynamic>> AttributeNamesValues)
        //{
        //    //Build in client call
        //    List<Dictionary<string, dynamic>> requestParameters = new List<Dictionary<string, dynamic>>();
        //    requestParameters.Add(new Dictionary<string, dynamic>() {
        //        { "attribute-def", "v1.0:orion:Date of Birth" },
        //        { "Visibilty", "PROTECTED" },
        //        { "attribute-value", new Dictionary<string,string>(){ { "DateOfBirth", "2010-03-03" } } },
        //    });
        //}
    }
}
