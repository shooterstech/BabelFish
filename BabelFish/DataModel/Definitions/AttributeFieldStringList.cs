

using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldStringList : AttributeField<List<string>> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldStringList() {
            MultipleValues = true;
            ValueType = ValueType.STRING;
        }


        /// <summary>
        /// Indicates if the value of the attribute must be chosen from a list, 
        /// may be any value, of the there is a suggested list of values.
        /// </summary>
        [DefaultValue( FieldType.OPEN )]
		[G_NS.JsonProperty( Order = 11 )]
		public FieldType FieldType { get; set; } = FieldType.OPEN;

		/// <summary>
		/// List of possible values, when FieldType is CLOSED or SUGGEST
		/// </summary>
		[G_NS.JsonProperty( Order = 12 )]
		public List<AttributeValueOption<string>> Values { get; set; } = new List<AttributeValueOption<string>>();

		[G_NS.JsonProperty( Order = 13 )]
		public AttributeValidationString ? Validation = null;

        internal override dynamic DeserializeFromJsonElement( G_STJ.JsonElement value ) {
            List<string> list = new List<string>();

            if (value.ValueKind == G_STJ.JsonValueKind.Array) {
                foreach (var v in value.EnumerateArray()) {
                    if (v.ValueKind == G_STJ.JsonValueKind.String) {
                        list.Add( v.GetString() );
                    } else {
                        Logger.Error( $"Got passed an unexpected JsonElement of type ${value.ValueKind}." );
                        //Ignore adding anything to the list
                    }
                }
                return list;
            } else {
                Logger.Error( $"Got passed an unexpected JsonElement of type ${value.ValueKind}." );
                return GetDefaultValue();
            }
        }

        /// <inheritdoc />
        public override List<string> GetDefaultValue() {
            return new List<string>();
        }

        /// <inheritdoc />
        public override bool ValidateFieldValue( List<string> value ) {

            if (this.FieldType == FieldType.CLOSED) {
                foreach (var item in value) {
                    bool isOneOfTheOptionValues = false;
                    foreach (var option in Values) {
                        if (option.Value == item) {
                            isOneOfTheOptionValues = true;
                            break;
                        }
                    }

                    if (!isOneOfTheOptionValues)
                        return false;
                }

                return true;

            } else {
                if (Validation == null)
                    return true;

                foreach (var item in value)
                    if (!Validation.ValidateFieldValue( item ))
                        return false;

                return true;
            }
        }
    }
}
