using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.Threading;
using NLog;

namespace Scopos.BabelFish.DataModel.AttributeValue {
    /// <summary>
    /// The AttributeValueDefinitionFetch is effectively a facade for the Definition API Client, and one
    /// that only retreives Attribute Value definitions. 
    /// 
    /// this class is expected to be used within the instantiation of Attribute Values, which require the 
    /// knowledge of the attribute definition. 
    /// 
    /// Access to the class is through the static class variable FETCHER
    /// 
    /// To use this class, the FETCHER's .XApiKey property must be set first.
    /// </summary>
    public class AttributeValueDefinitionFetcher {

        public static AttributeValueDefinitionFetcher FETCHER = new AttributeValueDefinitionFetcher();

        private string xApiKey = "";
        private DefinitionAPIClient client = null;

        private Logger logger = LogManager.GetCurrentClassLogger();

        private AttributeValueDefinitionFetcher() { }

        /// <summary>
        /// Sets the x-api-key to use when making calls to read new attribute definitions.
        /// Also reinstntiates the definition api client, using the new x-api-key.
        /// </summary>
        public string XApiKey { 
            get {
                return xApiKey;
            }
            set {
                if (xApiKey != value && !string.IsNullOrEmpty( value ) ) {
                    xApiKey = value;
                    client = new DefinitionAPIClient( xApiKey );
                }
            }
        }

        /// <summary>
        /// Returns a boolean indicating if the X API Key has been set.
        /// This only validates that it's been set, not that the value is a good one.
        /// </summary>
        public bool IsXApiKeySet {
            get {
                return ! string.IsNullOrEmpty( xApiKey );
            }
        }

        public async Task<Scopos.BabelFish.DataModel.Definitions.Attribute> FetchAttributeDefinitionAsync( string attributeDef ) {

            if (string.IsNullOrEmpty( xApiKey )) {
                throw new XApiKeyNotSetException();
            }

            var setName = SetName.Parse( attributeDef );

            return await FetchAttributeDefinitionAsync( setName );
        }

        public async Task<Scopos.BabelFish.DataModel.Definitions.Attribute> FetchAttributeDefinitionAsync( SetName attributeDef ) {

            if (string.IsNullOrEmpty( xApiKey )) {
                throw new XApiKeyNotSetException();
            }

            var response = await client.GetAttributeDefinitionAsync( attributeDef );
            if (response.StatusCode == System.Net.HttpStatusCode.OK )
                return response.Definition;

            //If we get here, likely a definition not found error.
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                throw new AttributeNotFoundException( $"Attribute Definition for '{attributeDef}' could not be found.", logger );

            throw new AttributeValueException( response.MessageResponse.ToString(), logger );
        }
    }
}
