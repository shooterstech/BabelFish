using System.Text.Json;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.Specification;
using Scopos.BabelFish.DataActors.Specification.Definitions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

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
    public class StageStyle : Definition, IGetScoreFormatCollectionDefinition
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

        /// <summary>
        /// The number of shots that make up a Series, for this Stage Style
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 11 )]
        [G_NS.JsonProperty( Order = 11, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( 10 )]
        public int ShotsInSeries { get; set; } = 10;

        /// <summary>
        /// The SetName of the SCORE FORMAT COLLECTION definition to use when displaying scores with this STAGE STYLE
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 12 )]
        [G_NS.JsonProperty( Order = 12, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( "v1.0:orion:Standard Score Formats" )]
        public string ScoreFormatCollectionDef { get; set; } = "v1.0:orion:Standard Score Formats";

        /// <summary>
        /// The default ScoreConfigName to use, that is defined by the .ScoreFormatCollectionDef, to use when displaying scores with this STAGE STYLE
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 13 )]
        [G_NS.JsonProperty( Order = 13, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( "Integer" )]
        public string ScoreConfigDefault { get; set; } = "Integer";

        /// <summary>
        /// A list (order is inconsequential) of other STAGE STYLEs that are similar to this STAGE STYLE.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 14 )]
        [G_NS.JsonProperty( Order = 14 )]
        public List<string> RelatedStageStyles { get; set; } = new List<string>();

        /// <inheritdoc />
        public async Task<ScoreFormatCollection> GetScoreFormatCollectionDefinitionAsync() {

            SetName scoreFormatCollectionSetName = Scopos.BabelFish.DataModel.Definitions.SetName.Parse( ScoreFormatCollectionDef );
            return await DefinitionCache.GetScoreFormatCollectionDefinitionAsync( scoreFormatCollectionSetName );
        }

        [G_STJ_SER.JsonPropertyOrder( 90 )]
        [G_NS.JsonProperty( Order = 90 )]
        [Obsolete( "Use ScoreFormatCollectionDef and ScoreConfigDefault")]
        public List<DisplayScoreFormat> DisplayScoreFormats { get; set; } = new List<DisplayScoreFormat>();


        /// <summary>
        /// A Newtonsoft Conditional Property to only serialize DisplayScoreFormats when the list has something in it.
        /// https://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeDisplayScoreFormats() {
            return (DisplayScoreFormats != null && DisplayScoreFormats.Count > 0);
        }

        /// <inheritdoc />
		public override async Task<bool> GetMeetsSpecificationAsync() {
                var validation = new IsStageStyleValid();

                var meetsSpecification = await validation.IsSatisfiedByAsync( this );
                SpecificationMessages = validation.Messages;
            
            return meetsSpecification;
        }
	}
}
