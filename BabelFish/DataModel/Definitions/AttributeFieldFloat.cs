using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldFloat : AttributeField<float> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldFloat() {
            MultipleValues = false;
            ValueType = ValueType.FLOAT;
        }

		/// <summary>
		/// The default value for this field. It is the value assigned to the field if the user does not enter one.
		/// </summary>
		[G_NS.JsonProperty( Order = 11 )]
		public float DefaultValue { get; set; } = 0;

		[G_NS.JsonProperty( Order = 12 )]
		public AttributeValidationFloat Validation = null;

        internal override dynamic DeserializeFromJsonElement( G_STJ.JsonElement value ) {
            if (value.ValueKind == G_STJ.JsonValueKind.Number) {
                return value.GetSingle();
            } else {
                Logger.Error( $"Got passed an unexpected JsonElement of type ${value.ValueKind}." );
                return GetDefaultValue();
            }
        }

        /// <inheritdoc />
        public override float GetDefaultValue() {
            return DefaultValue;
        }

        /// <inheritdoc />
        public override bool ValidateFieldValue( float value ) {
            if (Validation == null)
                return true;

            return Validation.ValidateFieldValue( value );
        }
    }

    public class AttributeValidationFloat : AttributeValidation<float> {

        public AttributeValidationFloat() {
        }

        /// <summary>
        /// The minimum value
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        [DefaultValue( null  )]
        public float ? MinValue { get; set; } = null;

        /// <summary>
        /// The maximum value
        /// </summary>
        [G_NS.JsonProperty( Order = 4 )]
        [DefaultValue( null )]
        public float ? MaxValue { get; set; } = null;

        /// <inheritdoc />
        public override bool ValidateFieldValue( float value ) {
            if (MinValue != null && value < MinValue)
                return false;

            if (MaxValue != null && value > MaxValue) 
                return false;

            return true;
        }
    }
}
