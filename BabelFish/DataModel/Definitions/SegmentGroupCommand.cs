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
    /// A SegmentGroupCommand object (sometimes simply called a Command) specifies the state of the EST Targets and Monitors for the current command. It also lists the text of the range officer's commands and notes for the ROs
    /// A SegmentGroupCommand is a sub-object of RangeScripts, and SegmentGroup.In SegmentGroup a SegmentGroupCommand object is in the field DefaultCommand, and the list Commands.
    /// In general, if a field is not listed in the Commands list, then it is pulled from SegmentGroup.DefaultCommand.If it is not listed there, then it is pulled from RangeScripts.DefaultCommand.See each field for specifics.
    /// </summary>
    public class SegmentGroupCommand : IReconfigurableRulebookObject, ICopy<SegmentGroupCommand> {

        private const int DEFAULT_INT = -9999;
        private const string DEFAULT_STR = "";

        /// <summary>
        /// Public interface
        /// </summary>
        public SegmentGroupCommand() {

        }

        /// <inheritdoc />
        public SegmentGroupCommand Copy() {
            SegmentGroupCommand copy = new SegmentGroupCommand();
            copy.Command = this.Command;
            copy.Notes = this.Notes;
            copy.Fade = this.Fade;
            copy.Timer = this.Timer;
            copy.TimerCommand = this.TimerCommand;
            copy.OccursAt = this.OccursAt;
            copy.GreenLight = this.GreenLight;
            copy.RedLight = this.RedLight;
            copy.TargetLight = this.TargetLight;
            copy.DisplayEvent = this.DisplayEvent;
            copy.Continue = this.Continue;
            copy.NextCommandIndex = this.NextCommandIndex;
            copy.Comment = this.Comment;

            if (this.ShotAttributes != null) {
                foreach( var sa in this.ShotAttributes) {
                    copy.ShotAttributes.Add( sa );
                }
            }

            return copy;
        }

        [JsonIgnore]
        protected internal SegmentGroupCommand Parent { get; set; }

        /// <summary>
        /// Commands: Not required, defaults to empty string.
        /// DefaultCommand: Ignored
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(Order = 1)]
        public string Command { get; set; } = DEFAULT_STR;

        public string GetCommand() {
            return Command;
        }

        /// <summary>
        /// How long to display the text of the Command in the athlete monitor or spectator display units. Measured in seconds. A value of 0 means don't display the Command . Value of -1 means do not ever remove the Command.
        /// Commands: Not required, missing or value of -9999 uses DefaultCommand.Fade
        /// DefaultCommand: Required, defaults to 60
        /// </summary>
        [DefaultValue(-9999)]
        [JsonProperty(Order = 3)]
        public int Fade { get; set; } = DEFAULT_INT;

        public int GetFade() {
            if (Fade != DEFAULT_INT)
                return Fade;

            if (Parent.Fade != DEFAULT_INT)
                return Parent.Fade;

            return Parent.Parent.Fade;
        }

        /// <summary>
        /// Commands: Not required, defaults to ""
        /// DefaultCommand: Ignored
        /// </summary>
        [DefaultValue( "" )]
        [JsonProperty( Order = 2 )]
        public string Notes { get; set; } = DEFAULT_STR;

        public string GetNotes() {
            return Notes;
        }

        /// <summary>
        /// Commands: Not required, missing or value of "" does not change the RangeClock
        /// DefaultCommand: Ignored
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(Order = 5)]
        public string Timer { get; set; } = DEFAULT_STR;

        public string GetTimer() {
            return Timer;
        }

        /// <summary>
        /// Commands: Not required, missing or value of NONE does not effect the RangeClock
        /// DefaultCommand: Ignored
        /// </summary>
        [JsonConverter( typeof( StringEnumConverter ) )]
        [DefaultValue( TimerCommandOptions.NONE )]
        [JsonProperty( Order = 6 )]
        public TimerCommandOptions TimerCommand { get; set; } = TimerCommandOptions.NONE;

        public TimerCommandOptions GetTimerCommand() {
            return TimerCommand;
        }

        /// <summary>
        /// Commands: Not required, missing or value of "" does not implement automation
        /// DefaultCommand: Ignored
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(Order = 4)]
        public string OccursAt { get; set; } = DEFAULT_STR;

        public string GetOccursAt() {
            return OccursAt;
        }

        /// <summary>
        /// Commands: Not required, missing or value of -9999 uses DefaultCommand.GreenLight
        /// DefaultCommand: Required with default value 0
        /// </summary>
        [JsonConverter( typeof( StringEnumConverter ) )]
        [DefaultValue( LightIllumination.NONE )]
        [JsonProperty( Order = 8 )]
        public LightIllumination GreenLight { get; set; } = LightIllumination.NONE;

        public LightIllumination GetGreenLight() {
            if (GreenLight != LightIllumination.NONE)
                return GreenLight;

            if (Parent.GreenLight != LightIllumination.NONE)
                return Parent.GreenLight;

            return Parent.Parent.GreenLight;
        }

        /// <summary>
        /// Commands: Not required, missing or value of -9999 uses DefaultCommand.RedLight
        /// DefaultCommand: Required with default value 0
        /// </summary>
        [JsonConverter( typeof( StringEnumConverter ) )]
        [DefaultValue( LightIllumination.NONE )]
        [JsonProperty( Order = 9 )]
        public LightIllumination RedLight { get; set; } = LightIllumination.NONE;

        public LightIllumination GetRedLight() {
            if (RedLight != LightIllumination.NONE)
                return RedLight;

            if (Parent.RedLight != LightIllumination.NONE)
                return Parent.RedLight;

            return Parent.Parent.RedLight;
        }

        /// <summary>
        /// Commands: Not required, missing or value of -9999 uses DefaultCommand.TargetLight
        /// DefaultCommand: Required with default value 0
        /// </summary>
        [JsonConverter( typeof( StringEnumConverter ) )]
        [DefaultValue( LightIllumination.NONE )]
        [JsonProperty( Order = 7 )]
        public LightIllumination TargetLight { get; set; } = LightIllumination.NONE;

        public LightIllumination GetTargetLight() {
            if (TargetLight != LightIllumination.NONE)
                return TargetLight;

            if (Parent.TargetLight != LightIllumination.NONE)
                return Parent.TargetLight;

            return Parent.Parent.TargetLight;
        }

        /// <summary>
        /// A list of ShotAttributes that should decorate a Shot if fired during this SegmentGroupCommand.
        /// Must be one of the following
        /// FIRED BEFORE COMMAND START
        /// FIRED AFTER COMMAND STOP
        /// Commands: Not required, missing or null uses DefaultCommand.ShotAttributes
        /// DefaultCommand: Required, may be empty list []
        /// </summary>
        [DefaultValue( null )]
        [JsonProperty( Order = 10 )]
        public List<string> ShotAttributes { get; set; } = new List<string>();

        public List<string> GetShotAttributes() {
            if (ShotAttributes != null)
                return ShotAttributes;

            if (Parent.ShotAttributes != null)
                return Parent.ShotAttributes;

            return Parent.Parent.ShotAttributes;
        }

        [JsonProperty( Order = 11 )]
        [JsonConverter( typeof( StringEnumConverter ) )]
        [DefaultValue( DisplayEventOptions.NONE )]
        public DisplayEventOptions DisplayEvent { get; set; } = DisplayEventOptions.NONE;

        public DisplayEventOptions GetDisplayEvent() {
            if (DisplayEvent != DisplayEventOptions.NONE)
                return DisplayEvent;

            if (Parent.DisplayEvent != DisplayEventOptions.NONE)
                return Parent.DisplayEvent;

            if (Parent.Parent.DisplayEvent != DisplayEventOptions.NONE)
                return Parent.Parent.DisplayEvent;

            return DisplayEventOptions.Default;
        }

        [DefaultValue( -1 )]
        [JsonProperty( Order = 12 )]
        public int Continue { get; set; } = DEFAULT_INT;

        public int GetContinue() {
            if (Continue >= 0)
                return Continue;

            if (Parent.Continue >= 0)
                return Parent.Continue;

            if (Parent.Parent.Continue >= 0)
                return Parent.Parent.Continue;

            return -1;
        }

        [DefaultValue(-1)]
        [JsonProperty(Order = 13)]
        public int NextCommandIndex { get; set; } = DEFAULT_INT;

        /// <inheritdoc/>
        [JsonProperty( Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        /// <inheritdoc/>
        public override string ToString() {
            var c = GetCommand();
            if (c != "")
                return c;
            else
                return "{no command}";
        }

        public static IEnumerable<DisplayEventOptions> GetDisplayEventOptionsAsList() {
            
            return Enum.GetValues(typeof(DisplayEventOptions)).Cast<DisplayEventOptions>();
        }
    }
}
