using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldBooleanList : AttributeField {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldBooleanList() {
            MultipleValues = true;
            ValueType = ValueType.BOOLEAN;
            Validation = new AttributeValidationBoolean();
        }

        /// <summary>
        /// The default value for this field, which is always a empty list.
        /// </summary>
        public List<bool> DefaultValue { get; private set; } = new List<bool>();

        private AttributeValidationBoolean validation = new AttributeValidationBoolean();

        /// <inheritdoc />
        public override AttributeValidation Validation {
            get { return validation; }
            set {
                if (value is AttributeValidationBoolean) {
                    validation = (AttributeValidationBoolean)value;
                } else {
                    throw new ArgumentException( $"Must set Validation to an object of type AttributeValidationBoolean, instead received {value.GetType()}" );
                }
            }
        }
    }
}
