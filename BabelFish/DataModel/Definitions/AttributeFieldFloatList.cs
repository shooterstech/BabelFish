
namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldFloatList : AttributeField<List<float>> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldFloatList() {
            MultipleValues = true;
            ValueType = ValueType.FLOAT;
        }

        public AttributeValidationFloat Validation = null;

        internal override dynamic DeserializeFromJsonElement( G_STJ.JsonElement value ) {
            if (value.ValueKind == G_STJ.JsonValueKind.Array) {
                //EKA NOTE Jan 2025: May need a JsonSerializerOptions specifying a custom DateTiem format
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

        /// <inheritdoc />
        public override bool ValidateFieldValue( List<float> value ) {
            if (Validation == null)
                return true;

            foreach( var item in value )
                if (!Validation.ValidateFieldValue( item ) )
                    return false;

            return true;
        }
    }
}
