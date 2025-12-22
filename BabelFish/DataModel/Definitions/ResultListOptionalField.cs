using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// Within a RESULT LIST DEFINITION there are often places where we want the end user to be able 
    /// to dynamically set the format values. An exammple would be on a spanning field that gives the 
    /// user the choice to include demographic information. 
    /// </summary>
    public class ResultListOptionalField : IReconfigurableRulebookObject {

        /// <summary>
        /// The default value for this optional field.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        [DefaultValue( "" )]
        public string DefaultFieldText { get; set; } = string.Empty;

        /// <summary>
        /// A human readable description, of what this optional field sets (what field in the Result List Format does it effect). 
        /// </summary>
        [DefaultValue( "" )]
        [G_STJ_SER.JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        public string Description {  get; set; } = string.Empty;

        /// <inheritdoc />
		[G_STJ_SER.JsonPropertyOrder( 100 )]
        [G_NS.JsonProperty( Order = 100 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// Helper method to determine if this optional field instance is empty. 
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty() {
            return string.IsNullOrEmpty( DefaultFieldText ) && string.IsNullOrEmpty( Description );
        }
    }
}
