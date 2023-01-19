using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.Requests.AttributeValueAPI;
using Scopos.BabelFish.Responses.AttributeValueAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.APIClients {
    public class AttributeValueAPIClient : APIClient {

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="apiKey"></param>
        private AttributeValueAPIClient(string apiKey) : base(apiKey) { }

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
        public async Task<GetAttributeValueResponse> GetAttributeValueAsync(List<string> attributeNames, UserCredentials credentials) {
            
            GetAttributeValueRequest requestParameters = new GetAttributeValueRequest(credentials)
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
        public async Task<SetAttributeValueResponse> SetAttributeValueAsync(AttributeValueList attributeValue, UserCredentials credentials )
        {

            SetAttributeValueRequest requestParameters = new SetAttributeValueRequest(attributeValue, credentials);

            SetAttributeValueResponse response = new SetAttributeValueResponse( requestParameters );

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Assemble and Set 1 AttributeValue
        /// </summary>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public async Task<SetAttributeValueResponse> SetAttributeValueAsync(AttributeValue attributeValue, UserCredentials credentials )
        {
            
            AttributeValueList newAttribute = new AttributeValueList();
            newAttribute.Attributes.Add(attributeValue);

            var response = await SetAttributeValueAsync(newAttribute, credentials).ConfigureAwait(false);

            return response;
        }

    }
}
