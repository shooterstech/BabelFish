using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldTimeList : AttributeField, ICopy<AttributeFieldTimeList> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldTimeList() {
            MultipleValues = true;
            ValueType = ValueType.TIME;
        }

        /// <inheritdoc />
        public AttributeFieldTimeList Copy() {

            var copy = new AttributeFieldTimeList();
            base.Copy( copy );

            return copy;
        }

        /// <summary>
        /// The default value for this field, which is always a empty list.
        /// </summary>
        [JsonConverter( typeof( ScoposTimeOnlyConverter ) )]
        public List<DateTime> DefaultValue { get; private set; } = new List<DateTime>();
    }
}
