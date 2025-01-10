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
    [Serializable]
    public class AttributeValueOption: ICopy<AttributeValueOption>, IReconfigurableRulebookObject {

        /// <summary>
        /// Public constructor
        /// </summary>
        public AttributeValueOption() {

        }

        /// <inheritdoc/>
        public AttributeValueOption Copy() {
            AttributeValueOption copy = new AttributeValueOption();
            copy.Name = Name;
            copy.Value = Value; //Value is a dynamic field, so not sure a straight copy is correct.
            copy.Description = Description;
            copy.AttributeValueAppellation = AttributeValueAppellation;
            copy.Comment = Comment;

            return copy;
        }

        /// <summary>
        /// Human readable display value.
        /// </summary>
        [DefaultValue( "" )]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The value that get's stored.
        /// </summary>
        public dynamic Value { get; set; }

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
