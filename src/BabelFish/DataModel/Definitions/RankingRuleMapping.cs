using System.Diagnostics;
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
        /// Returns the SetName of the <see cref="RankingRule">RANKING RULE</see> definition that cooresponds to the past in Score Config Name.
        /// If the value of scoreConfigName is not defined in this RankingRuleMapping, then the
        /// default RANKING RULE is returned in its place which is defined in the <see cref="RankingRuleMapping"/> dictionary as "DefaultDef."
        /// If this value could not be found, then "v1.0:orion:Alphabetical Participant Sort" is returned.
        /// </summary>
        /// <param name="scoreConfigName"></param>
        /// <returns></returns>
        public SetName GetRankingRuleDef( string scoreConfigName ) {
            SetName rankingRuleDef;
            if (this.TryGetValue( scoreConfigName, out rankingRuleDef )) {
                return rankingRuleDef;
            }

            if (this.TryGetValue( DEFAULTDEF, out rankingRuleDef )) {
                return rankingRuleDef;
            }

            //In theory we should never get here, if we do, it means the definition didn't get constructed correctly.
            Debug.Fail( $"Could not identify the RANKING RULE SetName to return. Neither {scoreConfigName} was found in the dictionary, nor DefaultDef was found (which always should be here)." );
            return DEFAULT_RANKING_RULE_DEF;
        }
    }
}
