﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BabelFish.DataModel.Definitions {

    [Serializable]
    public class ScoreFormatCollection : Definition {
        
        public ScoreFormatCollection() : base() {
            Type = Definition.DefinitionType.SCOREFORMATCOLLECTION;
        }


        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);
        }

        public List<string> ScoreFormats { get; set; } = new List<string>();

        public List<ScoreConfig> ScoreConfigs { get; set; } = new List<ScoreConfig>();
    }
}
