using Newtonsoft.Json;
using Scopos.BabelFish.APIClients;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// A STAGE SYTLE defines a portion of an EVENT STYLE.
    /// </summary>
    /// <remarks>
    /// A STAGE STYLE will be:
    /// <list type="bullet">
    /// <item>Shot continguously</item>
    /// <item>Have the same equipment, time limits, distance of fire, and shooting position.</item>
    /// </list>
    /// </remarks>
    [Serializable]
    public class StageStyle : Definition, ICopy<StageStyle>, IGetScoreFormatCollectionDefinition
    {
        /// <summary>
        /// Public constructor
        /// </summary>
        public StageStyle() : base() {
            Type = DefinitionType.STAGESTYLE;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);
        }

        /// <inheritdoc />
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

        /// <summary>
        /// The number of shots that make up a Series, for this Stage Style
        /// </summary>
        [JsonProperty( Order = 11 ) ]
        public int ShotsInSeries { get; set; } = 10;

        /// <summary>
        /// The SetName of the SCORE FORMAT COLLECTION definition to use when displaying scores with this STAGE STYLE
        /// </summary>
        [DefaultValue( "v1.0:orion:Standard Score Formats" )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.Include, Order = 12 )]
        public string ScoreFormatCollectionDef { get; set; } = "v1.0:orion:Standard Score Formats";

        /// <summary>
        /// The default ScoreConfigName to use, that is defined by the .ScoreFormatCollectionDef, to use when displaying scores with this STAGE STYLE
        /// </summary>
        [DefaultValue( "Integer" )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.Include, Order = 13 )]
        public string ScoreConfigDefault { get; set; } = "Integer";

        /// <summary>
        /// A list (order is inconsequential) of other STAGE STYLEs that are similar to this STAGE STYLE.
        /// </summary>
        [JsonProperty( Order = 14 )]
        public List<string> RelatedStageStyles { get; set; } = new List<string>();

        /// <inheritdoc />
        public async Task<ScoreFormatCollection> GetScoreFormatCollectionDefinitionAsync() {

            SetName scoreFormatCollectionSetName = Scopos.BabelFish.DataModel.Definitions.SetName.Parse( ScoreFormatCollectionDef );
            return await DefinitionCache.GetScoreFormatCollectionDefinitionAsync( scoreFormatCollectionSetName );
        }

        [Obsolete( "Use ScoreFormatCollectionDef and ScoreConfigDefault")]
        [JsonProperty( Order = 90 )]
        public List<DisplayScoreFormat> DisplayScoreFormats { get; set; } = new List<DisplayScoreFormat>();

    }
}
