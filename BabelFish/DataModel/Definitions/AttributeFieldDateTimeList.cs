
namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldDateTimeList : AttributeField<List<DateTime>> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldDateTimeList() {
            MultipleValues = true;
            ValueType = ValueType.DATE_TIME;
        }

        public AttributeValidationDateTime Validation = new AttributeValidationDateTime();

        internal override dynamic DeserializeFromJsonElement( G_STJ.JsonElement value ) {
            if (value.ValueKind == G_STJ.JsonValueKind.Array) {
                //EKA NOTE Jan 2025: May need a JsonSerializerOptions specifying a custom DateTiem format
                return G_STJ.JsonSerializer.Deserialize<List<DateTime>>( value );
            } else {
                Logger.Error( $"Got passed an unexpected JsonElement of type ${value.ValueKind}." );
                return GetDefaultValue();
            }
        }

        /// <inheritdoc />
        public override List<DateTime> GetDefaultValue() {
            return new List<DateTime>();
        }

        /// <inheritdoc />
        public override bool ValidateFieldValue( List<DateTime> value ) {
            if (Validation == null)
                return true;

            foreach (var item in value)
                if (!Validation.ValidateFieldValue( item ))
                    return false;

            return true;
        }
    }
}
