using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.Definitions {

    [Serializable]
    public class ScoreFormatCollection : Definition {
        
        public ScoreFormatCollection() : base() {
            Type = DefinitionType.SCOREFORMATCOLLECTION;
        }


        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);

            if (ScoreConfigs == null)
                ScoreConfigs = new List<ScoreConfig>();
        }

        public List<string> ScoreFormats { get; set; } = new List<string>();

        public List<ScoreConfig> ScoreConfigs { get; set; } = new List<ScoreConfig>();

        public string GetDefaultScoreConfigName() {
            if (ScoreConfigs.Count > 0)
                return ScoreConfigs[0].ScoreConfigName;

            //NOTE: This really is an error condition, as all ScoreFormatCollection should have at least 1 ScoreConfig
            return "";
        }

        public bool IsValidScoreConfigName( string scoreConfigName ) {
            foreach (var sc in ScoreConfigs) {
                if (sc.ScoreConfigName == scoreConfigName)
                    return true;
            }

            return false;
        }
    }
}
