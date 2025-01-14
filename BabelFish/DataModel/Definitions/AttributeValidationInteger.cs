using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    public class AttributeValidationInteger : AttributeValidation {

        public AttributeValidationInteger() {
            this.ValueType = ValueType.INTEGER;
        }

        /// <summary>
        /// The minimum value
        /// </summary>
        [DefaultValue( 0 )]
        public int MinValue { get; set; } = 0;

        /// <summary>
        /// The maximum value
        /// </summary>
        [DefaultValue( int.MaxValue )]
        public int MaxValue { get; set; } = int.MaxValue;
    }
}
