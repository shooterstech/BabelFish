using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldDateTimeList : AttributeField, ICopy<AttributeFieldDateTimeList> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldDateTimeList() {
            MultipleValues = true;
            ValueType = ValueType.DATE_TIME;
        }

        /// <inheritdoc />
        public AttributeFieldDateTimeList Copy() {

            var copy = new AttributeFieldDateTimeList();
            base.Copy( copy );

            return copy;
        }

        /// <summary>
        /// The default value for this field, which is always a empty list.
        /// </summary>
        public List<DateTime> DefaultValue { get; private set; } = new List<DateTime>();
    }
}
