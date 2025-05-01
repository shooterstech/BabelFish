using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldInteger : AttributeField<int> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldInteger() {
            MultipleValues = false;
            ValueType = ValueType.INTEGER;
        }

		/// <summary>
		/// The default value for this field. It is the value assigned to the field if the user does not enter one.
		/// </summary>
		[G_NS.JsonProperty( Order = 11 )]
		public int DefaultValue { get; set; } = 0;


		[G_NS.JsonProperty( Order = 12 )]
		public AttributeValidationInteger ? Validation = null;

        internal override dynamic DeserializeFromJsonElement( JsonElement value ) {
            if (value.ValueKind == JsonValueKind.Number) {
                return value.GetInt32();
            } else {
                Logger.Error( $"Got passed an unexpected JsonElement of type ${value.ValueKind}." );
                return DefaultValue;
            }
        }

        /// <inheritdoc />
        public override int GetDefaultValue() {
            return DefaultValue;
        }

        /// <inheritdoc />
        public override bool ValidateFieldValue( int value ) {
            if (Validation == null)
                return true;

            return Validation.ValidateFieldValue( value );
        }
    }

    public class AttributeValidationInteger : AttributeValidation<int> {

        public AttributeValidationInteger() {
        }

        /// <summary>
        /// The minimum value
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        [DefaultValue( null )]
        public int ? MinValue { get; set; } = null;

        /// <summary>
        /// The maximum value
        /// </summary>
        [G_NS.JsonProperty( Order = 4 )]
        [DefaultValue( null )]
        public int ? MaxValue { get; set; } = null;

        public override bool ValidateFieldValue( int value ) {
            if (MinValue != null && value < MinValue)
                return false;

            if (MaxValue != null && value > MaxValue)
                return false;

            return true;
        }
    }
}
