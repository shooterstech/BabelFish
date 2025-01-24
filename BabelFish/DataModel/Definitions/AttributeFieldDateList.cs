using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldDateList : AttributeField {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldDateList() {
            MultipleValues = true;
            ValueType = ValueType.DATE;
            //Validation = new AttributeValidationDate();
        }

        /// <summary>
        /// The default value for this field, which is always an empty list.
        /// </summary>
        public List<DateTime> DefaultValue { get; private set; } = new List<DateTime>();

        private AttributeValidationDate validation = new AttributeValidationDate();

        /// <inheritdoc />
        /*
        public override AttributeValidation Validation {
            get { return validation; }
            set {
                if (value is AttributeValidationDate) {
                    validation = (AttributeValidationDate)value;
                } else {
                    throw new ArgumentException( $"Must set Validation to an object of type AttributeValidationDate, instead received {value.GetType()}" );
                }
            }
        }
        */

        internal override dynamic DeserializeFromJsonElement( JsonElement value ) {
            if (value.ValueKind == JsonValueKind.Array) {
                //EKA NOTE Jan 2025: May need a JsonSerializerOptions specifying a custom DateTiem format
                return JsonSerializer.Deserialize<List<DateTime>>( value );
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
