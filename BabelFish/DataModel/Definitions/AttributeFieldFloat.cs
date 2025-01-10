using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldFloat : AttributeField, ICopy<AttributeFieldFloat> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldFloat() {
            MultipleValues = false;
            ValueType = ValueType.FLOAT;
        }

        /// <inheritdoc />
        public AttributeFieldFloat Copy() {

            var copy = new AttributeFieldFloat();
            base.Copy( copy );

            copy.DefaultValue = this.DefaultValue;

            return copy;
        }

        /// <summary>
        /// The default value for this field. It is the value assigned to the field if the user does not enter one.
        /// </summary>
        public float DefaultValue { get; set; } = 0;
    }
}
