using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.Definitions {

    public class TargetCollectionDefinition : Definition {
        
        public TargetCollectionDefinition() : base() {
            Type = DefinitionType.TARGETCOLLECTION;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);

            if (TargetCollections == null)
                TargetCollections = new List<TargetCollection>();
        }

        public List<TargetCollection> TargetCollections { get; set; } = new List<TargetCollection>();

        public string GetDefaultTargetCollectionName() {
            if (TargetCollections.Count > 0)
                return TargetCollections[0].TargetCollectionName;

            return "";
        }

        public bool IsValidTargetCollectionName( string targetCollectionName ) {
            foreach (var tc in TargetCollections) {
                if (tc.TargetCollectionName == targetCollectionName)
                    return true;
            }

            return false;
        }
    }
}
