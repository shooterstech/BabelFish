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
    public class ScoreFormatCollection : Definition, ICopy<ScoreFormatCollection>
    {
        
        public ScoreFormatCollection() : base() {
            Type = DefinitionType.SCOREFORMATCOLLECTION;
        }


        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);

            if (ScoreConfigs == null)
                ScoreConfigs = new List<ScoreConfig>();
        }

        public ScoreFormatCollection Copy() {
            ScoreFormatCollection sfc = new ScoreFormatCollection();
            this.Copy( sfc );
            if (this.ScoreFormats != null) {
                foreach (var sf in this.ScoreFormats) {
                    sfc.ScoreFormats.Add( sf );
                }
            }
            if (this.ScoreConfigs != null) {
                foreach (var sc in this.ScoreConfigs) {
                    sfc.ScoreConfigs.Add( sc.Copy() );
                }
            }
            return sfc;
        }

        public List<string> ScoreFormats { get; set; } = new List<string>();

        public List<ScoreConfig> ScoreConfigs { get; set; } = new List<ScoreConfig>();

        public string GetDefaultScoreConfigName() {
            if (ScoreConfigs.Count > 0)
                return ScoreConfigs[0].ScoreConfigName;

            //NOTE: This really is an error condition, as all ScoreFormatCollection should have at least 1 ScoreConfig
            return "";
        }

        /// <summary>
        /// Helper function that returns a list of the names of the ScoreConfigs
        /// </summary>
        /// <returns></returns>
        public List<string> GetScoreConfigNames() {
            List<string> names = new List<string>();
            foreach( var sc in ScoreConfigs) {
                names.Add(sc.ScoreConfigName);
            }

            return names;
        }

        public bool IsValidScoreConfigName( string scoreConfigName ) {
            foreach (var sc in ScoreConfigs) {
                if (sc.ScoreConfigName == scoreConfigName)
                    return true;
            }

            return false;
        }

        /// <inheritdoc/>
        [JsonProperty(Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;
    }
}
