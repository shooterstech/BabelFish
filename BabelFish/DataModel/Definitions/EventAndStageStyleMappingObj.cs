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
    public class EventAndStageStyleMappingObj : IReconfigurableRulebookObject, ICopy<EventAndStageStyleMappingObj> {

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

            if (AttributeValueAppellation == null)
                AttributeValueAppellation = new List<string>();

            if (TargetCollectionName == null)
                TargetCollectionName = new List<string>();
        }

        /// <inheritdoc />
        public EventAndStageStyleMappingObj Copy()
        {
            EventAndStageStyleMappingObj e = new EventAndStageStyleMappingObj();

            e.DefaultEventStyleDef = this.DefaultEventStyleDef;
            e.DefaultStageStyleDef = this.DefaultStageStyleDef;

            if (this.AttributeValueAppellation != null)
            {
                foreach (var av in this.AttributeValueAppellation)
                {
                    e.AttributeValueAppellation.Add(av);
                }
            }
            if (this.TargetCollectionName != null)
            {
                foreach (var av in this.TargetCollectionName)
                {
                    e.TargetCollectionName.Add(av);
                }
            }
            if (this.EventStyleMappings != null)
            {
                foreach(var ess in this.EventStyleMappings)
                {
                    e.EventStyleMappings.Add(ess.Copy());
                }
            }
            if (this.StageStyleMappings != null)
            {
                foreach (var sss in this.StageStyleMappings)
                {
                    e.StageStyleMappings.Add(sss.Copy());
                }
            }
            return e;
        }

        /// <summary>
        /// The AttributeValueAppellation to use within this mapping.
        /// </summary>
        [JsonProperty( Order = 1 )]
        public List<string> AttributeValueAppellation { get; set; } = new List<string>();

        /// <summary>
        /// The TargetCollectionName to use within this mapping.
        /// </summary>
        [JsonProperty( Order = 2 )]
        public List<string> TargetCollectionName { get; set; } = new List<string>();

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

        public List<EventStyleSelection> EventStyleMappings { get; set; } = new List<EventStyleSelection> { };

        public List<StageStyleSelection> StageStyleMappings { get; set; } = new List<StageStyleSelection> { };

        /// <inheritdoc/>
        [JsonProperty( Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;
    }
}
