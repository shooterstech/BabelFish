using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class ScoreConfig: IReconfigurableRulebookObject {

        public ScoreConfig() { }

        public string ScoreConfigName { get; set; } = string.Empty;

        /// <summary>
        /// The Keys to the dictionary should be set by the parent SCORE FORMAT COLLECTION's ScoreFormats list.
        /// Values are a Score Format, eg. "{i} - {x}"
        /// </summary>
        public Dictionary<string, string> ScoreFormats { get; set; } = new Dictionary<string, string>();

        /// <inheritdoc/>
        [JsonProperty( Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        public override string ToString() {
            return ScoreConfigName;
        }
    }
}
