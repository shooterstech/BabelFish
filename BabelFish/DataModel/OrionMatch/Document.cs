using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Describes the name, location (url), and a description of a document. 
    /// Usually PDFs or similiar.
    /// </summary>
    public class Document {

        /// <summary>
        /// Unique key value for this document
        /// </summary>
        [DefaultValue( "" )] 
        public string DocumentName { get; set; } = string.Empty;

        /// <summary>
        /// The file name for this document.
        /// </summary>
        [DefaultValue( "" )] 
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// The downloadable URL to retreive this document.
        /// </summary>
        [DefaultValue( "" )] 
        public string URL { get; set; } = string.Empty;

        /// <summary>
        /// Human readable description of this document.
        /// </summary>
        [DefaultValue( "" )]
        public string Description { get; set; } = string.Empty;

    }
}
