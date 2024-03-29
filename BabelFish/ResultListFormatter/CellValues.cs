using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Definitions;


namespace Scopos.BabelFish.ResultListFormatter {

    /// <summary>
    /// Cell Values represent the what and the how to display in a result list cell. A cell meaning
    /// one section in a result list row. Probable implemented as a <td /> or a <div />.
    ///
    /// A CellValue can represent cells in the header, in a result row, or child row, or footer.
    /// </summary>
    public class CellValues {

        public CellValues() {
            Text = "";
            ClassList = new List<string>();
        }

        public CellValues( string text, List<string> cellClasses ) {
            ClassList = cellClasses;
            Text = text;
        }

        /// <summary>
        /// The text to display in the cell. The text would have already been formatted into
        /// a string, based on the ResultListFormat's .Format.Column values.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The list of CSS Classes to assign to the cell's class attribute.
        /// </summary>
        public List<string> ClassList { get; set; }

        /// <summary>
        /// Indicates what page or view this Cell should link to.
        /// The data to link to this page is found in the LinkToData property.
        /// The default value is None, indicating no link.
        /// </summary>
        public LinkToOption LinkTo { get; set; } = LinkToOption.None;


        public string LinkToData { get; set; } = "";

        public override string ToString() {
            return $"CellValues: {Text}";
        }
    }
}
