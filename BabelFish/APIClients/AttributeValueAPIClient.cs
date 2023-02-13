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
        public AttributeValueAPIClient( string xapikey ) : base( xapikey ) { }

        public AttributeValueAPIClient( string xapikey, APIStage apiStage ) : base( xapikey, apiStage ) { }

        /// <summary>
        /// Get Attribute Value API
        /// </summary>
        /// <param name="requestParameters">GetAttributeValueRequest</param>
        /// <returns>List of Attribute objects</returns>
        public async Task<GetAttributeValueAuthenticatedResponse> GetAttributeValueAsync( GetAttributeValueAuthenticatedRequest requestParameters ) {

            GetAttributeValueAuthenticatedResponse response = new GetAttributeValueAuthenticatedResponse( requestParameters );

            await this.CallAPI( requestParameters, response ).ConfigureAwait( false );

            return response;
        }

        /// <summary>
        /// Get Attribute Value API
        /// </summary>
        /// <param name="AttributeNames">List<string> of valid Attribute Names</string>. Each attribute name must be formatted as a Set Name. </param>
        /// <returns>List of Attribute objects</returns>
        public async Task<GetAttributeValueAuthenticatedResponse> GetAttributeValueAsync( List<string> attributeNames, UserAuthentication credentials ) {

            GetAttributeValueAuthenticatedRequest requestParameters = new GetAttributeValueAuthenticatedRequest( credentials ) {
                AttributeNames = attributeNames
            };

            return await GetAttributeValueAsync( requestParameters );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SetAttributeValueAuthenticatedResponse> SetAttributeValueAsync( SetAttributeValueAuthenticatedRequest request ) {

            SetAttributeValueAuthenticatedResponse response = new SetAttributeValueAuthenticatedResponse( request );

            await this.CallAPI( request, response ).ConfigureAwait( false );

            return response;
        }
    }
}
