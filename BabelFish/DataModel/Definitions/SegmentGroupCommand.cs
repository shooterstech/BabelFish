using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// A SegmentGroupCommand object (sometimes simply called a Command) specifies the state of the EST Targets and Monitors for the current command. It also lists the text of the range officer's commands and notes for the ROs
    /// A SegmentGroupCommand is a sub-object of RangeScripts, and SegmentGroup.In SegmentGroup a SegmentGroupCommand object is in the field DefaultCommand, and the list Commands.
    /// In general, if a field is not listed in the Commands list, then it is pulled from SegmentGroup.DefaultCommand.If it is not listed there, then it is pulled from RangeScripts.DefaultCommand.See each field for specifics.
    /// </summary>
    public class SegmentGroupCommand : IReconfigurableRulebookObject {

        private const int DEFAULT_INT = -9999;
        private const string DEFAULT_STR = "";

        /// <summary>
        /// Public interface
        /// </summary>
        public SegmentGroupCommand() {

        }

        [JsonIgnore]
        protected internal SegmentGroupCommand Parent { get; set; }

        /// <summary>
        /// Commands: Not required, defaults to empty string.
        /// DefaultCommand: Ignored
        /// </summary>
        [DefaultValue("")]
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
        public string Notes { get; set; } = DEFAULT_STR;

        public string GetNotes() {
            return Notes;
        }

        /// <summary>
        /// Commands: Not required, missing or value of "" does not change the RangeClock
        /// DefaultCommand: Ignored
        /// </summary>
        [DefaultValue("")]
        [Obsolete("Will be replaced with RangeTimer")]
        public string Timer { get; set; } = DEFAULT_STR;

        public string GetTimer() {
            return Timer;
        }

        /// <summary>
        /// Represents the same value as .Timer, but as a float.
        /// </summary>
        /// <remarks>Value is in seconds.</remarks>
        [JsonInclude]
        [DefaultValue( -9999 )]
        public float RangeTimer {
            get {
                if (string.IsNullOrEmpty( Timer ))
                    return DEFAULT_INT;

                TimeSpan timerAsTimeSpan;
                if (TimeSpan.TryParse( Timer, out timerAsTimeSpan )) {
                    return (float)timerAsTimeSpan.TotalSeconds;
                } else {
                    return DEFAULT_INT;
                }
            }
        }

        /// <summary>
        /// Commands: Not required, missing or value of NONE does not effect the RangeClock
        /// DefaultCommand: Ignored
        /// </summary>
        [DefaultValue( TimerCommandOptions.NONE )]
        public TimerCommandOptions TimerCommand { get; set; } = TimerCommandOptions.NONE;

        public TimerCommandOptions GetTimerCommand() {
            return TimerCommand;
        }

        /// <summary>
        /// Commands: Not required, missing or value of "" does not implement automation
        /// DefaultCommand: Ignored
        /// </summary>
        [DefaultValue("")]
        [Obsolete("Will be replaced with AtRangeTimerValue")]
        public string OccursAt { get; set; } = DEFAULT_STR;

        public string GetOccursAt() {
            return OccursAt;
        }

        /// <summary>
        /// Represents the same value as .OccuresAt, but as a float.
        /// </summary>
        /// <remarks>Value is in seconds.</remarks>
        [JsonInclude]
        [DefaultValue( -9999 )]
        public float AtRangeTimerValue {
            get {
                if (string.IsNullOrEmpty( OccursAt ))
                    return DEFAULT_INT;

                TimeSpan timerAsTimeSpan;
                if (TimeSpan.TryParse( OccursAt, out timerAsTimeSpan )) {
                    return (float)timerAsTimeSpan.TotalSeconds;
                } else {
                    return DEFAULT_INT;
                }
            }
        }

        /// <summary>
        /// Commands: Not required, missing or value of -9999 uses DefaultCommand.GreenLight
        /// DefaultCommand: Required with default value 0
        /// </summary>
        [DefaultValue( LightIllumination.NONE )]
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
        [DefaultValue( LightIllumination.NONE )]
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
        [DefaultValue( LightIllumination.NONE )]
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
        public List<string> ShotAttributes { get; set; } = new List<string>();

        public List<string> GetShotAttributes() {
            if (ShotAttributes != null)
                return ShotAttributes;

            if (Parent.ShotAttributes != null)
                return Parent.ShotAttributes;

            return Parent.Parent.ShotAttributes;
        }

        /// <summary>
        /// A Display Event is a transitional moment in a competition. When used, it keys an automated change to the Spectator Display.
        /// </summary>
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

        /// <summary>
        /// Number of seconds to wait before advancing to the next Command (or the Command indicated by NextCommandIndex). This field works independent of the Range Timer.
        /// <para>Only works if the Target and Monitor pair is in Practice Mode. Generally used to automate advancing Commands in practice that would otherwise be controlled by the Range Officer.</para>
        /// <list type="bullet">
        /// <item>Value of 0 means to advance to the next command immediately.</item>
        /// <item>Value of -1 means to not advance. This is also the default value.</item>
        /// </list>
        /// </summary>
        [DefaultValue( -1 )]
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

        /// <summary>
        /// A list of Command Automations, these are currently Remark additions or None.
        /// Commands: Not required, missing or null uses DefaultCommand.ShotAttributes
        /// DefaultCommand: Required, may be empty list []
        /// </summary>
        [G_NS.JsonProperty( Order = 200 )]
        public CommandAutomationList Automation { get; set; } = new CommandAutomationList();

        public bool ShouldSerializeCommandAutomation() {
            return Automation != null && Automation.Count > 0;
        }

        /// <summary>
        /// The index of the command, within the current SegmentGroup to advance to next using the Continue attribute. Useful if you want to go back a few commands to repeat a loop.
        /// <list type="bullet">
        /// <item>Value of -1 means to advance to the next command regardless of current index value.</item>
        /// <item>A value is not required, following the Value Inheritance Rules. Defaults to -1.</item>
        /// </list>
        /// </summary>
        [DefaultValue(-1)]
        public int NextCommandIndex { get; set; } = DEFAULT_INT;

        /// <inheritdoc/>
        [DefaultValue( "" )]
        [G_NS.JsonProperty( Order = 100 )]
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
