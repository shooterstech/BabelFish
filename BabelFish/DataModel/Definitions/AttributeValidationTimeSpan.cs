using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    public class AttributeValidationTimeSpan : AttributeValidation {

        public AttributeValidationTimeSpan() {
            this.ValueType = ValueType.TIME_SPAN;
        }

        /// <summary>
        /// The minimum value.
        /// </summary>
        /// <remarks>Value represents the number of seconds.</remarks>
        [DefaultValue( 0 )]
        public float MinValue { get; set; } = 0;

        /// <summary>
        /// The maximum value.
        /// </summary>
        /// <remarks>Value represents the number of seconds.</remarks>
        [DefaultValue( 360 )]
        public float MaxValue { get; set; } = 360;
    }
}
