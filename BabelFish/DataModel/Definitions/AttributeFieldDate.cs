using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters.Microsoft;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldDate : AttributeField {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldDate() {
            MultipleValues = false;
            ValueType = ValueType.DATE;
            //Validation = new AttributeValidationDate();
        }

        /// <summary>
        /// The default value for this field. It is the value assigned to the field if the user does not enter one.
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime DefaultValue { get; set; } = DateTime.Today;

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
            if (value.ValueKind == JsonValueKind.String) {
                //EKA NOTE Jan 2025: May need a JsonSerializerOptions specifying a custom DateTiem format
                return JsonSerializer.Deserialize<DateTime>( value );
            } else {
                Logger.Error( $"Got passed an unexpected JsonElement of type ${value.ValueKind}." );
                return DefaultValue;
            }
        }

        /// <inheritdoc />
        public override dynamic GetDefaultValue() {
            return DefaultValue;
        }

        /// <inheritdoc />
        public override bool ValidateFieldValue( dynamic value ) {
            return value is DateTime;
        }
    }
}
