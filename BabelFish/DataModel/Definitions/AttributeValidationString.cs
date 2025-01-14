using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    public class AttributeValidationString : AttributeValidation {

        public AttributeValidationString() {
            this.ValueType = ValueType.STRING;
        }

        /// <summary>
        /// Regular expression to check the value.
        /// </summary>
        [DefaultValue( "" )]
        public string Regex { get; set; } = string.Empty;
    }
}
