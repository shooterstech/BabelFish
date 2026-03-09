using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.DataModel.Definitions {



    /// <summary>
    /// Key is the ScoreConfigName, Value is the SetName that is the Ranking Rule Definition to use with that Score Config
    /// </summary>
    public class RankingRuleMapping : Dictionary<string, SetName>, IGetRankingRuleDefinitionList {

        /// <summary>
        /// Default constructor, that sets the DefaultDef to "v1.0:orion:Alphabetical Participant Sort"
        /// </summary>
        public RankingRuleMapping() {
            this[DEFAULTDEF] = DEFAULT_RANKING_RULE_DEF;
        }

        /// <summary>
        /// Constructor that sets the DefaultDef to the provided SetName. 
        /// </summary>
        /// <param name="rankingRuleDef"></param>
        public RankingRuleMapping( SetName rankingRuleDef ) {
            this[DEFAULTDEF] = rankingRuleDef;
        }

        /// <summary>
        /// This is the default ScoreConfig name. When no other ScoreConfig names match, this value may point to the default RankingRuleDefinition to use instead.
        /// </summary>
        public const string DEFAULTDEF = "DefaultDef";

        public readonly SetName DEFAULT_RANKING_RULE_DEF = SetName.Parse( "v1.0:orion:Alphabetical Participant Sort", false );

        /// <inheritdoc />
        /// <exception cref="XApiKeyNotSetException" />
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public async Task<Dictionary<SetName, RankingRule>> GetRankingRuleDefinitionListAsync() {

            Dictionary<SetName, RankingRule> rankingRules = new Dictionary<SetName, RankingRule>();

            foreach (var rankingRuleDef in this.Values) {
                var rankingRule = await DefinitionCache.GetRankingRuleDefinitionAsync( rankingRuleDef );
                rankingRules.Add( rankingRuleDef, rankingRule );
            }

            return rankingRules;
        }

        /// <summary>
        /// Returns the RANKING RULE definition that cooresponds to the past in Score Config Name.
        /// If the value of scoreConfigName is not defined in this RankingRuleMapping, then the
        /// default RANKING RULE is returned in its place.
        /// </summary>
        /// <param name="scoreConfigName"></param>
        /// <returns></returns>
        public SetName GetRankingRuleDef( string scoreConfigName ) {
            if (this.TryGetValue( scoreConfigName, out SetName rankingRuleDef )) {
                return rankingRuleDef;
            }

            return DEFAULT_RANKING_RULE_DEF;
        }
    }
}
