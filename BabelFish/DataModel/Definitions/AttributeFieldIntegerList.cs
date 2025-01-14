using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldIntegerList : AttributeField{

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldIntegerList() {
            MultipleValues = true;
            ValueType = ValueType.INTEGER;
            Validation = new AttributeValidationInteger();
        }

        /// <summary>
        /// The default value for this field, which is always a empty list.
        /// </summary>
        public List<int> DefaultValue { get; private set; } = new List<int>();

        private AttributeValidationInteger validation = new AttributeValidationInteger();

        /// <inheritdoc />
        public override AttributeValidation Validation {
            get { return validation; }
            set {
                if (value is AttributeValidationInteger) {
                    validation = (AttributeValidationInteger)value;
                } else {
                    throw new ArgumentException( $"Must set Validation to an object of type AttributeValidationInteger, instead received {value.GetType()}" );
                }
            }
        }
    }
}
