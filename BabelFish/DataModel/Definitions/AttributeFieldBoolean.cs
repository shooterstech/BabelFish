using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldBoolean : AttributeField {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldBoolean() {
            MultipleValues = false;
            ValueType = ValueType.BOOLEAN;
            //Validation = new AttributeValidationBoolean();
        }

        /// <summary>
        /// The default value for this field. It is the value assigned to the field if the user does not enter one.
        /// </summary>
        public bool DefaultValue { get; set; } = false;

        private AttributeValidationBoolean validation = new AttributeValidationBoolean();

        /// <inheritdoc />
        /*
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
        */

        /// <inheritdoc />
        internal override dynamic DeserializeFromJsonElement( JsonElement value ) {
            if (value.ValueKind == JsonValueKind.True || value.ValueKind == JsonValueKind.False) {
                return value.GetBoolean();
            } else {
                Logger.Error( $"Got passed an unexpected JsonElement of type ${value.ValueKind}.");
                return DefaultValue;
            }
        }

        /// <inheritdoc />
        public override dynamic GetDefaultValue() {
            return DefaultValue;
        }
    }
}
