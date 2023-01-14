using Scopos.BabelFish.DataModel.GetSetAttributeValue;
using Scopos.BabelFish.Requests.GetSetAttributeValueAPI;
using Newtonsoft.Json.Linq;

namespace Scopos.BabelFish.Responses.GetSetAttributeValueAPI
{
    public class SetAttributeValueResponse : Response<SetAttributeValueList>
    {
        private const string OBJECT_LIST_NAME = "attribute-value-responses";

        public SetAttributeValueResponse( SetAttributeValueRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public List<SetAttributeValue> SetAttributeValues
        {
            get { return Value.SetAttributeValues; }
        }

        /// <summary>
        /// Retrieve an object of the SetAttribute requested
        /// </summary>
        /// <param name="AttributeValueSetName"></param>
        /// <returns>SetAttributeValue response object if found</returns>
        public SetAttributeValue GetAttributeValue(string AttributeValueSetName)
        {
            //loop AttributeValues and return the one object they ask for
            return SetAttributeValues.Where(x => x.AttributeValue == AttributeValueSetName).FirstOrDefault();
        }

        protected override void ConvertBodyToValue() {
            SetAttributeValueList returnList = new SetAttributeValueList();
            List<SetAttributeValue> returnAttributes = new List<SetAttributeValue>();
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
                            SetAttributeValue buildAttribute = new SetAttributeValue();

                            JObject o3 = JObject.Parse(property2.Value.ToString());
                            buildAttribute.AttributeValue = property2.Name;
                            foreach (JProperty property3 in o3.Properties())
                            {
                                switch (property3.Name)
                                {
                                    case "Message":
                                        buildAttribute.Message.Add(property3.Value.ToString());
                                        break;
                                    case "statusCode":
                                        buildAttribute.StatusCode = property3.Value.ToString();
                                        break;
                                    default:
                                        break;
                                }
                            }
                            returnAttributes.Add(buildAttribute);
                        }
                    }
                }
                returnList.SetAttributeValues = returnAttributes;
            }
            catch (Exception ex)
            {
                CaptureErrors.Add(AttributeValueException.GetExceptionJSONParseError($": {ex.ToString()}"));
            }

            Value = returnList;
            if (CaptureErrors.Count > 0)
                CaptureErrors.ForEach(x => this.MessageResponse.Message.Add(x.ToString()));
        }
    }
}