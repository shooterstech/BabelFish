using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
//using System.Text.Json; //COMMENT OUT FOR .NET Standard 2.0
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BabelFish.DataModel.GetSetAttributes {

    [Serializable]
    public class AttributeDefinition
    {

        [JsonProperty(Order = 1)]
        public bool Discontinued { get; set; } = false;

        [JsonProperty(Order = 2)]
        public bool MultipleValues { get; set; } = false;

        [JsonProperty(Order = 3)]
        [JsonConverter(typeof(StringEnumConverter))]
        public Helpers.VisibilityOption MaxVisibility { get; set; }

        [JsonProperty(Order = 4)]
        public List<AttributeDefinitionField> Fields { get; set; } = new List<AttributeDefinitionField>();

    }
}
