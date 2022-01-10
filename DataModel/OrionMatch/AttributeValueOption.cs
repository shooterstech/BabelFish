using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BabelFish.DataModel.Match {
    [Serializable]
    public class AttributeValueOption {

        private List<string> errorList = new List<string>();

        public AttributeValueOption() {

        }

        public string Name { get; set; }

        public dynamic Value { get; set; }

        public string Description { get; set; }

    }
}
