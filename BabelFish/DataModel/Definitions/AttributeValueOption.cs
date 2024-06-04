using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.Definitions {
    [Serializable]
    public class AttributeValueOption {

        private List<string> errorList = new List<string>();

        public AttributeValueOption() {

        }

        /// <summary>
        /// Human readable display value.
        /// </summary>
        [JsonProperty( Order = 1 )]
        [DefaultValue( "" )]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The value that get's stored.
        /// </summary>
        [JsonProperty( Order = 2 )]
        public dynamic Value { get; set; }

        /// <summary>
        /// Human readable description
        /// </summary>
        [JsonProperty( Order = 3 )]
        [DefaultValue( "" )]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The Attribute Value's appellation (name) to use when looking up the mapping to an EventStyle or StageStyle.
        /// </summary>
        [JsonProperty( Order = 4 )]
        [DefaultValue( "" )]
        public string AttributeValueAppellation { get; set; } = string.Empty;
    }
}
