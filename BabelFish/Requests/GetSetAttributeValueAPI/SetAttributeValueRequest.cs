using System.Text;
using BabelFish.DataModel.GetSetAttributeValue;
using Newtonsoft.Json;

namespace BabelFish.Requests.GetSetAttributeValueAPI
{
    public class SetAttributeValueRequest : Request
    {
        private Dictionary<string, Dictionary<string,string>> AttributesList = new Dictionary<string, Dictionary<string, string>>();
        private List<Dictionary<string, dynamic>> AttributeValues { get; set; } = new List<Dictionary<string, dynamic>>();

        private AttributeValueList AttributeToUpdate = new AttributeValueList();

        public SetAttributeValueRequest(AttributeValueList attributeToUpdate) //List<Dictionary<string,dynamic>> postParameters, List<string> queryParameters = null)
        {
            WithAuthentication = true;
            AttributeToUpdate = attributeToUpdate;
            // Validate Attributes were fuly generated correctly before submitting
            foreach ( AttributeValue checkAttribute in attributeToUpdate.Attributes)
                checkAttribute.ValidateForSubmit();
        }

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/users/attribute-value"; }
        }

        public override StringContent PostParameters
        {
            get
            {
                StringBuilder serializedJSON = new StringBuilder();
                try
                {
                    serializedJSON.Append("{\"attribute-values\": {");
                    bool firstrun = true;
                    foreach (AttributeValue attributeValue in AttributeToUpdate.Attributes)
                    {
                        if ( attributeValue.LastException == "")
                        {
                            if (!firstrun)
                                serializedJSON.Append(",");
                            serializedJSON.Append($"\"{attributeValue.SetName}\": {{");
                            serializedJSON.Append($"\"attribute-def\" : \"{attributeValue.SetName}\",");
                            if (attributeValue.Action != Helpers.AttributeValueActionEnums.EMPTY)
                                serializedJSON.Append($"\"action\" : \"{attributeValue.Action.ToString()}\",");
                            else
                                serializedJSON.Append($"\"action\" : \"\",");
                            serializedJSON.Append($"\"visibility\" : \"{attributeValue.Visibility.ToString()}\",");
                            serializedJSON.Append($"\"attribute-value\": ");
                            if (attributeValue.attributeValues.Count > 1)
                                serializedJSON.Append("[");
                            bool firstrun2 = true;
                            foreach (var attributeVal in attributeValue.attributeValues)
                            {
                                if (!firstrun2)
                                    serializedJSON.Append(",");
                                serializedJSON.Append(JsonConvert.SerializeObject(attributeVal.Value));
                                firstrun2 = false;
                            }
                            if (attributeValue.attributeValues.Count > 1)
                                serializedJSON.Append("]");
                            serializedJSON.Append("}");
                            firstrun = false;
                        }
                    }
                    serializedJSON.Append("}}");
                    
                    string check = serializedJSON.ToString();
                    return new StringContent(serializedJSON.ToString(), Encoding.UTF8, "application/json");
                }
                catch(Exception ex)
                {
                    return new StringContent("");
                }
            }
        }
    }
}