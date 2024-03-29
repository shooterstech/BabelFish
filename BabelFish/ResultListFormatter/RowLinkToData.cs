using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.ResultListFormatter {
    public class RowLinkToData {


        /// <summary>
        /// Indicates what page or view this Cell should link to.
        /// The data to link to this page is found in the LinkToData property.
        /// The default value is None, indicating no link.
        /// </summary>
        public LinkToOption LinkTo { get; set; } = LinkToOption.None;


        public string LinkToData { get; set; } = "";

        public override string ToString() {
            return LinkTo.Description();
        }
    }
}
