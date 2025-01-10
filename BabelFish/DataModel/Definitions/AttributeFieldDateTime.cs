using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeFieldDateTime : AttributeField, ICopy<AttributeFieldDateTime> {

        /// <summary>
        /// Public default constructor
        /// </summary>
        public AttributeFieldDateTime() {
            MultipleValues = false;
            ValueType = ValueType.DATE_TIME;
        }

        /// <inheritdoc />
        public AttributeFieldDateTime Copy() {

            var copy = new AttributeFieldDateTime();
            base.Copy( copy );

            copy.DefaultValue = this.DefaultValue;

            return copy;
        }

        /// <summary>
        /// The default value for this field. It is the value assigned to the field if the user does not enter one.
        /// </summary>
        [JsonConverter( typeof( ScoposDateTimeConverter ) )]
        public DateTime DefaultValue { get; set; } = DateTime.Now;
    }
}
