using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BabelFish.DataModel.Definitions {
    public class SegmentGroupCommand {

        private const int DEFAULT_INT = -9999;
        private const string DEFAULT_STR = "";

        [JsonConverter(typeof(StringEnumConverter))]
        public enum TimerCommandOptions { NONE, START, PAUSE, RESUME, STOP, CLOCK };

        [JsonConverter(typeof(StringEnumConverter))]
        public enum LightIllumination { NONE, ON, OFF, DIM };


        [JsonConverter(typeof(StringEnumConverter))]
        public enum DisplayEventOptions {
            NONE, Default, QualificationPreEvent, QualificationPostEvent, QualificationCallToLine, QualificationRemoveEquipment,
            QualificationStart, QualificationStop, QualificationPreparationPeriodStart, QualificationPreparationPeriodStop, QualificationSightersStart, QualificationSightersStop,
            QualificationStageStart, QualificationStageStop, QualificationTargetChangeStart, QualificationTargetChangeStop, QualificationChangeOverStart, QualificationChangeOverStop,
            QualificationExtraTimeStart, QualificationExtraTimeStop, QualificationUnscheduledCeaseFire, QualificationBoatInImpactArea, QualificationAlibiStart, QualificationAlibiStop,
            FinalPreEvent, FinalPostEvent, FinalCallToLine, FinalRemoveEquipment, FinalCommentary, FinalStart, FinalStop, FinalPreparationPeriodStart, FinalPreparationPeriodStop, FinalSightersStart, FinalSightersStop,
            FinalAthleteIntroductionStart, FinalAthleteIntroductionStop, FinalStageStart, FinalStageStop, FinalEliminationStageStart, FinalEliminationStageStop,
            FinalChangeOverStart, FinalChangeOverStop, FinalAthleteEliminated, FinalThirdPlaceAnnounced, FinalSecondPlaceAnnounced, FinalFirstPlaceAnnounced,
            SpecialEventOne, SpecialEventTwo, SpecialEventThree, SpecialEventFour, SafetyBriefing
        }

        private List<string> validationErrorList = new List<string>();

        public SegmentGroupCommand() {

        }

        [JsonIgnore]
        protected internal SegmentGroupCommand Parent { get; set; }

        /// <summary>
        /// Commands: Not required, defaults to empty string.
        /// DefaultCommand: Ignored
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(Order = 1)]
        public string Command { get; set; }

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
        public int Fade { get; set; }

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
        [DefaultValue("")]
        [JsonProperty(Order = 2)]
        public string Notes { get; set; }

        public string GetNotes() {
            return Notes;
        }

        /// <summary>
        /// Commands: Not required, missing or value of "" does not change the RangeClock
        /// DefaultCommand: Ignored
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(Order = 5)]
        public string Timer { get; set; }

        public string GetTimer() {
            return Timer;
        }

        /// <summary>
        /// Commands: Not required, missing or value of NONE does not effect the RangeClock
        /// DefaultCommand: Ignored
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [DefaultValue(TimerCommandOptions.NONE)]
        [JsonProperty(Order = 6)]
        public TimerCommandOptions TimerCommand { get; set; }

        public TimerCommandOptions GetTimerCommand() {
            return TimerCommand;
        }

        /// <summary>
        /// Commands: Not required, missing or value of "" does not implement automation
        /// DefaultCommand: Ignored
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(Order = 4)]
        public string OccursAt { get; set; }

        public string GetOccursAt() {
            return OccursAt;
        }

        /// <summary>
        /// Commands: Not required, missing or value of -9999 uses DefaultCommand.GreenLight
        /// DefaultCommand: Required with default value 0
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        [DefaultValue(LightIllumination.NONE)]
        [JsonProperty(Order = 8)]
        public LightIllumination GreenLight { get; set; }

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
        [JsonConverter(typeof(StringEnumConverter))]
        [DefaultValue(LightIllumination.NONE)]
        [JsonProperty(Order = 9)]
        public LightIllumination RedLight { get; set; }

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
        [JsonConverter(typeof(StringEnumConverter))]
        [DefaultValue(LightIllumination.NONE)]
        [JsonProperty(Order = 7)]
        public LightIllumination TargetLight { get; set; }

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
        [DefaultValue(null)]
        [JsonProperty(Order = 10)]
        public List<string> ShotAttributes { get; set; }

        public List<string> GetShotAttributes() {
            if (ShotAttributes != null)
                return ShotAttributes;

            if (Parent.ShotAttributes != null)
                return Parent.ShotAttributes;

            return Parent.Parent.ShotAttributes;
        }

        [JsonProperty(Order = 11)]
        [JsonConverter(typeof(StringEnumConverter))]
        [DefaultValue(DisplayEventOptions.NONE)]
        public DisplayEventOptions DisplayEvent { get; set; }

        public DisplayEventOptions GetDisplayEvent() {
            if (DisplayEvent != DisplayEventOptions.NONE)
                return DisplayEvent;

            if (Parent.DisplayEvent != DisplayEventOptions.NONE)
                return Parent.DisplayEvent;

            if (Parent.Parent.DisplayEvent != DisplayEventOptions.NONE)
                return Parent.Parent.DisplayEvent;

            return DisplayEventOptions.Default;
        }

        [DefaultValue(-1)]
        [JsonProperty(Order = 12)]
        public int Continue { get; set; }

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
        public int NextCommandIndex { get; set; }

        /// <summary>
        /// Authors internal comments for documentation. Value does not effect any functionality.
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(Order = 100)]
        public string Comment { get; set; }

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
