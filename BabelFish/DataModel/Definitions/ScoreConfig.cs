using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.Definitions {
    public class ScoreConfig {

        public ScoreConfig() { }

        public string ScoreConfigName { get; set; } = string.Empty;

        /// <summary>
        /// The Keys to the dictionary should be set by the parent SCORE FORMAT COLLECTION's ScoreFormats list.
        /// Values are a Score Format, eg. "{i} - {x}"
        /// </summary>
        public Dictionary<string, string> ScoreFormats { get; set; } = new Dictionary<string, string>();
    }
}
