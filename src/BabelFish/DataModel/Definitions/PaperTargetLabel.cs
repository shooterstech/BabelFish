using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// A PaperTargetLabel details how paper targets should be labeled for the COURSE OF FIRE. Each 
    /// PaperTargetLabel is an option for a different type of target and shots per bull.
    /// </summary>
    public class PaperTargetLabel : IReconfigurableRulebookObject {

        /// <summary>
        ///  When a <see cref="CourseOfFire"/> RangeScripts are all designed only for ESTs (and not for paper),
        ///  this default PaperTargetLabel, which does not specify
        ///  any labels are to be printed, is returned.
        ///  <para>To use, it is best to Clone() the instance. </para>
        /// </summary>
        public readonly static PaperTargetLabel NONE = new PaperTargetLabel() {
            PaperTargetLabelName = "None",
            ShotsPerBull = 1,
            Labels = new List<BarcodeLabel>()
        };

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
        [G_NS.JsonProperty( Order = 1 )]
        public string PaperTargetLabelName { get; set; } = string.Empty;

        /// <summary>
        /// The number of shots an athlete should fire per aiming bull and the number of shots the scoring algorithm is expecting to find.
        /// <para>Value must be greater than or equal to 0. A value of 0 is allowed and usually is reserved for scorecards.</para>
        /// </summary>
        [DefaultValue( 1 )]
        [G_NS.JsonProperty( Order = 2, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public int ShotsPerBull { get; set; } = 1;

        /// <summary>
        /// List of BarcodeLabels that specify how barcode labels should be printed.
        /// </summary>
        [G_NS.JsonProperty( Order = 10 )]
        public List<BarcodeLabel> Labels { get; set; }


        /// <inheritdoc/>
        [G_NS.JsonProperty( Order = 100 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        /// <inheritdoc/>
        public override string ToString() {
            return PaperTargetLabelName;
        }
    }
}
