using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldInteger : AttributeField, ICopy<AttributeFieldInteger> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldInteger() {
            MultipleValues = false;
            ValueType = ValueType.INTEGER;
        }

        /// <inheritdoc />
        public AttributeFieldInteger Copy() {

            var copy = new AttributeFieldInteger();
            base.Copy( copy );

            copy.DefaultValue = this.DefaultValue;

            return copy;
        }

        /// <summary>
        /// The default value for this field. It is the value assigned to the field if the user does not enter one.
        /// </summary>
        public int DefaultValue { get; set; } = 0;
    }
}
