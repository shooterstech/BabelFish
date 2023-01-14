using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class BarcodeLabel {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum BarcodeLabelSize { OL385, OL161 }

        private List<string> validationErrorList = new List<string>();

        public BarcodeLabel() {
            LabelSize = BarcodeLabelSize.OL385;
            StageLabel = "";
            SeriesLabels = "";
            TargetName = "";
        }

        /// <summary>
        /// A unique value that is used in the mapping process of shots to events. StageLabels are defined in Singular objects.
        /// StageLabel values are traditionally a single character.
        /// </summary>
        public string StageLabel { get; set; }

        /// <summary>
        /// The series numbers to print on the barcode labels. Must be formated as a ValueSeries
        /// </summary>
        public string SeriesLabels { get; set; }

        public List<int> GetSeriesAsList() {
            ValueSeries vs = new ValueSeries(SeriesLabels);
            return vs.GetAsList();
        }

        /// <summary>
        /// The formal name of the paper target to use with this BarcodeLabel option.
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// The size of barcode labels that should be used for printing. To avoid future name colision, the original product name is used, not the Orion 'small' or 'large' barcode label as used in the product. Value must be one of the following:
        /// OL385(for Small barcode labels)
        /// OL161(for Large barcode labels).
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public BarcodeLabelSize LabelSize { get; set; }

        public override string ToString() {
            return $"{StageLabel} {TargetName}";
        }
    }
}
