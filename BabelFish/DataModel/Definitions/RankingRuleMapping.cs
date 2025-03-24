using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BabelFish.DataModel.Definitions {



    /// <summary>
    /// Key is the ScoreConfigName, Value is a strintg, in the form of a SetName that is the Ranking Rule Definition to use with that Score Config
    /// </summary>
    public class RankingRuleMapping : Dictionary<string, string>, IGetRankingRuleDefinitionList {

        /// <summary>
        /// This is the default ScoreConfig name. When no other ScoreConfig names match, this value may point to the default RankingRuleDefinition to use instead.
        /// </summary>
        public const string DEFAULTDEF = "DefaultDef";

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
