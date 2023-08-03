using System.Net;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.Requests.AttributeValueAPI;
using Scopos.BabelFish.DataModel.Definitions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;

namespace Scopos.BabelFish.Responses.AttributeValueAPI {
    public class GetAttributeValuePublicResponse : Response<AttributeValuesWrapper> {

        private Logger logger = LogManager.GetCurrentClassLogger();

        public GetAttributeValuePublicResponse( GetAttributeValuePublicRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public Dictionary<string, AttributeValueDataPacketAPIResponse> AttributeValues {
            get { return Value.AttributeValues; }
        }

        /// <summary>
        /// Deserilization of a AttributeValueDataPacket is handled by the overridden ReadJson()
        /// method of AttributeValueDataPacketConverter class. Because to deserialize an AttributeValue
        /// the Definition of the Attribute must be known. And reading the Definition is an IO bound
        /// Async call. But ReadJson() is not Async and can't be made async because it is overridden.
        /// To get around this limitation, the Task is assigned to AttributeValueTask (instead
        /// of awaiting and assigning to AttributeValue. The awaiting of AttributeValueTask
        /// is then handled in an async call sepeartly.
        /// </summary>
        /// <returns></returns>
        protected internal async Task PostResponseProcessingAsync() {
            foreach ( var attributeValue in AttributeValues.Values ) {
                if (attributeValue.AttributeValueTask != null) {
                    await attributeValue.FinishInitializationAsync();
                }
            }
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
                    attributeValue = avw.AttributeValue;
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