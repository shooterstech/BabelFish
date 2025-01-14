using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;


namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldString : AttributeField {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldString() {
            MultipleValues = false;
            ValueType = ValueType.STRING;
            Validation = new AttributeValidationString();
        }

        /// <summary>
        /// The default value for this field. It is the value assigned to the field if the user does not enter one.
        /// </summary>
        public string DefaultValue { get; set; } = string.Empty;


        /// <summary>
        /// Indicates if the value of the attribute must be chosen from a list, 
        /// may be any value, of the there is a suggested list of values.
        /// </summary>
        [DefaultValue( FieldType.OPEN )]
        public FieldType FieldType { get; protected set; } = FieldType.OPEN;

        /// <summary>
        /// List of possible values, when FieldType is CLOSED or SUGGEST
        /// </summary>
        public List<AttributeValueOption> Values { get; set; } = new List<AttributeValueOption>();

        private AttributeValidationString validation = new AttributeValidationString();

        /// <inheritdoc />
        public override AttributeValidation Validation {
            get { return validation; }
            set {
                if (value is AttributeValidationString) {
                    validation = (AttributeValidationString) value;
                } else {
                    throw new ArgumentException( $"Must set Validation to an object of type AttributeValidationString, instead received {value.GetType()}" );
                }
            }
        }

        internal override dynamic DeserializeFromJsonElement( JsonElement value ) {
            if (value.ValueKind == JsonValueKind.String) {
                return value.GetString();
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
