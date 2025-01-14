using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldTimeSpan : AttributeField {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldTimeSpan() {
            MultipleValues = false;
            ValueType = ValueType.TIME_SPAN;
            Validation = new AttributeValidationTimeSpan();
        }

        /// <summary>
        /// The default value for this field. It is the value assigned to the field if the user does not enter one.
        /// </summary>
        /// <remarks>Time span value represented in seconds.</remarks>
        public float DefaultValue { get; set; } = 0;

        private AttributeValidationTimeSpan validation = new AttributeValidationTimeSpan();

        /// <inheritdoc />
        public override AttributeValidation Validation {
            get { return validation; }
            set {
                if (value is AttributeValidationTimeSpan) {
                    validation = (AttributeValidationTimeSpan)value;
                } else {
                    throw new ArgumentException( $"Must set Validation to an object of type AttributeValidationTimeSpan, instead received {value.GetType()}" );
                }
            }
        }

        internal override dynamic DeserializeFromJsonElement( JsonElement value ) {
            if (value.ValueKind == JsonValueKind.Number) {
                return value.GetSingle();
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
