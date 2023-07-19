using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// Given the Target Collection Name, AttributeValueAppellation, and EventAppellation / StageAppellation, the Event and Stage Style Mapping
    /// defines how to map these inputs to an EventStyle or StageStyle. This is then used in the generation of a ResultCOF data structure.
    /// </summary>
    public class EventAndStageStyleMapping : Definition {

        public EventAndStageStyleMapping() : base() {
            Type = DefinitionType.EVENTANDSTAGESTYLEMAPPING;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod( StreamingContext context ) {
            base.OnDeserializedMethod( context );

            if (Mappings == null)
                Mappings = new List<EventAndStageStyleMappingObj>();
        }

        public EventAndStageStyleMappingObj DefaultMapping { get; set; }

        public List<EventAndStageStyleMappingObj> Mappings { get; set; }
    }
}
