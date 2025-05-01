using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class ScoreConfig: IReconfigurableRulebookObject
    {

        public ScoreConfig() { }


		[G_STJ_SER.JsonPropertyOrder( 1 )]
		[G_NS.JsonProperty( Order = 1 )]
		public string ScoreConfigName { get; set; } = string.Empty;

        /// <summary>
        /// The Keys to the dictionary should be set by the parent SCORE FORMAT COLLECTION's ScoreFormats list.
        /// Values are a Score Format, eg. "{i} - {x}"
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        public Dictionary<string, string> ScoreFormats { get; set; } = new Dictionary<string, string>();

		/// <summary>
		/// The ScoreCompoent to use to display a 'simple' score value.
		/// </summary>
        [G_STJ_SER.JsonPropertyOrder( 3 )]
		[G_NS.JsonProperty( Order = 3, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
		public ScoreComponent ScoreComponent { get; set; } = ScoreComponent.D;

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
