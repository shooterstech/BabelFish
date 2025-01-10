using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldTime : AttributeField, ICopy<AttributeFieldTime> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldTime() {
            MultipleValues = false;
            ValueType = ValueType.TIME;
        }

        /// <inheritdoc />
        public AttributeFieldTime Copy() {

            var copy = new AttributeFieldTime();
            base.Copy( copy );

            copy.DefaultValue = this.DefaultValue;

            return copy;
        }

        /// <summary>
        /// The default value for this field. It is the value assigned to the field if the user does not enter one.
        /// </summary>
        [JsonConverter( typeof( ScoposTimeOnlyConverter ) )]
        public DateTime DefaultValue { get; set; } = DateTime.Now;
    }
}
