using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShootersTech.BabelFish.DataModel.Definitions {
    /// <summary>
    /// A PaperTargetLabel details how paper targets should be labeled for the COURSE OF FIRE. Each 
    /// PaperTargetLabel is an option for a different type of target and shots per bull.
    /// </summary>
    public class PaperTargetLabel {

        private List<string> validationErrorList = new List<string>();

        public PaperTargetLabel() {
            PaperTargetLabelName = "";
            Labels = new List<BarcodeLabel>();
            ShotsPerBull = 1;
        }

        /// <summary>
        ///  unique human readable name given to this PaperTargetLable, that also describes the type of target to be used with this option.
        /// </summary>
        public string PaperTargetLabelName { get; set; }

        /// <summary>
        /// The number of shots an athlete should fire per aiming bull and the number of shots the scoring algorithm is expecting to find.
        /// </summary>
        public int ShotsPerBull { get; set; }

        /// <summary>
        /// List of BarcodeLabels that specify how barcode labels should be printed.
        /// </summary>
        public List<BarcodeLabel> Labels { get; set; }

        public override string ToString() {
            return PaperTargetLabelName;
        }
    }
}
