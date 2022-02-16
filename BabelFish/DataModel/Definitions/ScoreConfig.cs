using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.Definitions {
    public class ScoreConfig {

        public ScoreConfig() { }

        public string ScoreConfigName { get; set; }

        /// <summary>
        /// The Keys to the dictionary should be set by the parent SCORE FORMAT COLLECTION's ScoreFormats list.
        /// Values are a Score Format, eg. "{i} - {x}"
        /// </summary>
        public Dictionary<string, string> ScoreFormats { get; set; }
    }
}
