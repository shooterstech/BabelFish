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
        public AttributeValueAPIClient(string xapikey ) : base( xapikey ) { }

        public AttributeValueAPIClient( string xapikey, APIStage apiStage ) : base( xapikey, apiStage ) { }

        /// <summary>
        /// Get Attribute Value API
        /// </summary>
        /// <param name="requestParameters">GetAttributeValueRequest</param>
        /// <returns>List of Attribute objects</returns>
        public async Task<GetAttributeValueAuthenticatedResponse> GetAttributeValueAsync(GetAttributeValueAuthenticatedRequest requestParameters) {

            GetAttributeValueAuthenticatedResponse response = new GetAttributeValueAuthenticatedResponse(requestParameters);

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Get Attribute Value API
        /// </summary>
        /// <param name="AttributeNames">List<string> of valid Attribute Names</string>. Each attribute name must be formatted as a Set Name. </param>
        /// <returns>List of Attribute objects</returns>
        public async Task<GetAttributeValueAuthenticatedResponse> GetAttributeValueAsync(List<string> attributeNames, UserAuthentication credentials) {
            
            GetAttributeValueAuthenticatedRequest requestParameters = new GetAttributeValueAuthenticatedRequest(credentials)
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
        public async Task<GetValidateUserIDAuthenticatedResponse> GetValidateUserIDAsync(GetValidateUserIDAuthenticatedRequest requestParameters) {

            GetValidateUserIDAuthenticatedResponse response = new GetValidateUserIDAuthenticatedResponse(requestParameters);

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Get UserId Validity
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>ValidateUserId object</returns>
        public bool GetValidateUserIDAsync(string userID) {

            GetValidateUserIDAuthenticatedRequest requestParameters = new GetValidateUserIDAuthenticatedRequest()
            {
                UserID = userID
            };

            var taskResponse = GetValidateUserIDAsync(requestParameters);
            var responses = taskResponse.Result;
            return responses.ValidateUserID.Valid;
        }

        /*
        /// <summary>
        /// Assemble and Set 1 or more AttributeValue items
        /// </summary>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public async Task<SetAttributeValueAuthenticatedResponse> SetAttributeValueAsync(AttributeValueList attributeValue, UserAuthentication credentials )
        {

            SetAttributeValueAuthenticatedRequest requestParameters = new SetAttributeValueAuthenticatedRequest(attributeValue, credentials);

            SetAttributeValueAuthenticatedResponse response = new SetAttributeValueAuthenticatedResponse( requestParameters );

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Assemble and Set 1 AttributeValue
        /// </summary>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public async Task<SetAttributeValueAuthenticatedResponse> SetAttributeValueAsync(AttributeValue attributeValue, UserAuthentication credentials )
        {
            
            AttributeValueList newAttribute = new AttributeValueList();
            newAttribute.Attributes.Add(attributeValue);

            var response = await SetAttributeValueAsync(newAttribute, credentials).ConfigureAwait(false);

            return response;
        }
        */

    }
}
