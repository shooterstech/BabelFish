using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.Definitions {
    [Serializable]
    public class DisplayScoreFormat : IReconfigurableRulebookObject, ICopy<DisplayScoreFormat>
    {

        List<string> errorList = new List<string>();

        public DisplayScoreFormat() { }

        public string Name { get; set; } = string.Empty;

        public string ScoreFormat { get; set; } = string.Empty;

        public float MaxShotValue { get; set; } = 0;

        public DisplayScoreFormat Copy()
        {
            DisplayScoreFormat dsf = new DisplayScoreFormat();
            dsf.Name = this.Name;
            dsf.ScoreFormat = this.ScoreFormat;
            dsf.MaxShotValue = this.MaxShotValue;
            return dsf;
        }

        /// <inheritdoc/>
        [JsonProperty(Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;
    }
}
