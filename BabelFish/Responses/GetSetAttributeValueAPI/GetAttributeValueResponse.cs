using BabelFish.DataModel.GetSetAttributeValue;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BabelFish.Responses.GetSetAttributeValueAPI
{
    public class GetAttributeValueResponse : Response<AttributeValueList>
    {
        private const string OBJECT_LIST_NAME = "attribute-values";

        public GetAttributeValueResponse() { }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public List<AttributeValue> AttributeValues
        {
            get { return Value.Attributes; }
        }

        /// <summary>
        /// Retrieve an object of the Attribute requested
        /// </summary>
        /// <param name="AttributeDefinitionSetName"></param>
        /// <returns>AttributeValue object if found</returns>
        public AttributeValue GetAttributeValue(string AttributeDefinitionSetName)
        {
            //loop AttributeValues and return the one object they ask for
            return AttributeValues.Where(x => x.SetName == AttributeDefinitionSetName).FirstOrDefault();
        }

        /// <inheritdoc/>
        protected override void ConvertBodyToValue()
        {
            AttributeValueList returnList = new AttributeValueList();
            List<AttributeValue> returnAttributes = new List<AttributeValue>();
            List<string> CaptureErrors = new List<string>();

            try
            {
                JObject o = JObject.Parse(Body.ToString());
                string otype = o.Type.ToString();
                foreach (JProperty property in o.Properties())
                {
                    if (property.Name == OBJECT_LIST_NAME)
                    {
                        JObject o2 = JObject.Parse(property.Value.ToString());
                        foreach (JProperty property2 in o2.Properties())
                        {
                            CaptureErrors.Clear();
                            AttributeValue buildAttribute = new AttributeValue(property2.Name);

                            JObject o3 = JObject.Parse(property2.Value.ToString());
                            foreach (JProperty property3 in o3.Properties())
                            {
                                switch (property3.Name)
                                {
                                    case "Message": //Getting an error back
                                        this.MessageResponse.Title = "GetAttributeValue API errors encountered";
                                        this.MessageResponse.Message.Add(property3.Value.ToString());
                                        break;
                                    case "ResponseCodes":
                                        this.MessageResponse.ResponseCodes.Add(property3.Value.ToString());
                                        break;
                                    case "statusCode":
                                        buildAttribute.statusCode = property3.Value.ToString();
                                        break;
                                    case "attribute-value":
                                        if (buildAttribute.IsMultipleValue)
                                        {
                                            string KeyFieldName = buildAttribute.GetAttributeFieldKey();
                                            var ObjectList = Helpers.JsonHelper.Deserialize(property3.Value.ToString());
                                            foreach (Dictionary<string, dynamic> EachObject in (List<object>)ObjectList)
                                            {
                                                if (EachObject.ContainsKey(KeyFieldName))
                                                {
                                                    foreach (KeyValuePair<string, dynamic> item in EachObject)
                                                        buildAttribute.SetFieldName(item.Key,
                                                                                item.Value,
                                                                                EachObject[KeyFieldName]);
                                                }
                                                else
                                                {
                                                    CaptureErrors.Add(AttributeValueException.GetExceptionKeyFieldNameError($"{KeyFieldName} not found for MultipleValues Field in {property2.Name}"));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if ( property3.Name == "attribute-value")
                                            {
                                                JObject o4 = JObject.Parse(property3.Value.ToString());
                                                foreach (JProperty property4 in o4.Properties())
                                                {
                                                    buildAttribute.SetFieldName(property4.Name, property4.Value.ToString());
                                                }
                                            }
                                        }
                                        break;
                                    case "visibility":
                                        if (Helpers.VisibilityOption.TryParse(property3.Value.ToString(), out Helpers.VisibilityOption checkVis))
                                            buildAttribute.Visibility = checkVis;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            if (buildAttribute.LastException != "" || CaptureErrors.Count>0)
                                CaptureErrors.Add(buildAttribute.LastException);
                            else if (this.MessageResponse.Message.Count == 0)
                                returnAttributes.Add(buildAttribute);
                        }
                    }
                }
                returnList.Attributes = returnAttributes;
            } 
            catch(Exception ex)
            {
                CaptureErrors.Add(AttributeValueException.GetExceptionJSONParseError($": {ex.ToString()}"));
            }

            Value = returnList;
            if ( CaptureErrors.Count > 0 )
                CaptureErrors.ForEach(x => this.MessageResponse.Message.Add(x.ToString()));

        }
    }
}