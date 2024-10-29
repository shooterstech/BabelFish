using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Definitions {
    [Serializable]
    public class StageStyle : Definition, ICopy<StageStyle>
    {

        public StageStyle() : base() {
            Type = DefinitionType.STAGESTYLE;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);
        }

        public StageStyle Copy()
        {
            StageStyle stageStyle = new StageStyle();
            this.Copy(stageStyle);
            if (this.RelatedStageStyles != null)
            {
                foreach (var rss in this.RelatedStageStyles)
                {
                    stageStyle.RelatedStageStyles.Add(rss);
                }
            }

            stageStyle.ShotsInSeries = this.ShotsInSeries;

            if (this.DisplayScoreFormats != null)
            {
                foreach (var dsf in this.DisplayScoreFormats)
                {
                    stageStyle.DisplayScoreFormats.Add(dsf.Copy());
                }
            }

            return stageStyle;
        }

        public List<string> RelatedStageStyles { get; set; } = new List<string>();

        public int ShotsInSeries { get; set; } = 0;

        public List<DisplayScoreFormat> DisplayScoreFormats { get; set; } = new List<DisplayScoreFormat>();

    }
}
