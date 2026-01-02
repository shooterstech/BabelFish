using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {


    /// <summary>
    /// Classes that reference RankingRule Definitions should implement this interface.
    /// </summary>
    public interface IGetRankingRuleDefinition {

        /// <summary>
        /// Retreives the RankingRule Definition referenced by the instantiating class.
        /// </summary>
        /// <returns></returns>
		Task<RankingRule> GetRankingRuleDefinitionAsync();
    }


    /// <summary>
    /// Classes that reference RankingRule Definition Lists should implement this interface.
    /// </summary>
    public interface IGetRankingRuleDefinitionList {

        /// <summary>
        /// Retreives the RankingRule Definition referenced by the instantiating class.
        /// Key is the set name, value is the definition.
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<string, RankingRule>> GetRankingRuleDefinitionListAsync();
    }
}
