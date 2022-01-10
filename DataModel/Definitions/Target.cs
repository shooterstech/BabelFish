using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BabelFish.DataModel.Definitions {
    [Serializable]
    public class Target : Definition {

        public Target() : base() {
            Type = Definition.DefinitionType.TARGET;
            ScoringRings = new List<ScoringRing>();
            AimingMarks = new List<AimingMark>();
            BackgroundColor = AimingMark.COLOR_WHITE;
        }


        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);
        }


        [JsonProperty(Order = 10)]
        public List<ScoringRing> ScoringRings { get; set; }

        [JsonProperty(Order = 11)]
        public ScoringRing InnerTen { get; set; }

        [JsonProperty(Order = 12)]
        public List<AimingMark> AimingMarks { get; set; }

        [JsonProperty(Order = 13)]
        public string BackgroundColor { get; set; }
    }
}
