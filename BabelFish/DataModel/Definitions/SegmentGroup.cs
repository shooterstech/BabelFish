using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// A SegmentGroup is a series of Segments and Commands for one portion of a COURSE OF FIRE. A Segment 
    /// defines the controls the athlete and target have. Commands define the Range Officer script and the controls over the targets.
    /// </summary>
    public class SegmentGroup: IReconfigurableRulebookObject, ICopy<SegmentGroup> {

        /// <summary>
        /// Public constructor
        /// </summary>
        public SegmentGroup() {
        }

        /// <inheritdoc />
        public SegmentGroup Copy() {
            SegmentGroup copy = new SegmentGroup();
            copy.SegmentGroupName = this.SegmentGroupName;
            copy.DefaultCommand = this.DefaultCommand.Copy();
            copy.DefaultSegment = this.DefaultSegment.Copy();

            if (this.Commands != null ) {
                foreach( var c in this.Commands ) {
                    copy.Commands.Add( c.Copy() );
                }
            }
            if (this.Segments != null ) {
                foreach( var s in this.Segments ) {
                    copy.Segments.Add( s.Copy() );
                }
            }

            return copy;
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context) {

            if (DefaultSegment == null) {
                DefaultSegment = new SegmentGroupSegment();
            }

            if (DefaultCommand == null) {
                DefaultCommand = new SegmentGroupCommand();
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
        public string SegmentGroupName { get; set; } = string.Empty;

        /// <summary>
        /// The list of Commands. A Command object specifies the state of the EST Targets and Monitors for the 
        /// current command. It also lists the text of the range officer's commands and notes for the ROs.
        /// </summary>
        [JsonProperty(Order = 4)]
        public List<SegmentGroupCommand> Commands { get; set; } = new List<SegmentGroupCommand>();

        /// <summary>
        /// The list of Segments. A Segment object controls how shots are labeled and scored during the segment of the match. It also specifies 
        /// what capabilities the athlete has over the EST Target.
        /// </summary>
        [JsonProperty(Order = 5)]
        public List<SegmentGroupSegment> Segments { get; set; } = new List<SegmentGroupSegment>();

        /// <summary>
        /// Default values to use when fields are not included in objects in the Commands list.
        /// </summary>
        [JsonProperty( Order = 2 )]
        public SegmentGroupCommand DefaultCommand { get; set; } = new SegmentGroupCommand();

        /// <summary>
        /// Default values to use when fields are not included in objects in the Segments list.
        /// </summary>
        [JsonProperty( Order = 3 )]
        public SegmentGroupSegment DefaultSegment { get; set; } = new SegmentGroupSegment();

        /// <inheritdoc />
        [DefaultValue("")]
        [JsonProperty(Order = 100)]
        public string Comment { get; set; } = string.Empty ;

        /// <inheritdoc />
        public override string ToString() {
            return SegmentGroupName;
        }
    }
}
