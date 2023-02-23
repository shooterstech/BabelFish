using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

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

        public Scopos.BabelFish.DataModel.Definitions.Attribute FetchAttributeDefinition( string attributeDef ) {

            if (string.IsNullOrEmpty( xApiKey )) {
                throw new XApiKeyNotSetException();
            }

            var setName = SetName.Parse( attributeDef );

            return FetchAttributeDefinition( setName );
        }

        public Scopos.BabelFish.DataModel.Definitions.Attribute FetchAttributeDefinition( SetName attributeDef ) {

            if (string.IsNullOrEmpty( xApiKey )) {
                throw new XApiKeyNotSetException();
            }

            var taskResponse = client.GetAttributeDefinitionAsync( attributeDef );
            var response = taskResponse.Result;
            return response.Definition;
        }
    }
}
