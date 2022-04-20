using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShootersTech.DataModel.Athena.AbstractEST;

namespace ShootersTech.DataModel.Athena.Range {
    public class FiringPointState {

        FiringPointState() {
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
        /// Competition means the Firing point is current being used in a competition managed by Orion.
        /// When the target is in Practice Mode, Competition would be set to false.
        /// </summary>
        [JsonProperty( Order = 2 )]
        public bool Competition { get; set; } = true;

        /// <summary>
        /// Key is the Target's State Address (IoT Thing Name), Value is the TargetState
        /// </summary>
        [JsonProperty(Order = 3)]
        public Dictionary<string, TargetState> Targets {get; set; }

        /// <summary>
        /// Key is the Monitor State Address (IoT Thing Name), Value is the MonitorState
        /// </summary>
        [JsonProperty(Order = 4)]
        public Dictionary<string, MonitorState> Monitors { get; set; }
    }

}
