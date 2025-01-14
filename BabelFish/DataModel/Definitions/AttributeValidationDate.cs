using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    public class AttributeValidationDate : AttributeValidation {

        public AttributeValidationDate() {
            this.ValueType = ValueType.DATE;
        }

        /// <summary>
        /// The minimum value
        /// </summary>
        [DefaultValue( int.MinValue )]
        public DateTime MinValue { get; set; } = DateTime.MinValue;

        /// <summary>
        /// The maximum value
        /// </summary>
        [DefaultValue( int.MaxValue )]
        public DateTime MaxValue { get; set; } = DateTime.MaxValue;
    }
}
