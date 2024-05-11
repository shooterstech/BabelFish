using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    [Serializable]
    public class RankingDirective {

        public RankingDirective() {
        }

        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {

            if (Rules == null)
                this.Rules = new List<TieBreakingRule>();

            if (ListOnly == null)
                this.ListOnly = new List<TieBreakingRule>();
        }

        /// <summary>
        /// A string indicating who to apply this RankingRules to. The first, and only the first, RankingRule must have a value “*”.
        /// The value must be one of the following:
        /// “*” to indicate to apply the rule to all participants in the event.
        /// “n..m” where n and m are integer values, representing the rank of the participants after the last RankingRule was applied. 
        /// For example, “1..8” means to apply this RankingRule to participants currently in first through eighth place.
        /// Not required, assumed to be '*' if missing.
        /// </summary>
        [DefaultValue( "*" )]
        public string AppliesTo { get; set; } = "*";

        /// <summary>
        /// An ordered list of TieBreakingRules to follow to sort two participants in an event. 
        /// 
        /// The result engine must use the list of Rule in order until the tie is broken. Once the tie is broken the remaining 
        /// Rules are ignored. If the tie can not be broken given the set of Rules the two competitors are given the same rank. 
        /// 
        /// This attribute is required and must have one or more elements in the list.
        /// </summary>
        public List<TieBreakingRule> Rules { get; set; } = new List<TieBreakingRule>();

        /// <summary>
        /// In the event that tie between two participants can not be broken, the TieBreakingRules in ListOnly are used to sort participants for display purposes only.
        /// 
        /// The result engine must use the list of TieBreakingRules in the order listed. Once the tie is broke the remaining rules may be ignored. 
        /// These rules do not affect a participants rank, only the order they are listed.
        /// 
        /// This attribute is not required.
        /// </summary>
        public List<TieBreakingRule> ListOnly { get; set; } = new List<TieBreakingRule>();
    }
}
