using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataActors.Tournaments {

    public class ReentryMethodConfiguration : MergeConfiguration, IGetCourseOfFireDefinition, IGetRankingRuleDefinition, IGetResultListFormatDefinition {

        public ReentryMethodConfiguration() : base() {
            /*
             * NOTE: MergeConfiguration classes use the same concrete class identifier
             * as the cooresponding MergeMethod classes.
             */

            this.Method = "Reentry";
        }

        /// <summary>
        /// The level, within the COURSE OF FIRE Event Tree to perform the reentry score selection.
        /// </summary>
        public EventtType EventType { get; set; } = EventtType.EVENT;

        /// <summary>
        /// The SetName of the COURSE OF FIRE definition that all result lists are 
        /// expected to be composed from.
        /// </summary>
        //[G_STJ_SER.JsonConverter(typeof(G_BF_STJ_CONV.SetNameConverter))]
        //[G_NS.JsonConverter( typeof( G_BF_NS_CONV.SetNameConverter ))]
        public SetName CourseOfFireDef {  get; set; }

        /// <summary>
        /// The SetName of the RANKING RULE definition that should be used to rank the merged results.
        /// </summary>
        //[G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.SetNameConverter ) )]
        //[G_NS.JsonConverter( typeof( G_BF_NS_CONV.SetNameConverter ) )]
        public SetName RankingRuleDef { get; set; }

        /// <summary>
        /// The SetName of the RESULT LIST FORMAT definition that should be used to format the merged results.
        /// </summary>
        //[G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.SetNameConverter ) )]
        //[G_NS.JsonConverter( typeof( G_BF_NS_CONV.SetNameConverter ) )]
        public SetName ResultListFormatDef { get; set; }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown if the value for CourseOfFireDef 
        /// is empty, known to happen with older versions of Orion. </exception>
        /// <exception cref="XApiKeyNotSetException" ></exception>
        /// <exception cref="DefinitionNotFoundException" ></exception>
        /// <exception cref="ScoposAPIException" ></exception>
        public async Task<CourseOfFire> GetCourseOfFireDefinitionAsync() {

            if ( CourseOfFireDef is null)
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
