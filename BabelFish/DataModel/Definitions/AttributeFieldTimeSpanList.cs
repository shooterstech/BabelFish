
namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldTimeSpanList : AttributeField<List<float>> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldTimeSpanList() {
            MultipleValues = true;
            ValueType = ValueType.TIME_SPAN;
        }

		[G_NS.JsonProperty( Order = 12 )]
		public AttributeValidationTimeSpan ? Validation = null;

        internal override dynamic DeserializeFromJsonElement( G_STJ.JsonElement value ) {
            if (value.ValueKind == G_STJ.JsonValueKind.Array) {
                return G_STJ.JsonSerializer.Deserialize<List<float>>( value );
            } else {
                Logger.Error( $"Got passed an unexpected JsonElement of type ${value.ValueKind}." );
                return GetDefaultValue();
            }
        }

        /// <inheritdoc />
        public override List<float> GetDefaultValue() {
            return new List<float>();
        }

        public override bool ValidateFieldValue( List<float> value ) {
            if (Validation == null)
                return true;

            foreach (var item in value)
                if (!Validation.ValidateFieldValue( item ))
                    return false;

            return true;
        }
    }
}
