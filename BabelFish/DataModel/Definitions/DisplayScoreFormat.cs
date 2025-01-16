using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {
    [Serializable]
    public class DisplayScoreFormat : IReconfigurableRulebookObject
    {

        List<string> errorList = new List<string>();

        public DisplayScoreFormat() { }

        public string Name { get; set; } = string.Empty;

        public string ScoreFormat { get; set; } = string.Empty;

        public float MaxShotValue { get; set; } = 0;

        /// <inheritdoc/>
        [JsonPropertyOrder( 99 )]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;
    }
}
