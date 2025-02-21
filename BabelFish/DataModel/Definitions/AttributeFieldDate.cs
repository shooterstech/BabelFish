
namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldDate : AttributeField<DateTime> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldDate() {
            MultipleValues = false;
            ValueType = ValueType.DATE;
            Validation = new AttributeValidationDate();
        }

        /// <summary>
        /// The default value for this field. It is the value assigned to the field if the user does not enter one.
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime ? DefaultValue { get; set; } = null;

        public AttributeValidationDate Validation { get; set; } = null;

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
                return DateTime.Today;

            return (DateTime) DefaultValue;
        }

        public override bool ValidateFieldValue( DateTime value ) {
            if (Validation == null)
                return true;

            return Validation.ValidateFieldValue( value ); 
        }
    }

    public class AttributeValidationDate : AttributeValidation<DateTime> {

        public AttributeValidationDate() {
        }

        /// <summary>
        /// The minimum value
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        [G_NS.JsonProperty( Order = 3 )]
        public DateTime ? MinValue { get; set; } = null;

        /// <summary>
        /// The maximum value
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        [G_NS.JsonProperty( Order = 4 )]
        public DateTime ? MaxValue { get; set; } = null;

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
