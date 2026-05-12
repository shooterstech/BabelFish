using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.ResultListMerger {

    /// <summary>
    /// <see cref="MergeConfiguration"/> specific for a <see cref="MergedResultList"/> of type <see cref="MergeMethodType.REENTRY"/>.
    /// </summary>
    public class ReentryMethodConfiguration :
        MergeConfiguration,
        IGetCourseOfFireDefinition,
        IGetRankingRuleDefinition,
        IGetResultListFormatDefinition {

        /// <summary>
        /// Public constructor.
        /// <para>Unless you are a deserializer, rarely would you construct an instance of ReentryMethodConfiguration directly. Instead instances
        /// are creaed as part of the <see cref="MergedResultList.CreateAsync(IMergedResultListContainer, string, MergeMethodType)"/> method.</para>
        /// </summary>
        public ReentryMethodConfiguration() : base() {
            /*
             * NOTE: MergeConfiguration classes use the same concrete class identifier
             * as the cooresponding MergeMethod classes.
             */

            this.Method = DataModel.OrionMatch.MergeMethodType.REENTRY;
        }

        /// <summary>
        /// The level, within the COURSE OF FIRE Event Tree to perform the reentry score selection.
        /// </summary>
        [G_NS.JsonProperty( Order = 11, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public EventtType EventType { get; set; } = EventtType.EVENT;

        /// <summary>
        /// The SetName of the COURSE OF FIRE definition that all result lists are 
        /// expected to be composed from.
        /// </summary>
        [G_NS.JsonProperty( Order = 12 )]
        public SetName CourseOfFireDef { get; set; } = new SetName();

        /// <summary>
        /// The SetName of the RANKING RULE definition that should be used to rank the merged results.
        /// </summary>
        [G_NS.JsonProperty( Order = 13 )]
        public SetName RankingRuleDef { get; set; } = new SetName();

        /// <summary>
        /// The SetName of the RESULT LIST FORMAT definition that should be used to format the merged results.
        /// </summary>
        [G_NS.JsonProperty( Order = 14 )]
        public SetName ResultListFormatDef { get; set; } = new SetName();

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown if the value for CourseOfFireDef 
        /// is empty, known to happen with older versions of Orion. </exception>
        /// <exception cref="XApiKeyNotSetException" ></exception>
        /// <exception cref="DefinitionNotFoundException" ></exception>
        /// <exception cref="ScoposAPIException" ></exception>
        public async Task<CourseOfFire> GetCourseOfFireDefinitionAsync() {

            if (CourseOfFireDef is null)
                throw new ArgumentNullException( $"The value for CourseOfFireDef is null." );

            return await DefinitionCache.GetCourseOfFireDefinitionAsync( CourseOfFireDef );
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown if the value for CourseOfFireDef 
        /// is empty, known to happen with older versions of Orion. </exception>
        /// <exception cref="XApiKeyNotSetException" ></exception>
        /// <exception cref="DefinitionNotFoundException" ></exception>
        /// <exception cref="ScoposAPIException" ></exception>
        public async Task<RankingRule> GetRankingRuleDefinitionAsync() {

            if (this.RankingRuleDef is null)
                throw new ArgumentNullException( $"The value for RankingRuleDef is null." );

            return await DefinitionCache.GetRankingRuleDefinitionAsync( RankingRuleDef );
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown if the value for CourseOfFireDef 
        /// is empty, known to happen with older versions of Orion. </exception>
        /// <exception cref="XApiKeyNotSetException" ></exception>
        /// <exception cref="DefinitionNotFoundException" ></exception>
        /// <exception cref="ScoposAPIException" ></exception>
        public async Task<ResultListFormat> GetResultListFormatDefinitionAsync() {

            if (this.RankingRuleDef is null)
                throw new ArgumentNullException( $"The value for ResultListFormatDef is null." );

            return await DefinitionCache.GetResultListFormatDefinitionAsync( ResultListFormatDef );
        }
    }
}
