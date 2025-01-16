using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// A RangeScript details the range command script for the chief range officer to give 
    /// during the course of fire, the set up and configuration of Athena compliant ESTs, 
    /// and the labeling for paper targets. There can be multiple RangeScripts per COURSE OF FIRE. 
    /// Each one can be designed for ESTs, paper targets, or both (although in practice it is usually one or the other).
    /// </summary>
    public class RangeScript : IReconfigurableRulebookObject {

        private List<string> validationErrorList = new List<string>();
        private bool defaultCommandMissing = false;
        private bool defaultSegmentMissing = false;

        public RangeScript() {
            RangeScriptName = "";

            PaperTargetLabels = new List<PaperTargetLabel>();
            SegmentGroups = new List<SegmentGroup>();
            DesignedForEST = false;
            DesignedForPaper = false;
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context) {
            foreach( var sg in SegmentGroups) {

                if (DefaultSegment == null && DesignedForEST) {
                    DefaultSegment = new SegmentGroupSegment();
                    defaultSegmentMissing = true;
                }

                if (DefaultCommand == null) {
                    DefaultCommand = new SegmentGroupCommand();
                    defaultCommandMissing = true;
                }

                sg.DefaultCommand.Parent = DefaultCommand;
                sg.DefaultSegment.Parent = DefaultSegment;
            }
        }

        /// <summary>
        /// A unique human readable name given to this RangeScript.
        /// </summary>
        [JsonPropertyOrder( 1)]
        public string RangeScriptName { get; set; } = string.Empty;

        /// <summary>
        /// True if this RangeScript is intended to be used with Athena compliant ESTs. False if it is not.
        /// </summary>
        [JsonPropertyOrder( 2)]
        public bool DesignedForEST { get; set; }

        /// <summary>
        /// True if this RangeScript is intended to be used with paper targets for scoring with Orion. False if it is not. 
        /// </summary>
        [JsonPropertyOrder( 3)]
        public bool DesignedForPaper { get; set; }

        /// <summary>
        /// List of available options for printing barcode labels on paper targets.
        /// </summary>
        [JsonPropertyOrder( 4)]
        public List<PaperTargetLabel> PaperTargetLabels { get; set; } = new List<PaperTargetLabel>();

        /// <summary>
        /// List of SegmentGroups used to help run the match.
        /// </summary>
        [JsonPropertyOrder( 7)]
        public List<SegmentGroup> SegmentGroups { get; set; } = new List<SegmentGroup> ();

        [DefaultValue(null)]
        [JsonPropertyOrder( 5)]
        public SegmentGroupCommand DefaultCommand { get; set; } = new SegmentGroupCommand();

        [DefaultValue(null)]
        [JsonPropertyOrder( 6)]
        public SegmentGroupSegment DefaultSegment { get; set; } = new SegmentGroupSegment();

        /// <summary>
        /// Authors internal comments for documentation
        /// </summary>
        [DefaultValue( "" )]
        [JsonPropertyOrder ( 100 )]
        public string Comment { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string ToString() {
            return RangeScriptName;
        }
    }
}
