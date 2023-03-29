using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;
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
        public async Task<GetAttributeValueAuthenticatedResponse> GetAttributeValueAuthenticatedAsync( GetAttributeValueAuthenticatedRequest requestParameters ) {

            GetAttributeValueAuthenticatedResponse response = new GetAttributeValueAuthenticatedResponse( requestParameters );

            await this.CallAPIAsync( requestParameters, response );
            await response.PostResponseProcessingAsync();

            return response;
        }

        /// <summary>
        /// Retreives a list of AttributeValues for the passed in user identified by the credentials.
        /// </summary>
        /// <param name="attributeNamesAsStrings">List of attribute definition set names to pull back for the user. Each attribute name must be formatted as a Set Name. </param>
        /// <param name="credentials">The authenticated user to reteive the attribute values for.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown when one of the attributeNmesAsStrings can not be converted into a SetName</exception>"
        public async Task<GetAttributeValueAuthenticatedResponse> GetAttributeValueAuthenticatedAsync( List<string> attributeNamesAsStrings, UserAuthentication credentials ) {

            //Convert attributeNamesAsStrings to a list of SetNames
            List<SetName> attributeSetNames = new List<SetName>();
            foreach( var attributeNameAsString in attributeNamesAsStrings) {
                attributeSetNames.Add( SetName.Parse( attributeNameAsString ) );
            }

            GetAttributeValueAuthenticatedRequest requestParameters = new GetAttributeValueAuthenticatedRequest( credentials ) {
                AttributeNames = attributeSetNames
            };

            return await GetAttributeValueAuthenticatedAsync( requestParameters );
        }

        /// <summary>
        /// Retreives a list of AttributeValues for the passed in user identified by the credentials.
        /// </summary>
        /// <param name="attributeNames">List of attribute definition set names to pull back for the user.</param>
        /// <param name="credentials">The authenticated user to reteive the attribute values for.</param>
        /// <returns></returns>
        public async Task<GetAttributeValueAuthenticatedResponse> GetAttributeValueAuthenticatedAsync( List<SetName> attributeNames, UserAuthentication credentials ) {

            GetAttributeValueAuthenticatedRequest requestParameters = new GetAttributeValueAuthenticatedRequest( credentials ) {
                AttributeNames = attributeNames
            };

            return await GetAttributeValueAuthenticatedAsync( requestParameters );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SetAttributeValueAuthenticatedResponse> SetAttributeValueAuthenticatedAsync( SetAttributeValueAuthenticatedRequest request ) {

            SetAttributeValueAuthenticatedResponse response = new SetAttributeValueAuthenticatedResponse( request );

            await this.CallAPIAsync( request, response );

            return response;
        }
    }
}
