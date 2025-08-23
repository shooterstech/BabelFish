using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.Athena.Range {
    public class FiringPointState {

        public FiringPointState() {
            Targets = new Dictionary<string, TargetState>();
            Monitors = new Dictionary<string, MonitorState>();
        }

        [JsonIgnore]
        public string FiringPointNumber { get; set; }

        /// <summary>
        /// Enabled means the firing point is operational and ready for use.
        /// A broken target would be marked with Enabled = false, for example.
        /// </summary>
        [JsonProperty(Order =1)]
        public bool Enabled { get; set; }

        /// <summary>
        /// FiringState is once the target is enabled, a further state is chosen.
        /// IE:
        /// FiringState.Competition
        ///     ||     .HospitalPoint
        ///     ||     .Practice
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum FiringState {
            /// <summmary>Target is in competition mode and occupied with an athlete</summmary>
            COMPETITION,
            /// <summary>Target is in competition mode and is a Hospital Point (without an athlete)</summary>
            HOSPITAL_POINT,
            /// <summary>Target is in Practice mode</summary>
            PRACTICE 
                // I think we may want a 4th state things can be in, override state, where things can be set, apart from the main comp states
        };

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(Order = 2)]
        [JsonConverter( typeof( StringEnumConverter ) )]
        public FiringState State { get; set; } = FiringState.COMPETITION;

        /// <summary>
        /// Key is the Target's State Address (IoT Thing Name), Value is the TargetState
        /// </summary>
        [JsonProperty(Order = 3)]
        public Dictionary<string, TargetState> Targets { get; set; }

        /// <summary>
        /// Key is the Monitor State Address (IoT Thing Name), Value is the MonitorState
        /// </summary>
        [JsonProperty(Order = 4)]
        public Dictionary<string, MonitorState> Monitors { get; set; }

        /// <summary>
        /// OverrideEnable will allow the RO to know if the firing point is overriding the current target height 
        /// </summary>
        [JsonProperty(Order = 5)]
        public bool OverrideEnable { get; set; }
    }

}
