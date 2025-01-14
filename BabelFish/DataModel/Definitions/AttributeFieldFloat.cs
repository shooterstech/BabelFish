using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldFloat : AttributeField {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldFloat() {
            MultipleValues = false;
            ValueType = ValueType.FLOAT;
            Validation = new AttributeValidationFloat();
        }

        /// <summary>
        /// The default value for this field. It is the value assigned to the field if the user does not enter one.
        /// </summary>
        public float DefaultValue { get; set; } = 0;

        private AttributeValidationFloat validation = new AttributeValidationFloat();

        /// <inheritdoc />
        public override AttributeValidation Validation {
            get { return validation; }
            set {
                if (value is AttributeValidationFloat) {
                    validation = (AttributeValidationFloat)value;
                } else {
                    throw new ArgumentException( $"Must set Validation to an object of type AttributeValidationFloat, instead received {value.GetType()}" );
                }
            }
        }
    }
}
