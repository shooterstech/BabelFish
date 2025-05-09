using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// A SegmentGroupSegment object (sometimes simply called a Segment) controls how shots are labeled and scored during the segment of the match. It also specifies what capabilities the athlete has over the EST Target.
    /// </summary>
    public class SegmentGroupSegment : IReconfigurableRulebookObject {

        private const int DEFAULT_INT = -9999;
        private const string DEFAULT_STR = "";

        /// <summary>
        /// Public Constructor
        /// </summary>
        public SegmentGroupSegment() {
        }

        [JsonIgnore]
        protected internal SegmentGroupSegment Parent { get; set; }

        /// <summary>
        /// A unique name given to this segment.
        /// Segments: Required and must be unique
        /// DefaultSegment: Ignored
        /// </summary>
        [DefaultValue( "" )]
        [JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1)]
        public string SegmentName { get; set; }

        public string GetSegmentName() {
            return SegmentName;
        }

        /// <summary>
        /// The expected number of shots to be fired during this segment. The value -1 indicates shots are 
        /// expected and an unlimited number of shots could be fired (i.e. sighters). The value 0 indicates not shots are expected. 
        /// Segments: Required
        /// DefaultSegments: Ignored
        /// </summary>
        [JsonPropertyOrder( 3 )]
        [G_NS.JsonProperty( Order = 3 )]
        [DefaultValue( -9999 )]
        public int NumberOfShots { get; set; } = DEFAULT_INT;

        public int GetNumberOfShots() {
            if (NumberOfShots != DEFAULT_INT)
                return NumberOfShots;

            if (Parent.NumberOfShots != DEFAULT_INT)
                return Parent.NumberOfShots;

            return Parent.Parent.NumberOfShots;
        }

        /// <summary>
        /// The stage label that is applied to each shot that is fired during this segment. Stage labels are usually 
        /// represented by a single character ('P', 'S', 'K'). They are used to map shots to Singulars. 
        /// Segments: Not required. If missing or "" uses DefaultSegment.StageLabel
        /// DefaultSegment: Required
        /// </summary>
        [DefaultValue( "" )]
        [JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        public string StageLabel { get; set; } = DEFAULT_STR;

        public string GetStageLabel() {
            if (StageLabel != DEFAULT_STR)
                return StageLabel;

            if (Parent.StageLabel != DEFAULT_STR)
                return Parent.StageLabel;

            return Parent.Parent.StageLabel;
        }

        /// <summary>
        /// Specifies the TargetDef to use during this Segment. Specifically, this is the index into the 
        /// CourseOfFire.TargetCollectionDef.TargetCollections.TargetDefs array.
        /// </summary>
        [DefaultValue( DEFAULT_INT )]
        [JsonPropertyOrder( 4 )]
        [G_NS.JsonProperty( Order = 4 )]
        public int TargetCollectionIndex { get; set; } = DEFAULT_INT;

        public int GetTargetCollectionIndex() {
            if (TargetCollectionIndex != DEFAULT_INT)
                return TargetCollectionIndex;

            if (Parent.TargetCollectionIndex != DEFAULT_INT)
                return Parent.TargetCollectionIndex;

            if (Parent.Parent.TargetCollectionIndex != DEFAULT_INT)
                return Parent.Parent.TargetCollectionIndex;

            return 0;
        }

        /// <summary>
        /// Commands: Not required, missing or value of -9999 uses DefaultCommand.TargetHeight
        /// DefaultCommand: Required with default value 0
        /// </summary>
        [DefaultValue( -9999 )]
        [JsonPropertyOrder( 5 )]
        [G_NS.JsonProperty( Order = 5 )]
        public int TargetHeight { get; set; } = DEFAULT_INT;

        public int GetTargetHeight() {
            if (TargetHeight != DEFAULT_INT)
                return TargetHeight;

            if (Parent.TargetHeight != DEFAULT_INT)
                return TargetHeight;

            return Parent.Parent.TargetHeight;
        }

        /// <summary>
        /// Values must be one of
        /// TargetLight
        /// TargetLift
        /// TargetLiftDefault
        /// TargetLift(min, max)
        /// TargetLift('PosOne(height)', 'PosTwo(height)', 'PosThree(height)')
        /// ShotPresentation
        /// Pause (Need to figure out what this does on the Monitor.)
        /// Series (Need to figure out what this does on the Monitor, might be a function, e.g. Series() .)
        /// Group (Need to figure out what this does on the Monitor.)
        /// AdvancedSettings (Need to figure out what this does on the Monitor.)
        /// !AdvancedSettings (Need to figure out what this does on the Monitor.)
        /// Series(INFORMAL) Deprecated, needs to be removed.
        /// Commands: Not required, missing or null uses DefaultCommand.AthleteHasControl
        /// DefaultCommand: Required with default value of [ ]
        /// </summary>
        // TODO: Rearchitect. Instead of a list of strings, with some strings contain special characters that represent function actions, replace with list of objects.
        [DefaultValue( null )]
        [JsonPropertyOrder( 6 )]
        [G_NS.JsonProperty( Order = 6 )]
        public List<string> AthleteHasControl { get; set; } = new List<string>();

        public List<string> GetAthleteHasControl() {
            if (AthleteHasControl != null)
                return AthleteHasControl;

            if (Parent.AthleteHasControl != null)
                return Parent.AthleteHasControl;

            return Parent.Parent.AthleteHasControl;
        }

        /// <summary>
        /// List of other SegmentGroupSegments identified by the SegmentName that the athlete has the option to advance to next. 
        /// Commands: Not required, missing or null uses DefaultCommand.NextSegments
        /// DefaultCommand: Required with default value of [ ]
        /// </summary>
        [DefaultValue( null )]
        [JsonPropertyOrder( 7 )]
        [G_NS.JsonProperty( Order = 7 )]
        public List<string> NextSegments { get; set; } = new List<string>();

        public List<string> GetNextSegments() {
            if (NextSegments != null)
                return NextSegments;

            if (Parent.NextSegments != null)
                return Parent.NextSegments;

            return NextSegments;
        }

        /// <summary>
        /// A list of ShotAttributes that should decorate a Shot if fired during this SegmentGroupCommand.
        /// Must be one of the following
        /// SIGHTER
        /// STOP
        /// Commands: Not requried, missing or null uses DefaultCommand.ShotAttributes
        /// DefaultCommand: Required with default value of [ ] 
        /// </summary>
        [DefaultValue( null )]
        [JsonPropertyOrder( 8 )]
        [G_NS.JsonProperty( Order = 8 )]
        public List<string> ShotAttributes { get; set; } = new List<string>();

        public List<string> GetShotAttributes() {
            if (ShotAttributes != null)
                return ShotAttributes;

            if (Parent.ShotAttributes != null)
                return Parent.ShotAttributes;

            return Parent.Parent.ShotAttributes;
        }

        /// <summary>
        /// The name of the AbbreviatedFormat to use to display scores to the athlete on the monitor.
        /// </summary>
        [JsonPropertyOrder( 9 )]
        [DefaultValue( "" )]
        [G_NS.JsonProperty( Order = 9 )]
        public string AbbreviatedFormat { get; set; } = DEFAULT_STR;

        public string GetAbbreviatedFormat() {
            if (AbbreviatedFormat != "")
                return AbbreviatedFormat;

            if (Parent.AbbreviatedFormat != "")
                return Parent.AbbreviatedFormat;

            return Parent.Parent.AbbreviatedFormat;
        }

        /// <summary>
        /// Indicates what type of shots to display within the Athlete EST Monitor or Spectator EST Display.
        /// </summary>
        [JsonPropertyOrder( 10 )]
        [G_NS.JsonProperty( Order = 10 )]
        public ShowInSegment Show { get; set; } = new ShowInSegment();

        public ShowInSegment GetShow() {
            if (Show != null)
                return Show;

            if (Parent.Show != null)
                return Parent.Show;

            if (Parent.Parent.Show != null)
                return Parent.Parent.Show;

            return new ShowInSegment() {
                StageLabel = new List<string>(),
                Competition = CompetitionType.BOTH
            };
        }

        /// <summary>
        /// Unique display mode specifics for this segement.
        /// Must be one of the following
        /// GroupMode
        /// ShotCalling
        /// Commands: Not requried, missing or null uses DefaultCommand.ShotAttributes
        /// DefaultCommand: Required with default value of [ ] 
        /// </summary>
        [DefaultValue( null )]
        [JsonPropertyOrder( 11 )]
        [G_NS.JsonProperty( Order = 11 )]
        public List<SpecialOptions> Special { get; set; } = new List<SpecialOptions>();

        public List<SpecialOptions> GetSpecial() {
            if (Special != null)
                return Special;

            if (Parent.Special != null)
                return Parent.Special;

            return Parent.Parent.Special;
        }

        /// <summary>
        /// The number of shots in a string, used for displaying shots purposes only.
        /// </summary>
        [JsonPropertyOrder( 12 )]
        [G_NS.JsonProperty( Order = 12 )]
        [DefaultValue( -9999 )]
        public int StringSize { get; set; } = DEFAULT_INT;

        public int GetStringSize() {
            if (StringSize != DEFAULT_INT)
                return StringSize;

            if (Parent.StringSize != DEFAULT_INT)
                return Parent.StringSize;

            return Parent.Parent.StringSize;
        }

        /// <summary>
        /// When set, advances the tape feed on the target this number of millimeters at the start of the Segment.
        /// </summary>
        [JsonPropertyOrder( 15 )]
        [G_NS.JsonProperty( Order = 15 )]
        [DefaultValue( 0 )]
        public int TapeAdvance { get; set; } = DEFAULT_INT;


        /// <inheritdoc/>
        [JsonPropertyOrder( 99 )]
        [G_NS.JsonProperty( Order = 100 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        /// <inheritdoc/>
        public override string ToString() {
            return SegmentName;
        }
    }
}