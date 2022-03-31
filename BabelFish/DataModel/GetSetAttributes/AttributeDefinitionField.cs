using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
//using System.Text.Json; //COMMENT OUT FOR .NET Standard 2.0
using System.Threading.Tasks;
using BabelFish.DataModel.Definitions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BabelFish.DataModel.GetSetAttributes {

    [Serializable]
    public class AttributeDefinitionField
    {
        [JsonProperty(Order = 1)]
        public AttributeDefinitionFieldValidation Validation { get; set; } = new AttributeDefinitionFieldValidation();

        [JsonProperty(Order = 2)]
        public string DefaultValue { get; set; } = string.Empty;

        [JsonProperty(Order = 3)]
        public bool Required { get; set; } = false;

        [JsonProperty(Order = 4)] 
        public string ValueType { get; set; } = string.Empty;

        [JsonProperty(Order = 5)]
        public string DisplayName { get; set; } = string.Empty;

        [JsonProperty(Order = 6)]
        //public Tuple<Dictionary<string, string>,Dictionary<string, string>> Values { get; set; }
        public List<Dictionary<string, string>> Values { get; set; } = new List<Dictionary<string, string>>();

        [JsonProperty(Order = 7)]
        public bool MultipleValues { get; set; } = false;

        [JsonProperty(Order = 8)]
        public string FieldName { get; set; } = string.Empty;

        [JsonProperty(Order = 9)]
        public string FieldType { get; set; } = string.Empty;
    }
}
