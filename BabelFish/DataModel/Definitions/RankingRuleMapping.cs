using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.DataModel.Definitions {



    /// <summary>
    /// Key is the ScoreConfigName, Value is a strintg, in the form of a SetName that is the Ranking Rule Definition to use with that Score Config
    /// </summary>
    public class RankingRuleMapping : Dictionary<string, string>, IGetRankingRuleDefinitionList {

        public RankingRuleMapping() {
            this[DEFAULTDEF] = DEFAULT_RANKING_RULE_DEF;
        }

        public RankingRuleMapping( string rankingRuleDef ) {
            this[DEFAULTDEF] = rankingRuleDef;
        }

        /// <summary>
        /// This is the default ScoreConfig name. When no other ScoreConfig names match, this value may point to the default RankingRuleDefinition to use instead.
        /// </summary>
        public const string DEFAULTDEF = "DefaultDef";

        public const string DEFAULT_RANKING_RULE_DEF = "v1.0:orion:Alphabetical Participant Sort";

        /// <inheritdoc />
        /// <exception cref="XApiKeyNotSetException" />
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public async Task<Dictionary<string, RankingRule>> GetRankingRuleDefinitionListAsync() {

            Dictionary<string, RankingRule> rankingRules = new Dictionary<string, RankingRule>();

            foreach (var rankingRuleDef in this.Values) {
                var sn = SetName.Parse( rankingRuleDef );
                var rankingRule = await DefinitionCache.GetRankingRuleDefinitionAsync( sn );
                rankingRules.Add( rankingRuleDef, rankingRule );
            }

            return rankingRules;
        }
    }
}
