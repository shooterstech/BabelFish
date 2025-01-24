using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldInteger : AttributeField {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldInteger() {
            MultipleValues = false;
            ValueType = ValueType.INTEGER;
            //Validation = new AttributeValidationInteger();
        }

        /// <summary>
        /// The default value for this field. It is the value assigned to the field if the user does not enter one.
        /// </summary>
        public int DefaultValue { get; set; } = 0;

        private AttributeValidationInteger validation = new AttributeValidationInteger();

        /// <inheritdoc />
        /*
        public override AttributeValidation Validation {
            get { return validation; }
            set {
                if (value is AttributeValidationInteger) {
                    validation = (AttributeValidationInteger)value;
                } else {
                    throw new ArgumentException( $"Must set Validation to an object of type AttributeValidationInteger, instead received {value.GetType()}" );
                }
            }
        }
        */

        internal override dynamic DeserializeFromJsonElement( JsonElement value ) {
            if (value.ValueKind == JsonValueKind.Number) {
                return value.GetInt32();
            } else {
                Logger.Error( $"Got passed an unexpected JsonElement of type ${value.ValueKind}." );
                return DefaultValue;
            }
        }

        /// <inheritdoc />
        public override dynamic GetDefaultValue() {
            return DefaultValue;
        }
    }
}
