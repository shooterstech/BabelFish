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
    public class AttributeDefinitionFieldValidation
    {
        [JsonProperty(Order = 1)]
        public string Regex { get; set; } = string.Empty;

        [JsonProperty(Order = 2)]
        public string ErrorMessage { get; set; } = string.Empty;

        [JsonProperty(Order = 3)] 
        public List<string> ValidationErrorList { get; set; } = new List<string>();
    }
}
