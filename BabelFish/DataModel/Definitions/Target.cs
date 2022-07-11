using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShootersTech.DataModel.Definitions {
    [Serializable]
    public class Target : Definition {

        public Target() : base() {
            Type = DefinitionType.TARGET;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);
        }

        [JsonProperty(Order = 10)]
        public List<ScoringRing> ScoringRings { get; set; } = new List<ScoringRing>();

        [JsonProperty(Order = 11)]
        public ScoringRing InnerTen { get; set; } = new ScoringRing();

        [JsonProperty(Order = 12)]
        public List<AimingMark> AimingMarks { get; set; } = new List<AimingMark>();

        [JsonProperty(Order = 13)]
        public string BackgroundColor { get; set; } = AimingMark.COLOR_WHITE;

        /// <summary>
        /// The expected distance that this target is shot at. Measured in mm.
        /// </summary>
        [JsonProperty( Order = 14 )]
        [DefaultValue( 10000 )]
        public int Distance { get; set; } = 10000;
    }
}
