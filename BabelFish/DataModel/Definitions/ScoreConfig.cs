using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class ScoreConfig: IReconfigurableRulebookObject, ICopy<ScoreConfig>
    {

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

        public ScoreConfig Copy()
        {
            ScoreConfig sc = new ScoreConfig();
            sc.ScoreConfigName = this.ScoreConfigName;
            if (this.ScoreFormats != null)
            {
                foreach (KeyValuePair<string, string> pair in this.ScoreFormats)
                {
                    sc.ScoreFormats.Add(pair.Key, pair.Value);
                }
            }
            return sc;
        }

        public override string ToString() {
            return ScoreConfigName;
        }
    }
}
