using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class AttributeValue {

        public AttributeValue() { }

        /// <summary>
        /// Deprecated user AttributeDef
        /// </summary>
        public string Name { get; set; } = string.Empty;

        public string AttributeDef { get; set; } = string.Empty;

        /// <summary>
        /// Deprecated use Value
        /// </summary>
        [Obsolete("Deprecated, use Value")]
        public List<string> Values { get; set; } = new List<string>();

        //Calling Value a Dictionary is not entirely correct. On the Attribute Definition
        //if MultipleValues is true, then Value will be a list of Dictionaries. However,
        //the current implementation of Orion is such that it can only support one item and not a list.
        public Dictionary<string, dynamic> Value { get; set; } = new Dictionary<string, dynamic>();
    }
}
