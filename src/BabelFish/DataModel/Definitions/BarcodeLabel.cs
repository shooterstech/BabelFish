using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// A BarcodeLabel object details how physical barcode labels should be printed and for what type of target. It is a child object of PaperTargetLabel.
    /// </summary>
    public class BarcodeLabel : IReconfigurableRulebookObject {

        /// <summary>
        /// Public constructor
        /// </summary>
        public BarcodeLabel() {
        }

        /// <summary>
        /// A unique value that is used in the mapping process of shots to events. StageLabels are defined in Singular objects.
        /// StageLabel values are traditionally a single character.
        /// </summary>
        public string StageLabel { get; set; } = string.Empty;

        /// <summary>
        /// The series numbers to print on the barcode labels. Must be formated as a ValueSeries
        /// </summary>
        [DefaultValue( "1" )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.ValueSeriesConverter ) )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ValueSeriesConverter ) )]
        public ValueSeries Series { get; set; } = new ValueSeries( "1" );

        /// <summary>
        /// The formal name of the paper target to use with this BarcodeLabel option.
        /// </summary>
        public string TargetName { get; set; } = string.Empty;

		/// <summary>
		/// The size of barcode labels that should be used for printing. To avoid future name colision, the original product name is used, not the Orion 'small' or 'large' barcode label as used in the product. Value must be one of the following:
		/// OL385(for Small barcode labels)
		/// OL161(for Large barcode labels).
		/// </summary>
		[G_NS.JsonProperty( DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
		public BarcodeLabelSize LabelSize { get; set; } = BarcodeLabelSize.OL385;

        /// <inheritdoc/>
        [JsonPropertyOrder(99)]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        /// <inheritdoc/>
        public override string ToString() {
            return $"{StageLabel} {TargetName}";
        }
    }
}
