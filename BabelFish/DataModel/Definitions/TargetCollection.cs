using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShootersTech.BabelFish.DataModel.Definitions {

    [Serializable]
    public class TargetCollection : Definition {
        
        public TargetCollection() : base() {
            Type = DefinitionType.TARGETCOLLECTION;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);
        }

        public List<TargetCollection> TargetCollections { get; set; } = new List<TargetCollection>();
    }
}
