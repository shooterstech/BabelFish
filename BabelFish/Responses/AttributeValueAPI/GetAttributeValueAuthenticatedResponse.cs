using System.Net;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.Requests.AttributeValueAPI;
using Scopos.BabelFish.DataModel.Definitions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace Scopos.BabelFish.Responses.AttributeValueAPI {
    public class GetAttributeValueAuthenticatedResponse : Response<AttributeValuesWrapper> {

        private Logger logger = LogManager.GetCurrentClassLogger();

        public GetAttributeValueAuthenticatedResponse( GetAttributeValueAuthenticatedRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public Dictionary<string, AttributeValueDataPacketAPIResponse> AttributeValues {
            get { return Value.AttributeValues; }
        }

        /// <summary>
        /// Attempts to return the asked for AttributeValue. There are two reasons a False would
        /// be returned. 1) The set name was not part of the asked for list of attribute values in the request.
        /// 2) The server returned an error code for that specific attribute value. If this is the case use
        /// the function TryGetAttributeValueWrapper to return the full objects as sent by the server.
        /// </summary>
        /// <param name="attributeDefinitionSetName"></param>
        /// <param name="attributeValue"></param>
        /// <returns></returns>
        public bool TryGetAttributeValue( string attributeDefinitionSetName, out AttributeValue attributeValue ) {
            attributeValue = null;
            AttributeValueDataPacketAPIResponse avw;
            if (Value.AttributeValues.TryGetValue( attributeDefinitionSetName, out avw)) {
                if ( avw.StatusCode == System.Net.HttpStatusCode.OK ) {
                    attributeValue = avw.AttributeValue; ;
                    return true;
                }
            }
            return false;
        }

        public bool TryGetAttributeValueWrapper( string attributeDefinitionSetName, out AttributeValueDataPacketAPIResponse attributeValueWrapper ) {
            if (Value.AttributeValues.TryGetValue( attributeDefinitionSetName, out attributeValueWrapper )) {
                return true;
            }
            return false;
        }
    }
}