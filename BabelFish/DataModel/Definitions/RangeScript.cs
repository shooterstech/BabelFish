using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

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
		[G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        public string RangeScriptName { get; set; } = string.Empty;

        /// <summary>
        /// True if this RangeScript is intended to be used with Athena compliant ESTs. False if it is not.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        public bool DesignedForEST { get; set; }

        /// <summary>
        /// True if this RangeScript is intended to be used with paper targets for scoring with Orion. False if it is not. 
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 3 )]
        [G_NS.JsonProperty( Order = 3 )]
        public bool DesignedForPaper { get; set; }

        [G_STJ_SER.JsonPropertyOrder( 4 )]
        [G_NS.JsonProperty( Order = 4 )]
        [DefaultValue(null)]
        public SegmentGroupCommand DefaultCommand { get; set; } = new SegmentGroupCommand();

        [G_STJ_SER.JsonPropertyOrder( 5 )]
        [G_NS.JsonProperty( Order = 5 )]
        [DefaultValue(null)]
        public SegmentGroupSegment DefaultSegment { get; set; } = new SegmentGroupSegment();

        /// <summary>
        /// List of SegmentGroups used to help run the match.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 6 )]
        [G_NS.JsonProperty( Order = 6 )]
        public List<SegmentGroup> SegmentGroups { get; set; } = new List<SegmentGroup>();

        /// <summary>
        /// List of available options for printing barcode labels on paper targets.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 7 )]
        [G_NS.JsonProperty( Order = 7 )]
        public List<PaperTargetLabel> PaperTargetLabels { get; set; } = new List<PaperTargetLabel>();

        /// <summary>
        /// Authors internal comments for documentation
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 100 )]
        [G_NS.JsonProperty( Order = 100 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string ToString() {
            return RangeScriptName;
        }
    }
}
