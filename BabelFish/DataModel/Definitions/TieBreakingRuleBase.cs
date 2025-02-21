using System;
using System.Collections.Generic;
using System.ComponentModel;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Defines how to compare two IEventScore instances.
    /// </summary>
    [Serializable]
    [G_NS.JsonConverter( typeof( G_BF_NS_CONV.TieBreakingRuleConverter ) )]
    public abstract class TieBreakingRuleBase : IReconfigurableRulebookObject {

        /// <summary>
        /// Public constructor
        /// </summary>
        public TieBreakingRuleBase() { }

        /// <summary>
        /// Specifies the method to use to compare two competitors.
        /// </summary>
        /// <remarks>Method is also the Concrete class differentiator.</remarks>
        
        [DefaultValue( TieBreakingRuleMethod.SCORE )]
        public TieBreakingRuleMethod Method { get; set; }

        /// <summary>
        /// How the comparison should be made.
        /// </summary>
        public SortBy SortOrder { get; set; }

        /// <summary>
        /// If the fields EventName and Values require interpretation, CompiledTieBreakingRules
        /// interpres them and returns a new list of TieBreakingRules cooresponding to the interpretation.
        /// If interpretation is not required, then it returns a list of one tie breaking rule, itself.
        /// </summary>
        public virtual List<TieBreakingRuleBase> GetCompiledTieBreakingRules() {
            return new List<TieBreakingRuleBase>() { this };
        }

        /// <inheritdoc/>
		[G_STJ_SER.JsonPropertyOrder( 100 )]
        [G_NS.JsonProperty( Order = 100 )]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;

    }
}
