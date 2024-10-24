using Scopos.BabelFish.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BabelFish.DataModel.Definitions {



    /// <summary>
    /// Key is the ScoreConfigName, Value is a strintg, in the form of a SetName that is the Ranking Rule Definition to use with that Score Config
    /// </summary>
    public class RankingRuleMapping : Dictionary<string, string>, ICopy<RankingRuleMapping> {


        /// <inheritdoc/>
        public RankingRuleMapping Copy() {
            RankingRuleMapping mapping = new RankingRuleMapping();

            foreach (var item in this) 
                mapping.Add(item.Key, item.Value);

            return mapping;
        }

        /// <summary>
        /// This is the default ScoreConfig name. When no other ScoreConfig names match, this value may point to the default RankingRuleDefinition to use instead.
        /// </summary>
        public const string DEFAULTDEF = "DefaultDef";
    }
}
