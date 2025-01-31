using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class ScoreConfig: IReconfigurableRulebookObject
    {

        public ScoreConfig() { }

        public string ScoreConfigName { get; set; } = string.Empty;

        /// <summary>
        /// The Keys to the dictionary should be set by the parent SCORE FORMAT COLLECTION's ScoreFormats list.
        /// Values are a Score Format, eg. "{i} - {x}"
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        public Dictionary<string, string> ScoreFormats { get; set; } = new Dictionary<string, string>();

        /// <inheritdoc/>
		[G_STJ_SER.JsonPropertyOrder( 100 )]
        [G_NS.JsonProperty( Order = 100 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        /// <inheritdoc/>
        public override string ToString() {
            return ScoreConfigName;
        }
    }
}
