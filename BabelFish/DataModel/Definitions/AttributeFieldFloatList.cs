using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldFloatList : AttributeField, ICopy<AttributeFieldFloatList> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldFloatList() {
            MultipleValues = true;
            ValueType = ValueType.FLOAT;
        }

        /// <inheritdoc />
        public AttributeFieldFloatList Copy() {

            var copy = new AttributeFieldFloatList();
            base.Copy( copy );

            return copy;
        }

        /// <summary>
        /// The default value for this field, which is always a empty list.
        /// </summary>
        public List<float> DefaultValue { get; private set; } = new List<float>();
    }
}
