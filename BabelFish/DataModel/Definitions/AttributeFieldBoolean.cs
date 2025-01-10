using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldBoolean : AttributeField, ICopy<AttributeFieldBoolean> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldBoolean() {
            MultipleValues = false;
            ValueType = ValueType.BOOLEAN;
        }

        /// <inheritdoc />
        public AttributeFieldBoolean Copy() {

            var copy = new AttributeFieldBoolean();
            base.Copy( copy );

            copy.DefaultValue = this.DefaultValue;

            return copy;
        }

        /// <summary>
        /// The default value for this field. It is the value assigned to the field if the user does not enter one.
        /// </summary>
        public bool DefaultValue { get; set; } = false;
    }
}
