using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Definitions {
    public class AttributeValidation {

        private List<string> errorList = new List<string>();

        public AttributeValidation() {
        }

        public dynamic MinValue { get; set; }

        public dynamic MaxValue { get; set; }

        public string Regex { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

    }
}
