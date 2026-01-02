using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// A PaperTargetLabel details how paper targets should be labeled for the COURSE OF FIRE. Each 
    /// PaperTargetLabel is an option for a different type of target and shots per bull.
    /// </summary>
    public class PaperTargetLabel : IReconfigurableRulebookObject {

        /// <summary>
        /// Public constructor
        /// </summary>
        public PaperTargetLabel() {
            PaperTargetLabelName = "";
            Labels = new List<BarcodeLabel>();
            ShotsPerBull = 1;
        }

        /// <summary>
        ///  unique human readable name given to this PaperTargetLable, that also describes the type of target to be used with this option.
        /// </summary>
        public string PaperTargetLabelName { get; set; } = string.Empty;

        /// <summary>
        /// The number of shots an athlete should fire per aiming bull and the number of shots the scoring algorithm is expecting to find.
        /// </summary>
        [DefaultValue( 1 )]
        public int ShotsPerBull { get; set; } = 1;

        /// <summary>
        /// List of BarcodeLabels that specify how barcode labels should be printed.
        /// </summary>
        public List<BarcodeLabel> Labels { get; set; }


        /// <inheritdoc/>
        [JsonPropertyOrder ( 99 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        /// <inheritdoc/>
        public override string ToString() {
            return PaperTargetLabelName;
        }
    }
}
