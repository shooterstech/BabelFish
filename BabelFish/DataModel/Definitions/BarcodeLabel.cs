using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// A BarcodeLabel object details how physical barcode labels should be printed and for what type of target. It is a child object of PaperTargetLabel.
    /// </summary>
    public class BarcodeLabel : IReconfigurableRulebookObject, ICopy<BarcodeLabel> {

        /// <summary>
        /// Public constructor
        /// </summary>
        public BarcodeLabel() {
        }

        /// <inheritdoc />
        public BarcodeLabel Copy() {
            BarcodeLabel copy = new BarcodeLabel();
            copy.StageLabel = this.StageLabel;
            copy.Series = this.Series;
            copy.TargetName = this.TargetName;
            copy.LabelSize = this.LabelSize;

            return copy;
        }

        /// <summary>
        /// A unique value that is used in the mapping process of shots to events. StageLabels are defined in Singular objects.
        /// StageLabel values are traditionally a single character.
        /// </summary>
        public string StageLabel { get; set; } = string.Empty;

        /// <summary>
        /// The series numbers to print on the barcode labels. Must be formated as a ValueSeries
        /// </summary>
        public string Series { get; set; } = string.Empty;

        /// <summary>
        /// Interprets Series and returns a list of ints that it represents.
        /// </summary>
        /// <returns></returns>
        public List<int> GetSeriesAsList() {
            ValueSeries vs = new ValueSeries(Series);
            return vs.GetAsList();
        }

        /// <summary>
        /// The formal name of the paper target to use with this BarcodeLabel option.
        /// </summary>
        public string TargetName { get; set; } = string.Empty;

        /// <summary>
        /// The size of barcode labels that should be used for printing. To avoid future name colision, the original product name is used, not the Orion 'small' or 'large' barcode label as used in the product. Value must be one of the following:
        /// OL385(for Small barcode labels)
        /// OL161(for Large barcode labels).
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public BarcodeLabelSize LabelSize { get; set; } = BarcodeLabelSize.OL385;

        /// <inheritdoc/>
        [JsonProperty( Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        /// <inheritdoc/>
        public override string ToString() {
            return $"{StageLabel} {TargetName}";
        }
    }
}
