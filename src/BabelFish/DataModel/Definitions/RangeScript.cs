using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// A RangeScript details the range command script for the chief range officer to give 
    /// during the course of fire, the set up and configuration of Athena compliant ESTs, 
    /// and the labeling for paper targets. There can be multiple RangeScripts per COURSE OF FIRE. 
    /// Each one can be designed for ESTs, paper targets, or both (although in practice it is usually one or the other).
    /// </summary>
    public class RangeScript : IReconfigurableRulebookObject, IJsonOnDeserialized {

        private List<string> validationErrorList = new List<string>();
        private bool defaultCommandMissing = false;
        private bool defaultSegmentMissing = false;

        /// <summary>
        /// RangeScript constructor.
        /// <para>Unless you are a deserializer, the preferred method for creating a new RangeScript is to use <see cref="CourseOfFire.AddRangeScript"/></para>
        /// </summary>
        public RangeScript() {
            RangeScriptName = "";

            SegmentGroups = new List<SegmentGroup>();
            DesignedForEST = false;
            DesignedForPaper = false;
        }

        [OnDeserialized]
        internal void OnDeserializedMethod( StreamingContext context ) => OnDeserialized();

        /// <inheritdoc />
        public void OnDeserialized() {
            foreach (var sg in SegmentGroups) {

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
        /// Range Scripts can be composed to be one of five types.
        /// <list type="ul">
        /// <item>FORMAL_MATCH: Range Officer controlled</item>
        /// <item>FORMAL_PRACTICE: Mimics a FORMAL_MATCH but participant controls the commands.</item>
        /// <item>INFORMAL_PRACTICE</item>
        /// <item>DRILL</item>
        /// <item>GAME</item>
        /// </list>
        /// </summary>
        /// <remarks>EKA Note Apr 2026. RangeScriptType and CourseOfFireType need to be refactored. Not really in use currently.</remarks>
        public RangeScriptType RangeScriptType { get; set; } = RangeScriptType.FORMAL_MATCH;

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

        /// <summary>
        /// Default values for each SegmentGroupCommand in the SegmentGroup.Commands list, if a field value is not listed.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 4 )]
        [G_NS.JsonProperty( Order = 4 )]
        [DefaultValue( null )]
        public SegmentGroupCommand DefaultCommand { get; set; } = new SegmentGroupCommand();

        /// <summary>
        /// Default value for each SegmentGroupSegment in the SegmentGroup.Segments list, if a field value is not listed.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 5 )]
        [G_NS.JsonProperty( Order = 5 )]
        [DefaultValue( null )]
        public SegmentGroupSegment DefaultSegment { get; set; } = new SegmentGroupSegment();

        /// <summary>
        /// List of SegmentGroups used to help run the match.
        /// <para>Unless you are a deserializer, the preferred method for creating a new SegmentGroup is to use <see cref="AddSegmentGroup"/></para>
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 6 )]
        [G_NS.JsonProperty( Order = 6 )]
        public List<SegmentGroup> SegmentGroups { get; set; } = new List<SegmentGroup>();

        /// <summary>
        /// Authors internal comments for documentation
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 100 )]
        [G_NS.JsonProperty( Order = 100 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        #region Methods
        /// <summary>
        /// Prefered method for creating a new <see cref="SegmentGroup"/> within this RangeScript. As this method will
        /// ensure that the DefaultCommand and DefaultSegment are properly set for the new SegmentGroup.
        /// </summary>
        /// <param name="segmentGroupName"></param>
        /// <returns></returns>
        public SegmentGroup AddSegmentGroup( string segmentGroupName ) {
            var sg = new SegmentGroup();
            sg.SegmentGroupName = segmentGroupName;
            SegmentGroups.Add( sg );
            sg.DefaultCommand.Parent = DefaultCommand;
            sg.DefaultSegment.Parent = DefaultSegment;
            return sg;
        }
        /// <inheritdoc />
        public override string ToString() {
            return RangeScriptName;
        }
        #endregion
    }
}
