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

            foreach (AttributeValue attrVal in AttributeToUpdate.Attributes)
            {
                AttributesList.Add(attrVal.SetName, new Dictionary<string, string>());
                AttributesList[attrVal.SetName].Add("attribute-def", attrVal.SetName);
                AttributesList[attrVal.SetName].Add("action", attrVal.Action.ToString());
                AttributesList[attrVal.SetName].Add("visibility", attrVal.Visibility.ToString());
                Dictionary<string, dynamic> subcollection = new Dictionary<string, dynamic>();
                foreach (KeyValuePair<string, Dictionary<string, dynamic>> kvp in attrVal.attributeValues)
                {
                    // identify multi and non-multi attributes
                    if (kvp.Key != Helpers.AttributeValueKeyEnum.ATTRIBUTEVALUEKEY.ToString())
                        subcollection.Clear();

                    foreach (KeyValuePair<string, dynamic> kvp2 in kvp.Value)
                        subcollection.Add(kvp.Key, kvp.Value);

                    AttributeValues.Add(subcollection);
                }
            }
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

                serializedJSON.Append("{\"attribute-values\": {");
                string currentSetName = "";
                foreach (KeyValuePair<string, Dictionary<string, string>> attrList in AttributesList)
                {
                    if (currentSetName != attrList.Key)
                    {
                        currentSetName = attrList.Key;
                        //reset anything??
                    }
                    serializedJSON.Append($"\"{currentSetName}\": {{");
                    serializedJSON.Append($"\"attribute-def\" : \"{currentSetName}\",");
                    if (attrList.Value["action"] != "EMPTY")
                        serializedJSON.Append($"\"action\" : \"{attrList.Value["action"]}\",");
                    serializedJSON.Append($"\"visibility\" : \"{attrList.Value["visibility"]}\",");
                    serializedJSON.Append($"\"attribute-value\": ");
                    if (AttributeValues.Count > 1)
                        serializedJSON.Append("[");
                    foreach (var attrLoop in AttributeValues)
                    {
                        serializedJSON.Append("{");
                        foreach (KeyValuePair<string, dynamic> attrValues in attrLoop)
                        {
                            foreach (KeyValuePair<string, dynamic> attrValue in attrValues.Value)
                            {
                                serializedJSON.Append($"\"{attrValue.Key}\" : \"{attrValue.Value}\",");
                            }
                        }
                        serializedJSON.Append("},");
                    }
                    if (AttributeValues.Count > 1)
                        serializedJSON.Append("]");

                }
                serializedJSON.Append("}}}");

                return new StringContent(JsonConvert.SerializeObject(serializedJSON), Encoding.UTF8, "application/json");
            }
        }
    }
}