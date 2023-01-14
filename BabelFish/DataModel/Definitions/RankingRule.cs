using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.Definitions {
    [Serializable]
    public class RankingRule : Definition {

        public RankingRule() : base() {
            Type = DefinitionType.RANKINGRULES;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);
        }

        [JsonProperty(Order = 10)]
        [DefaultValue(null)]
        public List<RankingRule> RankingRules { get; set; } = new List<RankingRule>();

    }
}
