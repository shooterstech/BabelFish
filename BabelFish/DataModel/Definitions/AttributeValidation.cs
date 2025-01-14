using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// AttributeValidation are rules that are applied to entered data in an ATTRIBUTE VALUE to determine if the value is acceptable.
    /// </summary>
    public abstract class AttributeValidation : IReconfigurableRulebookObject {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AttributeValidation() {
        }

        /// <summary>
        /// The type of data that this field will hold.
        /// </summary>
        [JsonInclude]
        public ValueType ValueType { get; protected set; } = ValueType.STRING;

        /// <summary>
        /// Message to be displayed to user when validation fails.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;


        /// <inheritdoc/>
        [JsonIgnore( Condition = JsonIgnoreCondition.WhenWritingDefault )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

    }
}
