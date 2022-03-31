using BabelFish.DataModel.GetSetAttributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BabelFish.Responses.GetSetAttributeAPI
{
    public class GetAttributeResponse : Response<AttributeList>
    {
        private const string OBJECT_LIST_NAME = "attribute-values";

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public List<AttributeValue> AttributeValues
        {
            get { return Value.Attributes; }
        }

        /// <inheritdoc/>
        protected override void ConvertBodyToValue()
        {
            AttributeList returnList = new AttributeList();
            List<AttributeValue> returnAttributes = new List<AttributeValue>();

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
                            AttributeValue loopAttribute = new AttributeValue();

                            JObject o3 = JObject.Parse(property2.Value.ToString());
                            foreach (JProperty property3 in o3.Properties())
                            {
                                switch (property3.Name)
                                {
                                    case "Message":
                                        this.MessageResponse.Title = "GetAttributes API errors encountered";
                                        this.MessageResponse.Message.Add(property3.Value.ToString());
                                        loopAttribute.Name = property2.Name;
                                        break;
                                    case "ResponseCodes":
                                        this.MessageResponse.ResponseCodes.Add(property3.Value.ToString());
                                        break;
                                    case "statusCode":
                                        loopAttribute.statusCode = property3.Value.ToString();
                                        break;
                                    case "attribute-def":
                                        loopAttribute.Name = property3.Value.ToString();
                                        break;
                                    case "attribute-value":
                                        Dictionary<string, dynamic> loopaKVP = new Dictionary<string, dynamic>();
                                        object obj = Helpers.JsonHelper.Deserialize(property3.Value.ToString());
                                        if (obj.GetType().Name == "Dictionary`2")
                                            loopaKVP = (Dictionary<string, dynamic>)obj;
                                        else if (obj.GetType().Name == "List`1") {
                                            foreach (var val in (List<object>)obj)
                                                loopaKVP = (Dictionary<string, dynamic>)val;
                                        }
                                        foreach (KeyValuePair<string, dynamic> kvp in loopaKVP)
                                            loopAttribute.Values.Add(kvp.Key, kvp.Value);
                                        break;
                                    case "attribute-definition":
                                        JObject o5 = JObject.Parse(property3.Value.ToString());
                                        AttributeDefinition loopAttributeDefinition = new AttributeDefinition();
                                        foreach (JProperty property4 in o5.Properties()) {
                                            switch (property4.Name)
                                            {
                                                case "Discontinued":
                                                    loopAttributeDefinition.Discontinued = (bool)property4.Value;
                                                    break;
                                                case "MultipleValues":
                                                    loopAttributeDefinition.MultipleValues = (bool)property4.Value;
                                                    break;
                                                case "MaxVisibility":
                                                    if (Helpers.VisibilityOption.TryParse(property4.Value.ToString(), out Helpers.VisibilityOption checkVis2))
                                                        loopAttributeDefinition.MaxVisibility = checkVis2;
                                                    break;
                                                case "Fields":
                                                    List<AttributeDefinitionField> loopAttributeDefinitionField = new List<AttributeDefinitionField>();
                                                    loopAttributeDefinitionField = property4.Value.ToObject<List<AttributeDefinitionField>>();
                                                    loopAttributeDefinition.Fields = loopAttributeDefinitionField;
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                        loopAttribute.attributeDefinition = loopAttributeDefinition;
                                        break;
                                    case "visibility":
                                        if (Helpers.VisibilityOption.TryParse(property3.Value.ToString(), out Helpers.VisibilityOption checkVis))
                                            loopAttribute.Visibility = checkVis;
                                        break;
                                    default:
                                        break;
                                }
                            }
                            returnAttributes.Add(loopAttribute);
                        }
                    }
                }
                returnList.Attributes = returnAttributes;
            } finally { }

            Value = returnList;
        }
    }
}