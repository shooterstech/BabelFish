using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldTimeSpan : AttributeField<float> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldTimeSpan() {
            MultipleValues = false;
            ValueType = ValueType.TIME_SPAN;
        }

        /// <summary>
        /// The default value for this field. It is the value assigned to the field if the user does not enter one.
        /// </summary>
        /// <remarks>Time span value represented in seconds.</remarks>
        public float ? DefaultValue { get; set; } = null;

        public AttributeValidationTimeSpan ? Validation = null;

        internal override dynamic DeserializeFromJsonElement( JsonElement value ) {
            if (value.ValueKind == JsonValueKind.Number) {
                return value.GetSingle();
            } else {
                Logger.Error( $"Got passed an unexpected JsonElement of type ${value.ValueKind}." );
                return DefaultValue;
            }
        }

        /// <inheritdoc />
        public override float GetDefaultValue() {
            if (DefaultValue == null)
                return 0;

            return (float) DefaultValue;
        }

        public override bool ValidateFieldValue( float value ) {
            if (Validation == null)
                return true;

            return Validation.ValidateFieldValue( value );
        }
    }

    public class AttributeValidationTimeSpan : AttributeValidation<float> {

        public AttributeValidationTimeSpan() {
        }

        /// <summary>
        /// The minimum value.
        /// </summary>
        /// <remarks>Value represents the number of seconds.</remarks>
        [DefaultValue( null )]
        public float ? MinValue { get; set; } = null;

        /// <summary>
        /// The maximum value.
        /// </summary>
        /// <remarks>Value represents the number of seconds.</remarks>
        [DefaultValue( null )]
        public float ? MaxValue { get; set; } = null;

        public override bool ValidateFieldValue( float value ) {
            if (MinValue != null && value < MinValue)
                return false;

            if (MaxValue != null && value > MaxValue)
                return false;

            return true;
        }
    }
}
