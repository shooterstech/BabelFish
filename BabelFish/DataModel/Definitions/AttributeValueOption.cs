using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShootersTech.BabelFish.DataModel.Definitions {
    [Serializable]
    public class AttributeValueOption {

        private List<string> errorList = new List<string>();

        public AttributeValueOption() {

        }

        public string Name { get; set; } = string.Empty;

        public dynamic Value { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
