using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    public class AttributeValidationFloat : AttributeValidation {

        public AttributeValidationFloat() {
            this.ValueType = ValueType.FLOAT;
        }

        /// <summary>
        /// The minimum value
        /// </summary>
        [DefaultValue( 0 )]
        public int MinValue { get; set; } = 0;

        /// <summary>
        /// The maximum value
        /// </summary>
        [DefaultValue( float.MaxValue )]
        public int MaxValue { get; set; } = int.MaxValue;
    }
}
