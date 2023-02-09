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
        public Dictionary<string, AttributeValueDataPacket> AttributeValues {
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
            AttributeValueDataPacket avw;
            if (Value.AttributeValues.TryGetValue( attributeDefinitionSetName, out avw)) {
                if ( avw.StatusCode == System.Net.HttpStatusCode.OK ) {
                    attributeValue = avw.AttributeValue; ;
                    return true;
                }
            }
            return false;
        }

        public bool TryGetAttributeValueWrapper( string attributeDefinitionSetName, out AttributeValueDataPacket attributeValueWrapper ) {
            if (Value.AttributeValues.TryGetValue( attributeDefinitionSetName, out attributeValueWrapper )) {
                return true;
            }
            return false;
        }

        /// <inheritdoc/>
        protected override void ConvertBodyToValue() {
            Value = new AttributeValuesWrapper();

            try {
                JObject o = JObject.Parse( Body.ToString() );
                var attributeValueResponses = (JObject) o["attribute-values"];
                foreach( var attributeValueResponseJObject in attributeValueResponses.Properties()) {
                    var setNameStr = attributeValueResponseJObject.Name;
                    var attrValueWrapperJObject = (JObject)attributeValueResponseJObject.Value;

                    AttributeValueDataPacket avWrapper = new AttributeValueDataPacket();
                    Value.AttributeValues.Add( setNameStr, avWrapper );

                    avWrapper.SetName = SetName.Parse( setNameStr );
                    avWrapper.StatusCode = (HttpStatusCode)Enum.Parse( typeof( HttpStatusCode) , ( string)attrValueWrapperJObject["statusCode"] );
                    if (attrValueWrapperJObject.ContainsKey( "Message" ) )
                        avWrapper.Message = (string) attrValueWrapperJObject.GetValue( "Message" )[0];
                    if (avWrapper.StatusCode == HttpStatusCode.OK) {
                        avWrapper.AttributeValue = new AttributeValue( setNameStr, (JObject) attrValueWrapperJObject.GetValue( "attribute-value" ) );
                        avWrapper.Visibility = (VisibilityOption)Enum.Parse( typeof( VisibilityOption ), (string)attrValueWrapperJObject["visibility"] );
                    }
                }
                /*
                string otype = o.Type.ToString();
                foreach (JProperty property in o.Properties()) {
                    if (property.Name == OBJECT_LIST_NAME) {
                        JObject o2 = JObject.Parse( property.Value.ToString() );
                        foreach (JProperty property2 in o2.Properties()) {
                            captureErrors.Clear();
                            AttributeValue buildAttribute = new AttributeValue( property2.Name );

                            JObject o3 = JObject.Parse( property2.Value.ToString() );
                            foreach (JProperty property3 in o3.Properties()) {
                                switch (property3.Name) {
                                    case "Message": //Getting an error back
                                        this.MessageResponse.Title = "GetAttributeValue API errors encountered";
                                        this.MessageResponse.Message.Add( property3.Value.ToString() );
                                        break;
                                    case "ResponseCodes":
                                        this.MessageResponse.ResponseCodes.Add( property3.Value.ToString() );
                                        break;
                                    case "statusCode":
                                        buildAttribute.StatusCode = property3.Value.ToString();
                                        break;
                                    case "attribute-value":
                                        if (buildAttribute.IsMultipleValue) {
                                            string keyFieldName = buildAttribute.GetDefinitionKeyFieldName();
                                            var objectList = Helpers.JsonHelper.Deserialize( property3.Value.ToString() );
                                            foreach (Dictionary<string, dynamic> eachObject in (List<object>)objectList) {
                                                if (eachObject.ContainsKey( keyFieldName )) {
                                                    foreach (KeyValuePair<string, dynamic> item in eachObject)
                                                        buildAttribute.SetFieldValue( item.Key,
                                                                                item.Value,
                                                                                eachObject[keyFieldName] );
                                                } else {
                                                    captureErrors.Add( $"{keyFieldName} not found for MultipleValues Field in {property2.Name}" );
                                                }
                                            }
                                        } else {
                                            if (property3.Name == "attribute-value") {
                                                JObject o4 = JObject.Parse( property3.Value.ToString() );
                                                foreach (JProperty property4 in o4.Properties()) {
                                                    buildAttribute.SetFieldValue( property4.Name, property4.Value );
                                                }
                                            }
                                        }
                                        break;
                                    case "visibility":
                                        if (VisibilityOption.TryParse( property3.Value.ToString(), out VisibilityOption checkVis ))
                                            buildAttribute.Visibility = checkVis;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            if (captureErrors.Count > 0)
                                captureErrors.Add( "I'm not sure yet" );
                            else if (this.MessageResponse.Message.Count == 0)
                                returnAttributes.Add( buildAttribute );
                        }
                    }
                }
                */
            } catch (Exception ex) {
                logger.Error( ex );
            }

        }
    }
}