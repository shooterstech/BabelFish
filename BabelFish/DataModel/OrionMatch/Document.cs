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
    public class Document : ITelerikBindModel {

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

        /// <inheritdoc/>
        public string TextField {
            get {
                return DocumentName;
            }
        }

        /// <inheritdoc/>
        public string ValueField {
            get {
                return DocumentName;
            }
        }
    }

    public static class DocumentListExstensions {

        /// <summary>
        /// Returns a subset of the List of Documents, of those whose file enstension
        /// matches that passed in value.
        /// </summary>
        /// <param name="documents"></param>
        /// <param name="fileExstension"></param>
        /// <returns></returns>
        public static List<Document> GetDocumentsOfType( this List<Document> documents, string fileExstension ) {

            fileExstension = fileExstension.Trim().ToLower();

            List<Document> list = new List<Document>();
            foreach ( Document document in documents ) {
                if ( document.FileName.ToLower().EndsWith( fileExstension ) ) {
                    list.Add( document );
                }
            }

            return list;
        }
    }
}
