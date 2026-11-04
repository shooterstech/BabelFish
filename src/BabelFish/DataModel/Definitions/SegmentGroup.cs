using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// A SegmentGroup is a series of Segments and Commands for one portion of a COURSE OF FIRE. A Segment 
    /// defines the controls the athlete and target have. Commands define the Range Officer script and the controls over the targets.
    /// </summary>
    public class SegmentGroup : IReconfigurableRulebookObject, IJsonOnDeserialized {

        public const int DEFAULT_INT = -9999;
        public const string DEFAULT_STR = "";

        /// <summary>
        /// Public constructor
        /// </summary>
        public SegmentGroup() {
        }

        [OnDeserialized]
        internal void OnDeserializedMethod( StreamingContext context ) => OnDeserialized();

        /// <inheritdoc />
        public void OnDeserialized() {

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
        [JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        public string SegmentGroupName { get; set; } = string.Empty;

        /// <summary>
        /// Default values to use when fields are not included in objects in the Commands list.
        /// </summary>
        [JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        public SegmentGroupCommand DefaultCommand { get; set; } = new SegmentGroupCommand();

        /// <summary>
        /// Default values to use when fields are not included in objects in the Segments list.
        /// </summary>
        [JsonPropertyOrder( 3 )]
        [G_NS.JsonProperty( Order = 3 )]
        public SegmentGroupSegment DefaultSegment { get; set; } = new SegmentGroupSegment();

        /// <summary>
        /// The list of Commands. A Command object specifies the state of the EST Targets and Monitors for the 
        /// current command. It also lists the text of the range officer's commands and notes for the ROs.
        /// <para>Unless you happen to be a deserializer, the preferred method for creating a new SegmentGroupCommand is to use <see cref="AddCommand"/></para>
        /// </summary>
        [JsonPropertyOrder( 4 )]
        [G_NS.JsonProperty( Order = 4 )]
        public List<SegmentGroupCommand> Commands { get; set; } = new List<SegmentGroupCommand>();

        /// <summary>
        /// The list of Segments. A Segment object controls how shots are labeled and scored during the segment of the match. It also specifies 
        /// what capabilities the athlete has over the EST Target.
        /// <para>Unless you happen to be a deserializer, the preferred method for creating a new SegmentGroupSegment is to use <see cref="AddSegment"/></para>
        /// </summary>
        [JsonPropertyOrder( 5 )]
        [G_NS.JsonProperty( Order = 5 )]
        public List<SegmentGroupSegment> Segments { get; set; } = new List<SegmentGroupSegment>();

        /// <inheritdoc />
        [DefaultValue( "" )]
        [JsonPropertyOrder( 100 )]
        public string Comment { get; set; } = string.Empty;

        #region Methods
        /// <summary>
        /// Preferred method for adding a new <see cref="SegmentGroupCommand"/> to this SegmentGroup. As this method also sets the
        /// values for the SegmentGroupCommand's Parent.
        /// </summary>
        /// <returns></returns>
        public SegmentGroupCommand AddCommand() {
            var c = new SegmentGroupCommand();
            Commands.Add( c );
            c.Parent = DefaultCommand;
            return c;
        }

        /// <summary>
        /// Preferred method for adding a new <see cref="SegmentGroupSegment"/> to this SegmentGroup. As this method also sets the
        /// values for the SegmentGroupSegment's Parent.
        /// </summary>
        /// <returns></returns>
        public SegmentGroupSegment AddSegment() {
            var s = new SegmentGroupSegment();
            Segments.Add( s );
            s.Parent = DefaultSegment;
            return s;
        }

        /// <inheritdoc />
        public override string ToString() {
            return SegmentGroupName;
        }
        #endregion
    }
}
