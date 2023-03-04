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

        
        /// <inheritdoc/>
        protected override void ConvertBodyToValue() {
            base.ConvertBodyToValue();
            return;

            Value = new AttributeValuesWrapper();

            try {
                /*
                 * Deserializing the response manually for two reasons.
                 * 1) some of the property names from the response to the data model don't line up
                 * 2) (more importantly) there is no single data model for an attribute value. Since the data model
                 * is dynamic and specified by the Attribute Definition.
                 */
                JObject o = JObject.Parse( Body.ToString() );
                var attributeValueResponses = (JObject) o["attribute-values"];
                foreach( var attributeValueResponseJObject in attributeValueResponses.Properties()) {
                    var setNameStr = attributeValueResponseJObject.Name;
                    var attrValueWrapperJObject = (JObject)attributeValueResponseJObject.Value;

                    AttributeValueDataPacketAPIResponse avWrapper = new AttributeValueDataPacketAPIResponse();
                    Value.AttributeValues.Add( setNameStr, avWrapper );

                    avWrapper.AttributeDef = setNameStr;
                    avWrapper.StatusCode = (HttpStatusCode)Enum.Parse( typeof( HttpStatusCode) , ( string)attrValueWrapperJObject["statusCode"] );
                    if (attrValueWrapperJObject.ContainsKey( "Message" )) {
                        try {
                            avWrapper.Message = (string)attrValueWrapperJObject.GetValue( "Message" )[0];
                        } catch (Exception ex) {
                            //While there should be exactly 1 message in the Message array, catch for the chance there isn't. Swallow the error as it's not important enough to let the user know.
                            logger.Error( ex, $"Unable to read the Message property from the attribute value for {setNameStr}." );
                        }
                    }

                    if (avWrapper.StatusCode == HttpStatusCode.OK) {
                        avWrapper.AttributeValue = new AttributeValue( setNameStr, attrValueWrapperJObject.GetValue( "attribute-value" ) );
                        avWrapper.Visibility = (VisibilityOption)Enum.Parse( typeof( VisibilityOption ), (string)attrValueWrapperJObject["visibility"] );
                    }
                }

            } catch (Exception ex) {
                logger.Error( ex );
            }

        }
    }
}