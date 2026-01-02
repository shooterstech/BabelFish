using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// AttributeValidation are rules that are applied to entered data in an ATTRIBUTE VALUE to determine if the value is acceptable.
    /// </summary>
    public abstract class AttributeValidation<T> : IReconfigurableRulebookObject {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AttributeValidation() {
        }

        /// <summary>
        /// Message to be displayed to user when validation fails.
        /// </summary>
        [G_NS.JsonProperty( Order = 10 )]
        public string ErrorMessage { get; set; } = string.Empty;


        /// <inheritdoc/>
        [G_NS.JsonProperty( Order = 100 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        public abstract bool ValidateFieldValue( T value );

    }
}
