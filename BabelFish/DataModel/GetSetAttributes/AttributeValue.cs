using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace BabelFish.DataModel.GetSetAttributes {

    [Serializable]
    public class AttributeValue
    {
        public AttributeValue() { }

        /// <summary>
        /// httpStatus (leave this as string in case we get an unexpected status not in an enum?)
        /// </summary>
        [JsonProperty(Order = 1)]
        public string statusCode { get; set; } = string.Empty;

        [JsonProperty(Order = 2, PropertyName = "attribute-def")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty(Order = 3, PropertyName = "attribute-value")]
        public List<Dictionary<string, dynamic>> attributeKVP = new List<Dictionary<string, dynamic>>();

        [JsonConverter(typeof(StringEnumConverter))]
        public Helpers.VisibilityOption Visibility { get; set; }

        [JsonProperty(Order = 4, PropertyName = "attribute-definition")]
        public AttributeDefinition attributeDefinition = new AttributeDefinition();

        public override string ToString()
        {
            StringBuilder foo = new StringBuilder();
            foo.Append("Attributes for ");
            foo.Append(Name);
            return foo.ToString();
        }

    }
}