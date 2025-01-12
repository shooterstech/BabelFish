using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    public class AttributeValidationString : AttributeValidation, ICopy<AttributeValidationString> {


        public AttributeValidationString Copy() {

            AttributeValidationString copy = new AttributeValidationString();
            base.Copy(copy);

            copy.Regex = this.Regex;

            return copy;
        }


        /// <summary>
        /// Regular expression to check the value.
        /// </summary>
        public string Regex { get; set; } = string.Empty;
    }
}
