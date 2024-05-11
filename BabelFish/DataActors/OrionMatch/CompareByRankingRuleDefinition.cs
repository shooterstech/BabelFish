using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataActors.OrionMatch {

    /// <summary>
    /// Compares two IEventScores (e.g. Result COF or Result Event) by the rules defined in a 
    /// Ranking Rule Definition.
    /// </summary>
    public class CompareByRankingRuleDefinition : IComparer<IEventScores> {

        public CompareByRankingRuleDefinition( RankingRule rankingRule) { 
            this.RankingRule = rankingRule;
        }

        public RankingRule RankingRule { get; private set; }

        public int Compare( IEventScores x, IEventScores y ) {
            
            foreach( var rankingRule in this.RankingRule.RankingRules ) {

                foreach( var rule in rankingRule.)

            }
        }
    }
}
