using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        /// <inheritdoc />
        public AttributeValidation Copy(AttributeValidation copy) {

            copy.MinValue = MinValue;
            copy.MaxValue = MaxValue;
            copy.Regex = Regex;
            copy.ErrorMessage = ErrorMessage;

            return copy;
        }

        /// <summary>
        /// Minimum accepted value.
        /// </summary>
        public dynamic MinValue { get; set; }

        /// <summary>
        /// Maximum accepted value.
        /// </summary>
        public dynamic MaxValue { get; set; }

        /// <summary>
        /// Regular expression to check the value.
        /// </summary>
        public string Regex { get; set; } = string.Empty;

        /// <summary>
        /// Message to be displayed to user when validation fails.
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;


        /// <inheritdoc/>
        [JsonProperty( Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

    }
}
