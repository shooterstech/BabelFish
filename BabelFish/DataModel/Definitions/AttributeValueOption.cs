using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
        [JsonProperty( Order = 1 )]
        [DefaultValue( "" )]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The value that get's stored.
        /// </summary>
        [JsonProperty( Order = 2 )]
        public dynamic Value { get; set; }

        /// <summary>
        /// Human readable description
        /// </summary>
        [JsonProperty( Order = 3 )]
        [DefaultValue( "" )]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The Attribute Value's appellation (name) to use when looking up the mapping to an EventStyle or StageStyle.
        /// </summary>
        [JsonProperty( Order = 4 )]
        [DefaultValue( "" )]
        public string AttributeValueAppellation { get; set; } = string.Empty;


        /// <inheritdoc/>
        [JsonProperty( Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;
    }
}
