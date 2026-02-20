using System.ComponentModel;


namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldString : AttributeField<string> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldString() {
            MultipleValues = false;
            ValueType = ValueType.STRING;
        }

        /// <summary>
        /// The default value for this field. It is the value assigned to the field if the user does not enter one.
        /// </summary>
        [G_NS.JsonProperty( Order = 12 )]
        public string? DefaultValue { get; set; } = null;


        /// <summary>
        /// Indicates if the value of the attribute must be chosen from a list, 
        /// may be any value, of the there is a suggested list of values.
        /// </summary>
        [G_NS.JsonProperty( Order = 11, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( FieldType.OPEN )]
        public FieldType FieldType { get; set; } = FieldType.OPEN;

        /// <summary>
        /// List of possible values, when FieldType is CLOSED or SUGGEST
        /// </summary>
        [G_NS.JsonProperty( Order = 13 )]
        public List<AttributeValueOption<string>> Values { get; set; } = new List<AttributeValueOption<string>>();

        [G_NS.JsonProperty( Order = 14 )]
        public AttributeValidationString? Validation = null;

        internal override dynamic DeserializeFromJsonElement( G_STJ.JsonElement value ) {
            if (value.ValueKind == G_STJ.JsonValueKind.String) {
                return value.GetString();
            } else {
                Logger.Error( $"Got passed an unexpected JsonElement of type ${value.ValueKind}." );
                return GetDefaultValue();
            }
        }

        /// <inheritdoc />
        public override string GetDefaultValue() {
            if (DefaultValue == null)
                return string.Empty;

            return DefaultValue;
        }

        /// <inheritdoc />
        public override bool ValidateFieldValue( string value ) {
            if (this.FieldType == FieldType.CLOSED) {
                bool isOneOfTheOptionValues = false;
                foreach (var option in Values) {
                    if (option.Value == value) {
                        isOneOfTheOptionValues = true;
                        break;
                    }
                }

                if (!isOneOfTheOptionValues)
                    return false;

                return true;

            } else {

                if (Validation == null)
                    return true;

                return Validation.ValidateFieldValue( value );
            }
        }
    }

    public class AttributeValidationString : AttributeValidation<string> {

        public AttributeValidationString() {
        }

        /// <summary>
        /// Regular expression to check the value.
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        [DefaultValue( "" )]
        public string Regex { get; set; } = string.Empty;

        public override bool ValidateFieldValue( string value ) {
            return System.Text.RegularExpressions.Regex.IsMatch( value, this.Regex );
        }
    }
}
