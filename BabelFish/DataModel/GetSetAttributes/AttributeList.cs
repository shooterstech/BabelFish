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
using Newtonsoft.Json.Linq;

namespace BabelFish.DataModel.GetSetAttributes {

    [Serializable]
    public class AttributeList
    {
        public List<AttributeValue> Attributes { get; set; } = new List<AttributeValue>();

        public override string ToString()
        {
            StringBuilder foo = new StringBuilder();
            foo.Append("Attribute List");
            return foo.ToString();
        }

    }
}
