using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldBooleanList : AttributeField, ICopy<AttributeFieldBooleanList> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldBooleanList() {
            MultipleValues = true;
            ValueType = ValueType.BOOLEAN;
        }

        /// <inheritdoc />
        public AttributeFieldBooleanList Copy() {

            var copy = new AttributeFieldBooleanList();
            base.Copy( copy );

            copy.DefaultValue = this.DefaultValue;

            return copy;
        }

        /// <summary>
        /// The default value for this field, which is always a empty list.
        /// </summary>
        public List<bool> DefaultValue { get; private set; } = new List<bool>();
    }
}
