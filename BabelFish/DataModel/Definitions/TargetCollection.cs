using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// A TARGET COLLECTION defines a set of TARGET definitions that are used together for multiple COURSE OF FIRE scripts.  with each other during competitions or practice. It may only list one TARGET definition, or it may list multiple TARGET definitions. Each index is then used as part of a COURSE OF FIRE script.
    /// 
    /// For example, in high power, one TARGET COLLECTION could include the 200yds. SR target, the 300yds SR-3, and the 600yds MR-1 target for the National Match Course. Another TARGET SET, for the same style of shooting could define the 200yds. SR target, the 300yds reduced for 200yds SR-42, and the 600yds reduced for 200yds MR-2. 
    /// 
    /// In another example, for smallbore three position, one TARGET COLLECTION could include the 50m Rifle target. Another the 50m Rifle reduced for 50yds. And yet another the 50m Rifle reduced for 50ft. 
    /// </summary>
    public class TargetCollection : Definition, ICopy<TargetCollection> {
        
        /// <summary>
        /// Public constructor
        /// </summary>
        public TargetCollection() : base() {
            Type = DefinitionType.TARGETCOLLECTION;
        }

        /// <inheritdoc/>
        public TargetCollection Copy() {
            TargetCollection copy = new TargetCollection();
            base.Copy( copy );

            foreach( var tc in this.TargetCollections) 
                copy.TargetCollections.Add( tc.Copy() );

            return copy;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);

            if (TargetCollections == null)
                TargetCollections = new List<TargetCollectionModal>();
        }

        /// <summary>
        /// The list of TargetCollections for use as options. Each TargetCollections must have a its list of TargetDefs be the same length. 
        /// </summary>
        public List<TargetCollectionModal> TargetCollections { get; set; } = new List<TargetCollectionModal>();

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
