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
}
