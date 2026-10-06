using System.ComponentModel;
using System.Runtime.Serialization;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.Specification.Definitions;

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
    public class StageStyle : Definition, IGetScoreFormatCollectionDefinition {
        /// <summary>
        /// Public constructor
        /// </summary>
        public StageStyle() : base() {
            Type = DefinitionType.STAGESTYLE;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod( StreamingContext context ) {
            base.OnDeserializedMethod( context );
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
        public SetName ScoreFormatCollectionDef { get; set; } = Definitions.SetName.Parse( "v1.0:orion:Standard Score Formats" );

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
        /// <exception cref="XApiKeyNotSetException" />
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public async Task<ScoreFormatCollection> GetScoreFormatCollectionDefinitionAsync() {

            return await DefinitionCache.GetScoreFormatCollectionDefinitionAsync( ScoreFormatCollectionDef );
        }

        /// <summary>
        /// A relative value to 1.00, measuring the degree of difficulty of this stage style. The higher the value the more difficult the stage style.
        /// <para>Value is used to help project an athlete's score in multi-stage events. </para>
        /// <para>Value muse be between 1.10 and 0.500. 0.900 is the default.</para>
        /// <para>The exact value should be experimentally determined. Taking the 90th percentile of scores as the basis. </para>
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 15 )]
        [G_NS.JsonProperty( Order = 15 )]
        public float RelativeDifficulty { get; set; } = .97f;

        /*
         * Use the following SQL to determine RelativeDifficulty values, using the 85th percentile as the basis (so that the value is based on high level of performance, but not outliers). 
         * 
            with percentiles as (
            select ( score_decimal / shot_count ) as myAverage,
            ntile(100) over (order by ( score_decimal / shot_count )) as percentile_rank
            from score_history_stage
            where stage_style_def = "v1.0:nra:BB Gun Sitting" 
              and shot_count >= 10 and score_decimal > 1 and `date` >= DATE_SUB(CURDATE(), INTERVAL 5 YEAR)
            )
            select avg( myAverage ) as myValue
            from percentiles
            where percentile_rank = 85 or percentile_rank = 84
         */

        /// <summary>
        /// A list of common ways to display scores for this Stage Style. The first item in the list is considered the default.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 90 )]
        [G_NS.JsonProperty( Order = 90 )]
        [Obsolete( "Use ScoreFormatCollectionDef and ScoreConfigDefault" )]
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
