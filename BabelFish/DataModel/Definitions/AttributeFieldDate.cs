using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldDate : AttributeField {

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
        [JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        public DateTime DefaultValue { get; set; } = DateTime.Today;

        private AttributeValidationDate validation = new AttributeValidationDate();

        /// <inheritdoc />
        public override AttributeValidation Validation {
            get { return validation; }
            set {
                if (value is AttributeValidationDate) {
                    validation = (AttributeValidationDate)value;
                } else {
                    throw new ArgumentException( $"Must set Validation to an object of type AttributeValidationDate, instead received {value.GetType()}" );
                }
            }
        }
    }
}
