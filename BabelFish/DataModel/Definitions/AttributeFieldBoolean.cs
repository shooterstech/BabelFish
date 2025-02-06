using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldBoolean : AttributeField<bool> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldBoolean() {
            MultipleValues = false;
            ValueType = ValueType.BOOLEAN;
        }

        /// <summary>
        /// The default value for this field. It is the value assigned to the field if the user does not enter one.
        /// </summary>
        public bool DefaultValue { get; set; } = false;

        /// <inheritdoc />
        internal override dynamic DeserializeFromJsonElement( JsonElement value ) {
            if (value.ValueKind == JsonValueKind.True || value.ValueKind == JsonValueKind.False) {
                return value.GetBoolean();
            } else {
                Logger.Error( $"Got passed an unexpected JsonElement of type ${value.ValueKind}." );
                return DefaultValue;
            }
        }

        /*
         * Nothing to validate with ValueType Boolean. The value is always valid.
         */

        /// <inheritdoc />
        public override bool GetDefaultValue() {
            return DefaultValue;
        }

        public override bool ValidateFieldValue( bool value ) {
            return true;
        }
    }
}
