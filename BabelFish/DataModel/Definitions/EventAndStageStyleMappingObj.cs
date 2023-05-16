using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Given the Target Collection Name, AttributeValueAppellation, and EventAppellation / StageAppellation, the Event and Stage Style Mapping
    /// defines how to map these inputs to an EventStyle or StageStyle. This is then used in the generation of a ResultCOF data structure.
    /// </summary>
    public class EventAndStageStyleMappingObj {

        public EventAndStageStyleMappingObj() {
            EventStyleMappings = new List<EventStyleSelection>();

            StageStyleMappings = new List<StageStyleSelection>();
        }

        [OnDeserialized]
        internal void OnDeserializedMethod( StreamingContext context ) {
            if (EventStyleMappings == null)
                EventStyleMappings = new List<EventStyleSelection>();

            if (StageStyleMappings == null)
                StageStyleMappings = new List<StageStyleSelection>();
        }

        /// <summary>
        /// The AttributeValueAppellation to use within this mapping.
        /// </summary>
        [JsonProperty( Order = 1 )]
        [DefaultValue( "" )]
        public string AttributeValueAppellation { get; set; } = string.Empty;

        /// <summary>
        /// The TargetCollectionName to use within this mapping.
        /// </summary>
        [JsonProperty( Order = 2 )]
        [DefaultValue( "" )]
        public string TargetCollectionName { get; set; } = string.Empty;

        /// <summary>
        /// The EventStyle definition to use if a mappings could not be found.
        /// </summary>
        [JsonProperty( Order = 3 )]
        [DefaultValue( "" )] //Purposefully setting the JSON serializer default value to an empty string, which does not equal the object initialzer default value.
        public string DefaultEventStyleDef { get; set; } = "v1.0:orion:Default";

        /// <summary>
        /// The StageStyle definition to use if a mapping could not be found.
        /// </summary>
        [JsonProperty( Order = 4 )]
        [DefaultValue( "" )] //Purposefully setting the JSON serializer default value to an empty string, which does not equal the object initialzer default value.
        public string DefaultStageStyleDef { get; set; } = "v1.0:orion:Default";

        public List<EventStyleSelection> EventStyleMappings { get; set; }

        public List<StageStyleSelection> StageStyleMappings { get; set; }
    }
}
