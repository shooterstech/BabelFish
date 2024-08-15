using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Requests.AttributeValueAPI;
using Scopos.BabelFish.Responses.AttributeValueAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.APIClients {
    public class AttributeValueAPIClient : APIClient<AttributeValueAPIClient> {

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="apiKey"></param>
        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        public AttributeValueAPIClient( ) : base() {

            //AttributeValueAPIClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
        }

        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        public AttributeValueAPIClient( APIStage apiStage ) : base( apiStage ) {

            //AttributeValueAPIClient does not support file system cache
            LocalStoreDirectory = null;
            IgnoreFileSystemCache = true;
        }

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

        public async Task<GetAttributeValueAuthenticatedResponse> GetAttributeValueAuthenticatedAsync( SetName attributeName, UserAuthentication credentials ) {

            List<SetName> attributeSetNames = new List<SetName>() {
                attributeName
            };

            return await this.GetAttributeValueAuthenticatedAsync( attributeSetNames, credentials );
        }

        public async Task<GetAttributeValueAuthenticatedResponse> GetAttributeValueAuthenticatedAsync( string attributeName, UserAuthentication credentials ) {

            List<SetName> attributeSetNames = new List<SetName>() {
                SetName.Parse(attributeName)
            };

            return await this.GetAttributeValueAuthenticatedAsync( attributeSetNames, credentials );
        }


        /// <summary>
        /// Get Attribute Value API
        /// </summary>
        /// <param name="requestParameters">GetAttributeValueRequest</param>
        /// <returns>List of Attribute objects</returns>
        public async Task<GetAttributeValuePublicResponse> GetAttributeValuePublicAsync( GetAttributeValuePublicRequest requestParameters ) {

			GetAttributeValuePublicResponse response = new GetAttributeValuePublicResponse( requestParameters );

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
		public async Task<GetAttributeValuePublicResponse> GetAttributeValuePublicAsync( List<string> attributeNamesAsStrings, string userId ) {

			//Convert attributeNamesAsStrings to a list of SetNames
			List<SetName> attributeSetNames = new List<SetName>();
			foreach (var attributeNameAsString in attributeNamesAsStrings) {
				attributeSetNames.Add( SetName.Parse( attributeNameAsString ) );
			}

			GetAttributeValuePublicRequest requestParameters = new GetAttributeValuePublicRequest( ) {
				AttributeNames = attributeSetNames,
				UserId = userId
			};

			return await GetAttributeValuePublicAsync( requestParameters );
		}

		/// <summary>
		/// Retreives a list of AttributeValues for the passed in user identified by the credentials.
		/// </summary>
		/// <param name="attributeNames">List of attribute definition set names to pull back for the user.</param>
		/// <param name="credentials">The authenticated user to reteive the attribute values for.</param>
		/// <returns></returns>
		public async Task<GetAttributeValuePublicResponse> GetAttributeValuePublicAsync( List<SetName> attributeNames, string userId ) {

			GetAttributeValuePublicRequest requestParameters = new GetAttributeValuePublicRequest( ) {
				AttributeNames = attributeNames,
                UserId = userId
			};

			return await GetAttributeValuePublicAsync( requestParameters );
        }

        public async Task<GetAttributeValuePublicResponse> GetAttributeValuePublicAsync( SetName attributeName, string userId ) {

            List<SetName> attributeSetNames = new List<SetName>() {
                attributeName
            };

            return await this.GetAttributeValuePublicAsync( attributeSetNames, userId );
        }

        public async Task<GetAttributeValuePublicResponse> GetAttributeValuePublicAsync( string attributeName, string userId ) {

            List<SetName> attributeSetNames = new List<SetName>() {
                SetName.Parse(attributeName)
            };

            return await this.GetAttributeValuePublicAsync( attributeSetNames, userId );
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
