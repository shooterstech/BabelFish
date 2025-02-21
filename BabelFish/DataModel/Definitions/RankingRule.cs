using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataActors.Specification.Definitions;

namespace Scopos.BabelFish.DataModel.Definitions {
    [Serializable]
    public class RankingRule : Definition    {

        public RankingRule() : base() {
            Type = DefinitionType.RANKINGRULES;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);
        }

        [G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        [DefaultValue(null)]
        public List<RankingDirective> RankingRules { get; set; } = new List<RankingDirective>();

        /// <summary>
        /// Generates a default ranking rule definition based on the passed in Event Name and ScoreConfigName
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="scoreConfigName"></param>
        /// <returns></returns>
        public static RankingRule GetDefault( string eventName, string scoreConfigName ) {

            var rankingRule = new RankingRule();
            rankingRule.RankingRules.Add( RankingDirective.GetDefault( eventName, scoreConfigName ) );

            return rankingRule;
        }

		/// <inheritdoc />
		public override async Task<bool> GetMeetsSpecificationAsync() {
            var validation = new IsRankingRulesValid();

			var meetsSpecification = await validation.IsSatisfiedByAsync( this );
			SpecificationMessages = validation.Messages;

			return meetsSpecification;
		}
	}
}
