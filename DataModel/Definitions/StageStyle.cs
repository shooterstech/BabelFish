using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.Definitions {
    [Serializable]
    public class StageStyle : Definition {


        public StageStyle() : base() {
            Type = Definition.DefinitionType.STAGESTYLE;
        }


        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);
        }

        public List<string> RelatedStageStyles { get; set; }

        public int ShotsInSeries { get; set; }

        public List<DisplayScoreFormat> DisplayScoreFormats { get; set; }

    }
}
