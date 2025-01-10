using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldDateList : AttributeField, ICopy<AttributeFieldDateList> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldDateList() {
            MultipleValues = true;
            ValueType = ValueType.DATE;
        }

        /// <inheritdoc />
        public AttributeFieldDateList Copy() {

            var copy = new AttributeFieldDateList();
            base.Copy( copy );

            copy.DefaultValue = this.DefaultValue;

            return copy;
        }

        /// <summary>
        /// The default value for this field, which is always an empty list.
        /// </summary>
        public List<DateTime> DefaultValue { get; private set; } = new List<DateTime>();
    }
}
