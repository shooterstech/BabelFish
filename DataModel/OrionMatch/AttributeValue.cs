using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.Match {
    [Serializable]
    public class AttributeValue {

        public AttributeValue() {
            Name = "";
            Values = new List<string>();
        }

        /// <summary>
        /// Deprecated user AttributeDef
        /// </summary>
        public string Name { get; set; }

        public string AttributeDef { get; set; }

        /// <summary>
        /// Deprecated use Value
        /// </summary>
        public List<string> Values { get; set; }
        
        //Calling Value a Dictionary is not entirely correct. On the Attribute Definition
        //if MultipleValues is true, then Value will be a list of Dictionaries. However,
        //the current implementation of Orion is such that it can only support one item and not a list.
        public Dictionary<string, dynamic> Value { get; set; }
    }
}
