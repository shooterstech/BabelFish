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
		/// <para>Does not follow the <a href="https://support.scopos.tech/index.html?segment-and-command-value-inhe.html">value inheritance rules.</a></para>
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
		/// <para>Does follow the <a href="https://support.scopos.tech/index.html?segment-and-command-value-inhe.html">value inheritance rules.</a></para>
		/// </summary>
		[JsonPropertyOrder( 3 )]
        [G_NS.JsonProperty( Order = 3 )]
        [DefaultValue( -9999 )]
        public int NumberOfShots { get; set; } = SegmentGroup.DEFAULT_INT;

        public int GetNumberOfShots() {
            if (NumberOfShots != SegmentGroup.DEFAULT_INT)
                return NumberOfShots;

            if (Parent.NumberOfShots != SegmentGroup.DEFAULT_INT)
                return Parent.NumberOfShots;

            return Parent.Parent.NumberOfShots;
        }

		/// <summary>
		/// The stage label that is applied to each shot that is fired during this segment. Stage labels are usually 
		/// represented by a single character ('P', 'S', 'K'). They are used to map shots to Singulars. 
		/// Segments: Not required. If missing or "" uses DefaultSegment.StageLabel
		/// DefaultSegment: Required
		/// <para>Does follow the <a href="https://support.scopos.tech/index.html?segment-and-command-value-inhe.html">value inheritance rules.</a></para>
		/// </summary>
		[DefaultValue( "" )]
        [JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        public string StageLabel { get; set; } = SegmentGroup.DEFAULT_STR;

        public string GetStageLabel() {
            if (StageLabel != SegmentGroup.DEFAULT_STR)
                return StageLabel;

            if (Parent.StageLabel != SegmentGroup.DEFAULT_STR)
                return Parent.StageLabel;

            return Parent.Parent.StageLabel;
        }

		/// <summary>
		/// Specifies the TargetDef to use during this Segment. Specifically, this is the index into the 
		/// CourseOfFire.TargetCollectionDef.TargetCollections.TargetDefs array.
		/// <para>Does follow the <a href="https://support.scopos.tech/index.html?segment-and-command-value-inhe.html">value inheritance rules.</a></para>
		/// </summary>
		[DefaultValue( SegmentGroup.DEFAULT_INT )]
        [JsonPropertyOrder( 4 )]
        [G_NS.JsonProperty( Order = 4 )]
        public int TargetCollectionIndex { get; set; } = SegmentGroup.DEFAULT_INT;

        public int GetTargetCollectionIndex() {
            if (TargetCollectionIndex != SegmentGroup.DEFAULT_INT)
                return TargetCollectionIndex;

            if (Parent.TargetCollectionIndex != SegmentGroup.DEFAULT_INT)
                return Parent.TargetCollectionIndex;

            if (Parent.Parent.TargetCollectionIndex != SegmentGroup.DEFAULT_INT)
                return Parent.Parent.TargetCollectionIndex;

            return 0;
        }

		/// <summary>
		/// Commands: Not required, missing or value of -9999 uses DefaultCommand.TargetHeight
		/// DefaultCommand: Required with default value 0
		/// <para>Does follow the <a href="https://support.scopos.tech/index.html?segment-and-command-value-inhe.html">value inheritance rules.</a></para>
		/// </summary>
		[DefaultValue( -9999 )]
        [JsonPropertyOrder( 5 )]
        [G_NS.JsonProperty( Order = 5 )]
        public int TargetHeight { get; set; } = SegmentGroup.DEFAULT_INT;

        public int GetTargetHeight() {
            if (TargetHeight != SegmentGroup.DEFAULT_INT)
                return TargetHeight;

            if (Parent.TargetHeight != SegmentGroup.DEFAULT_INT)
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
		/// <para>Does follow the <a href="https://support.scopos.tech/index.html?segment-and-command-value-inhe.html">value inheritance rules.</a></para>
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
        /// <para>Does follow the <a href="https://support.scopos.tech/index.html?segment-and-command-list-rules.html">list inheritance rules.</a></para>
        /// </summary>
        [DefaultValue( null )]
        [JsonPropertyOrder( 7 )]
        [G_NS.JsonProperty( Order = 7 )]
        public List<string> NextSegments { get; set; } = new List<string>();

        /// <summary>
        /// Returns the calculated list of NextSegments, based on this Segment, the DefaultSegment, and the RangeSCript's DefaultSegment,
        /// and of course the <a href="https://support.scopos.tech/index.html?segment-and-command-list-rules.html">list inheritance rules.</a>
        /// </summary>
        /// <returns></returns>
        public List<string> GetNextSegments() {

            var list = new List<string>();

            //Start with the RangeScript's DefaultCommand .NextSegments.
            if (Parent.Parent.NextSegments != null) {
                foreach (var item in Parent.Parent.NextSegments) {
                    if (item != CONSTANT || !item.StartsWith( "!" )) {
                        list.Add( item );
                    }
                }
            }

            //Add values from the SegmentGroup's DefaultCommand
            if (Parent.NextSegments != null) {
                //Check for CONSTANT which is the override
                if (Parent.NextSegments.Contains( CONSTANT )) {
                    list.Clear();
                    foreach (var item in Parent.NextSegments) {
                        if (item != CONSTANT || !item.StartsWith( "!" )) {
                            list.Add( item );
                        }
                    }
                } else {
                    //Remove anything from the list that the user asked to be removed
                    foreach (var item in Parent.NextSegments) {
                        if (item.StartsWith( "!" ) && list.Contains( item.Substring(1) ) ) {
                            list.Remove( item.Substring( 1 ) );
                        } 
                    }
                    //Add to the list anything that is not already there
                    foreach (var item in Parent.NextSegments) {
                        if (item != CONSTANT || !item.StartsWith( "!" )) {
                            list.Add( item );
                        } 
                    }
                }
            }

            //Add values from the this
            if (NextSegments != null) {
                //Check for CONSTANT which is the override
                if (NextSegments.Contains( CONSTANT )) {
                    list.Clear();
                    foreach (var item in NextSegments) {
                        if (item != CONSTANT || !item.StartsWith( "!" )) {
                            list.Add( item );
                        }
                    }
                } else {
                    //Remove anything from the list that the user asked to be removed
                    foreach (var item in NextSegments) {
                        if (item.StartsWith( "!" ) && list.Contains( item.Substring( 1 ) )) {
                            list.Remove( item.Substring( 1 ) );
                        }
                    }
                    //Add to the list anything that is not already there
                    foreach (var item in NextSegments) {
                        if (item != CONSTANT || !item.StartsWith( "!" )) {
                            list.Add( item );
                        }
                    }
                }
            }

            return list;
        }

        private string SIGHTER = "SIGHTER";
        private string STOP = "STOP";
        private string NOT_SIGHTER = "!SIGHTER";
        private string NOT_STOP = "!STOP";
        private string CONSTANT = "CONSTANT";

        /// <summary>
        /// A list of ShotAttributes that should decorate a Shot if fired during this SegmentGroupSegment.
        /// <list type="bullet">
        /// Must be one of the following.
        /// <item>SIGHTER</item>
        /// <item>STOP</item>
        /// <item>CONSTANT (see below for meaning)</item>
        /// </list>
        /// <para>If an item is prepended with "!" (example "!SIGHTER") then it is removed from the inherited list.</para>
        /// <para>If CONSTANT is included, then all inherited values are ignored.</para>
        /// <para>Does follow the <a href="https://support.scopos.tech/index.html?segment-and-command-list-rules.html">list inheritance rules.</a></para>
        /// </summary>
        [JsonPropertyOrder( 8 )]
        [G_NS.JsonProperty( Order = 8 )]
        public List<string> ShotAttributes { get; set; } = new List<string>();

        /// <summary>
        /// Returns the calculated list of ShotAttributes, based on this Segment, the DefaultSegment, and the RangeSCript's DefaultSegment,
        /// and of course the <a href="https://support.scopos.tech/index.html?segment-and-command-list-rules.html">list inheritance rules.</a>
        /// </summary>
        /// <returns></returns>
        public List<string> GetShotAttributes() {

            var list = new List<string>();

            //Start with the RangeScript's DefaultCommand .ShotAttributes.
            //Note is only makes sense to add "FIRED BEFORE COMMAND START" or "FIRED AFTER COMMAND STOP"
            if (Parent.Parent.ShotAttributes != null) {
                foreach (var item in Parent.Parent.ShotAttributes) {
                    if (item == SIGHTER || item == STOP) {
                        list.Add( item );
                    }
                }
            }

            //Add values from the SegmentGroup's DefaultCommand
            if (Parent.ShotAttributes != null) {
                //Check for CONSTANT which is the override
                if (Parent.ShotAttributes.Contains( CONSTANT )) {
                    list.Clear();
                    foreach (var item in Parent.ShotAttributes) {
                        if (item == SIGHTER || item == STOP) {
                            list.Add( item );
                        }
                    }
                } else {
                    //Remove anything from the list that the user asked to be removed
                    foreach (var item in Parent.ShotAttributes) {
                        if (item == NOT_SIGHTER && list.Contains( SIGHTER )) {
                            list.Remove( SIGHTER );
                        } else if (item == NOT_STOP && list.Contains( STOP )) {
                            list.Remove( STOP );
                        }
                    }
                    //Add to the list anything that is not already there
                    foreach (var item in Parent.ShotAttributes) {
                        if (item == SIGHTER && !list.Contains( SIGHTER )) {
                            list.Add( SIGHTER );
                        } else if (item == STOP && !list.Contains( STOP )) {
                            list.Add( STOP );
                        }
                    }
                }
            }

            //Finally add values from the SegmentGroupCommand
            if (ShotAttributes != null) {
                //Check for CONSTANT which is the override
                if (ShotAttributes.Contains( CONSTANT )) {
                    list.Clear();
                    foreach (var item in ShotAttributes) {
                        if (item == SIGHTER || item == STOP) {
                            list.Add( item );
                        }
                    }
                } else {
                    //Remove anything from the list that the user asked to be removed
                    foreach (var item in ShotAttributes) {
                        if (item == NOT_SIGHTER && list.Contains( SIGHTER )) {
                            list.Remove( SIGHTER );
                        } else if (item == NOT_STOP && list.Contains( STOP )) {
                            list.Remove( STOP );
                        }
                    }
                    //Add to the list anything that is not already there
                    foreach (var item in ShotAttributes) {
                        if (item == SIGHTER && !list.Contains( SIGHTER )) {
                            list.Add( SIGHTER );
                        } else if (item == STOP && !list.Contains( STOP )) {
                            list.Add( STOP );
                        }
                    }
                }
            }

            return list;
        }

        public bool ShouldSerializeShotAttributes() {
            //Do not serialize if it is null or an empty list.
            return (ShotAttributes != null && ShotAttributes.Count > 0);
        }

		/// <summary>
		/// The name of the AbbreviatedFormat to use to display scores to the athlete on the monitor.
		/// <para>Does follow the <a href="https://support.scopos.tech/index.html?segment-and-command-value-inhe.html">value inheritance rules.</a></para>
		/// </summary>
		[JsonPropertyOrder( 9 )]
        [DefaultValue( "" )]
        [G_NS.JsonProperty( Order = 9 )]
        public string AbbreviatedFormat { get; set; } = SegmentGroup.DEFAULT_STR;

        public string GetAbbreviatedFormat() {
            if (AbbreviatedFormat != "")
                return AbbreviatedFormat;

            if (Parent.AbbreviatedFormat != "")
                return Parent.AbbreviatedFormat;

            return Parent.Parent.AbbreviatedFormat;
        }

		/// <summary>
		/// Indicates what type of shots to display within the Athlete EST Monitor or Spectator EST Display.
		/// <para>Does follow the <a href="https://support.scopos.tech/index.html?segment-and-command-value-inhe.html">value inheritance rules.</a></para>
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
		/// <para>Does follow the <a href="https://support.scopos.tech/index.html?segment-and-command-value-inhe.html">value inheritance rules.</a></para>
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
		/// <para>Does follow the <a href="https://support.scopos.tech/index.html?segment-and-command-value-inhe.html">value inheritance rules.</a></para>
		/// </summary>
		[JsonPropertyOrder( 12 )]
        [G_NS.JsonProperty( Order = 12 )]
        [DefaultValue( -9999 )]
        public int StringSize { get; set; } = SegmentGroup.DEFAULT_INT;

        public int GetStringSize() {
            if (StringSize != SegmentGroup.DEFAULT_INT)
                return StringSize;

            if (Parent.StringSize != SegmentGroup.DEFAULT_INT)
                return Parent.StringSize;

            return Parent.Parent.StringSize;
        }

		/// <summary>
		/// When set, advances the tape feed on the target this number of millimeters at the start of the Segment.
		/// <para>Does not follow the <a href="https://support.scopos.tech/index.html?segment-and-command-value-inhe.html">value inheritance rules.</a></para>
		/// </summary>
		[JsonPropertyOrder( 15 )]
        [G_NS.JsonProperty( Order = 15 )]
        [DefaultValue( 0 )]
        public int TapeAdvance { get; set; } = SegmentGroup.DEFAULT_INT;


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