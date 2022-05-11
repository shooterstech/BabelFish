using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace BabelFish.DataModel.GetSetAttributeValue {

    [Serializable]
    public class SetAttributeValue
    {
        public SetAttributeValue() { }

        /// <summary>
        /// UserID
        /// </summary>
        [JsonProperty(Order = 1)]
        public string AttributeValue { get; set; } = string.Empty;

        [JsonProperty(Order = 2)]
        public string StatusCode { get; set; } = string.Empty;

        [JsonProperty(Order = 3)]
        public string Message { get; set; } = string.Empty;

        public override string ToString()
        {
            StringBuilder foo = new StringBuilder();
            foo.Append("Set Attribute Value Response status for ");
            foo.Append(AttributeValue);
            return foo.ToString();
        }

    }
}