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
        public async Task<GetAttributeValueResponse> GetAttributeValueAsync(List<string> AttributeNames)
        {
            GetAttributeValueResponse response = new GetAttributeValueResponse();

            GetAttributeValueRequest requestParameters = new GetAttributeValueRequest(AttributeNames);

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Get UserId Validity
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>ValidateUserId object</returns>
        public async Task<GetValidateUserIDResponse> GetValidateUserIDAsync(string userID)
        {
            GetValidateUserIDResponse response = new GetValidateUserIDResponse();

            GetValidateUserIDRequest requestParameters = new GetValidateUserIDRequest(userID);

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        //public async Task<GetAttributeValueResponse> SetAttributeValueAsync(DataModel.GetSetAttributeValue.AttributeValue attributeValue)
        //{
        //    GetAttributeValueResponse response = new GetAttributeValueResponse();

        //    //set request with attributeValue

        //    //CallAPI 
        //}
    }
}
