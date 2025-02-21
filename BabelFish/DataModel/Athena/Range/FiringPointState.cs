using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.Athena.Range
{
    public class FiringPointState
    {

        FiringPointState()
        {
            Targets = new Dictionary<string, TargetState>();
            Monitors = new Dictionary<string, MonitorState>();
        }

        [JsonPropertyOrder(1)]
        [JsonIgnore]
        public string FiringPointNumber { get; set; }

        /// <summary>
        /// Enabled means the firing point is operational and ready for use.
        /// A broken target would be marked with Enabled = false, for example.
        /// </summary>
        [JsonPropertyOrder( 2 )]
        public bool Enabled { get; set; }

        /// <summary>
        /// Competition means the Firing point is current being used in a competition managed by Orion.
        /// When the target is in Practice Mode, Competition would be set to false.
        /// </summary>
        [JsonPropertyOrder( 3 )]
        public bool Competition { get; set; } = true;

        /// <summary>
        /// Key is the Target's State Address (IoT Thing Name), Value is the TargetState
        /// </summary>
        [JsonPropertyOrder( 4 )]
        public Dictionary<string, TargetState> Targets { get; set; }

        /// <summary>
        /// Key is the Monitor State Address (IoT Thing Name), Value is the MonitorState
        /// </summary>
        [JsonPropertyOrder( 5 )]
        public Dictionary<string, MonitorState> Monitors { get; set; }
    }

}