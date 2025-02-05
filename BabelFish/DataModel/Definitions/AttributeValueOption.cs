using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// AttributeValueOption are the choices presented to the user with the 
    /// AttributeField FieldType is SUGGEST or CLOSED.
    /// </summary>
    public class AttributeValueOption<T> : IReconfigurableRulebookObject {

        /// <summary>
        /// Public constructor
        /// </summary>
        public AttributeValueOption() {

        }

        /// <summary>
        /// Human readable display value.
        /// </summary>
        [DefaultValue( "" )]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The value that get's stored.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Human readable description
        /// </summary>
        [DefaultValue( "" )]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The Attribute Value's appellation (name) to use when looking up the mapping to an EventStyle or StageStyle.
        /// </summary>
        [DefaultValue( "" )]
        public string AttributeValueAppellation { get; set; } = string.Empty;


        /// <inheritdoc/>
        [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingDefault )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;
    }
}
