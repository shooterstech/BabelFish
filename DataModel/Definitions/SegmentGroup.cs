using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BabelFish.DataModel.Definitions {
    /// <summary>
    /// A SegmentGroup is a series of Segments and Commands for one portion of a COURSE OF FIRE. A Segment 
    /// defines the controls the athlete and target have. Commands define the Range Officer script and the controls over the targets.
    /// </summary>
    public class SegmentGroup {

        private List<string> validationErrorList = new List<string>();
        private bool defaultCommandMissing = false;
        private bool defaultSegmentMissing = false;

        public SegmentGroup() {
            Commands = new List<SegmentGroupCommand>();
            Segments = new List<SegmentGroupSegment>();
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context) {

            if (DefaultSegment == null) {
                DefaultSegment = new SegmentGroupSegment();
                defaultSegmentMissing = true;
            }

            if (DefaultCommand == null) {
                DefaultCommand = new SegmentGroupCommand();
                defaultCommandMissing = true;
            }

            if (Commands != null)
                foreach (var c in Commands) {
                    c.Parent = DefaultCommand;
                }

            if (Segments != null)
                foreach (var s in Segments) {
                    s.Parent = DefaultSegment;
                }
        }

        /// <summary>
        /// A unique short human readable name given to this SegmentGroup.
        /// </summary>
        [JsonProperty(Order = 1)]
        public string SegmentGroupName { get; set; }

        /// <summary>
        /// The list of Commands. A Command object specifies the state of the EST Targets and Monitors for the 
        /// current command. It also lists the text of the range officer's commands and notes for the ROs.
        /// </summary>
        [JsonProperty(Order = 4)]
        public List<SegmentGroupCommand> Commands { get; set; }

        /// <summary>
        /// The list of Segments. A Segment object controls how shots are labeled and scored during the segment of the match. It also specifies 
        /// what capabilities the athlete has over the EST Target.
        /// </summary>
        [JsonProperty(Order = 5)]
        public List<SegmentGroupSegment> Segments { get; set; }

        [JsonProperty(Order = 2)]
        public SegmentGroupCommand DefaultCommand { get; set; }

        [JsonProperty(Order = 3)]
        public SegmentGroupSegment DefaultSegment { get; set; }

        /// <summary>
        /// Authors internal comments for documentation
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(Order = 100)]
        public string Comment { get; set; }

        public override string ToString() {
            return SegmentGroupName;
        }
    }
}
