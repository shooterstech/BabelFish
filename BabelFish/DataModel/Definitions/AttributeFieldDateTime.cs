
namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldDateTime : AttributeField<DateTime> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldDateTime() {
            MultipleValues = false;
            ValueType = ValueType.DATE_TIME;
        }

        /// <summary>
        /// The default value for this field. It is the value assigned to the field if the user does not enter one.
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateTimeConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateTimeConverter ) )]
		[G_NS.JsonProperty( Order = 11 )]
		public DateTime ? DefaultValue { get; set; } = null;


		[G_NS.JsonProperty( Order = 12 )]
		public AttributeValidationDateTime Validation = new AttributeValidationDateTime();

        internal override dynamic DeserializeFromJsonElement( G_STJ.JsonElement value ) {
            if (value.ValueKind == G_STJ.JsonValueKind.String) {
                //EKA NOTE Jan 2025: May need a JsonSerializerOptions specifying a custom DateTiem format
                return G_STJ.JsonSerializer.Deserialize<DateTime>( value );
            } else {
                Logger.Error( $"Got passed an unexpected JsonElement of type ${value.ValueKind}." );
                return GetDefaultValue();
            }
        }

        /// <inheritdoc />
        public override DateTime GetDefaultValue() {
            if (DefaultValue == null)
                return DateTime.UtcNow;

            return (DateTime) DefaultValue;
        }

        /// <inheritdoc />
        public override bool ValidateFieldValue( DateTime value ) {
            if (Validation == null)
                return true;

            return Validation.ValidateFieldValue( value );
        }
    }

    public class AttributeValidationDateTime : AttributeValidation<DateTime> {

        public AttributeValidationDateTime() {
        }

        /// <summary>
        /// The minimum value
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateTimeConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateTimeConverter ) )]
        [G_NS.JsonProperty( Order = 3 )]
        public DateTime? MinValue { get; set; } = null;

        /// <summary>
        /// The maximum value
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateTimeConverter ) )]
        [G_NS.JsonProperty( Order = 4 )]
        public DateTime? MaxValue { get; set; } = null;

        /// <inheritdoc />
        public override bool ValidateFieldValue( DateTime value ) {
            if (MinValue != null && value < MinValue)
                return false;

            if (MaxValue != null && value > MaxValue)
                return false;

            return true;
        }
    }
}
