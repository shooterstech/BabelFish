
namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldDateList : AttributeField<List<DateTime>> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldDateList() {
            MultipleValues = true;
            ValueType = ValueType.DATE;
        }

        [G_NS.JsonProperty( Order = 12 )]
        public AttributeValidationDate? Validation { get; set; } = null;

        internal override dynamic DeserializeFromJsonElement( G_STJ.JsonElement value ) {
            if (value.ValueKind == G_STJ.JsonValueKind.Array) {
                //EKA NOTE Jan 2025: May need a JsonSerializerOptions specifying a custom DateTiem format
                return G_STJ.JsonSerializer.Deserialize<List<DateTime>>( value );
            } else {
                _logger.Error( $"Got passed an unexpected JsonElement of type ${value.ValueKind}." );
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

        /// <summary>
        /// When a List of DateTime value is being serialized to be sent to the API, we need to convert it into a string format that the API expects.
        /// This method handles that conversion, specifically to the "yyyy-MM-dd" format for dates.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override dynamic ValueForSerialization( dynamic value ) {
            // Convert the DateTime value into a string format that the API expects. Assuming the API expects dates in yyyy-MM-dd format, we can do the following:
            if (value is List<DateTime> dateTimeValues) {
                var serializedValues = new List<string>();
                foreach (var dateTimeValue in dateTimeValues) {
                    serializedValues.Add( dateTimeValue.ToString( DateTimeFormats.DATE_FORMAT ) );
                }
                return serializedValues;
            }

            // We shouldn't ever get here, b/c the value should always be a DateTime instance. But if we do, we can log an error and return the value as-is.
            _logger.Warn( $"Value for serialization is not a List<DateTime> instance. Value: {value}. Returning value as-is." );
            return value;
        }
    }
}
