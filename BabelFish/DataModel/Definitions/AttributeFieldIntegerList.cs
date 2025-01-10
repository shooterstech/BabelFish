using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldIntegerList : AttributeField, ICopy<AttributeFieldIntegerList> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldIntegerList() {
            MultipleValues = true;
            ValueType = ValueType.INTEGER;
        }

        /// <inheritdoc />
        public AttributeFieldIntegerList Copy() {

            var copy = new AttributeFieldIntegerList();
            base.Copy( copy );

            return copy;
        }

        /// <summary>
        /// The default value for this field, which is always a empty list.
        /// </summary>
        public List<int> DefaultValue { get; private set; } = new List<int>();
    }
}
