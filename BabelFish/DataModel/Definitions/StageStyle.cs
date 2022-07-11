using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel.Definitions {
    [Serializable]
    public class StageStyle : Definition {

        public StageStyle() : base() {
            Type = DefinitionType.STAGESTYLE;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);
        }

        public List<string> RelatedStageStyles { get; set; } = new List<string>();

        public int ShotsInSeries { get; set; } = 0;

        public List<DisplayScoreFormat> DisplayScoreFormats { get; set; } = new List<DisplayScoreFormat>();

    }
}
