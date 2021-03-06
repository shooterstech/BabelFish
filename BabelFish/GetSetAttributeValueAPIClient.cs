using ShootersTech.BabelFish.DataModel.GetSetAttributeValue;
using ShootersTech.BabelFish.Requests.GetSetAttributeValueAPI;
using ShootersTech.BabelFish.Responses.GetSetAttributeValueAPI;

namespace ShootersTech.BabelFish.GetSetAttributeValueAPI {
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
        /// <param name="requestParameters">GetAttributeValueRequest</param>
        /// <returns>List of Attribute objects</returns>
        public async Task<GetAttributeValueResponse> GetAttributeValueAsync(GetAttributeValueRequest requestParameters) {

            GetAttributeValueResponse response = new GetAttributeValueResponse(requestParameters);

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Get Attribute Value API
        /// </summary>
        /// <param name="AttributeNames">List<string> of valid Attribute Names</string>. Each attribute name must be formatted as a Set Name. </param>
        /// <returns>List of Attribute objects</returns>
        public async Task<GetAttributeValueResponse> GetAttributeValueAsync(List<string> attributeNames) {
            
            GetAttributeValueRequest requestParameters = new GetAttributeValueRequest()
            {
                AttributeNames = attributeNames
            };

            return await GetAttributeValueAsync(requestParameters);
        }

        /// <summary>
        /// Get UserId Validity
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns>ValidateUserId object</returns>
        public async Task<GetValidateUserIDResponse> GetValidateUserIDAsync(GetValidateUserIDRequest requestParameters) {

            GetValidateUserIDResponse response = new GetValidateUserIDResponse(requestParameters);

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Get UserId Validity
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>ValidateUserId object</returns>
        public async Task<GetValidateUserIDResponse> GetValidateUserIDAsync(string userID) {

            GetValidateUserIDRequest requestParameters = new GetValidateUserIDRequest()
            {
                UserID = userID
            };

            return await GetValidateUserIDAsync(requestParameters);
        }

        /// <summary>
        /// Assemble and Set 1 or more AttributeValue items
        /// </summary>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public async Task<SetAttributeValueResponse> SetAttributeValueAsync(AttributeValueList attributeValue)
        {

            SetAttributeValueRequest requestParameters = new SetAttributeValueRequest(attributeValue);

            SetAttributeValueResponse response = new SetAttributeValueResponse( requestParameters );

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Assemble and Set 1 AttributeValue
        /// </summary>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public async Task<SetAttributeValueResponse> SetAttributeValueAsync(AttributeValue attributeValue)
        {
            
            AttributeValueList newAttribute = new AttributeValueList();
            newAttribute.Attributes.Add(attributeValue);

            var response = await SetAttributeValueAsync(newAttribute).ConfigureAwait(false);

            return response;
        }

    }
}
