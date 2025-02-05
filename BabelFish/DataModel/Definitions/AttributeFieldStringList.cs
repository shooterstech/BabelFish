using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;


namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldStringList : AttributeField {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldStringList() {
            MultipleValues = true;
            ValueType = ValueType.STRING;
            //Validation = new AttributeValidationString();
        }

        /// <summary>
        /// The default value for this field, which is always a empty list.
        /// </summary>
        public List<string> DefaultValue { get; private set; } = new List<string>();


        /// <summary>
        /// Indicates if the value of the attribute must be chosen from a list, 
        /// may be any value, of the there is a suggested list of values.
        /// </summary>
        [DefaultValue( FieldType.OPEN )]
        public FieldType FieldType { get; protected set; } = FieldType.OPEN;

        /// <summary>
        /// List of possible values, when FieldType is CLOSED or SUGGEST
        /// </summary>
        public List<AttributeValueOption<string>> Values { get; set; } = new List<AttributeValueOption<string>>();

        private AttributeValidationString validation = new AttributeValidationString();

        /// <inheritdoc />
        /*
        public override AttributeValidation Validation {
            get { return validation; }
            set {
                if (value is AttributeValidationString) {
                    validation = (AttributeValidationString)value;
                } else {
                    throw new ArgumentException( $"Must set Validation to an object of type AttributeValidationString, instead received {value.GetType()}" );
                }
            }
        }
        */

        internal override dynamic DeserializeFromJsonElement( JsonElement value ) {
            List<string> list = new List<string>();

            if (value.ValueKind == JsonValueKind.Array) {
                foreach( var v in value.EnumerateArray()) {
                    if (v.ValueKind == JsonValueKind.String) {
                        list.Add( v.GetString() );
                    } else {
                        Logger.Error( $"Got passed an unexpected JsonElement of type ${value.ValueKind}." );
                        //Ignore adding anything to the list
                    }
                }
                return list;
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
            return value is List<string>;
        }
    }
}
