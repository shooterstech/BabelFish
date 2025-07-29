
namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldIntegerList : AttributeField<List<int>> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldIntegerList() {
            MultipleValues = true;
            ValueType = ValueType.INTEGER;
        }

		[G_NS.JsonProperty( Order = 12 )]
		public AttributeValidationInteger ? Validation = null;

        internal override dynamic DeserializeFromJsonElement( G_STJ.JsonElement value ) {
            if (value.ValueKind == G_STJ.JsonValueKind.Array) {
                //EKA NOTE Jan 2025: May need a JsonSerializerOptions specifying a custom DateTiem format
                return G_STJ.JsonSerializer.Deserialize<List<int>>( value );
            } else {
                Logger.Error( $"Got passed an unexpected JsonElement of type ${value.ValueKind}." );
                return GetDefaultValue();
            }
        }

        /// <inheritdoc />
        public override List<int> GetDefaultValue() {
            return new List<int>();
        }

        /// <inheritdoc />
        public override bool ValidateFieldValue( List<int> value ) {
            if (Validation == null)
                return true;

            foreach (var item in value)
                if (! Validation.ValidateFieldValue( item ))
                    return false;

            return true;
        }
    }
}
