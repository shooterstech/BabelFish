using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// Given the Target Collection Name, AttributeValueAppellation, and EventAppellation / StageAppellation, the Event and Stage Style Mapping
    /// defines how to map these inputs to an EventStyle or StageStyle. This is then used in the generation of a ResultCOF data structure.
    /// </summary>
    public class EventAndStageStyleMapping : Definition, ICopy<EventAndStageStyleMapping>
    {

        public EventAndStageStyleMapping() : base() {
            Type = DefinitionType.EVENTANDSTAGESTYLEMAPPING;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod( StreamingContext context ) {
            base.OnDeserializedMethod( context );

            if (Mappings == null)
                Mappings = new List<EventAndStageStyleMappingObj>();
        }

        /// <inheritdoc />
        public EventAndStageStyleMapping Copy()
        {
            EventAndStageStyleMapping esm = new EventAndStageStyleMapping();
            this.Copy(esm);
            esm.DefaultMapping = this.DefaultMapping.Copy();
            if (this.Mappings != null)
            {
                foreach (var map in this.Mappings)
                {
                    esm.Mappings.Add(map.Copy());
                }
            }
            return esm;
        }

        /// <summary>
        /// The default Event Style and Stage Styles to use, when no matches can be found with in the .Mappings array. 
        /// </summary>
        public EventAndStageStyleMappingObj DefaultMapping { get; set; } = new EventAndStageStyleMappingObj();

        /// <summary>
        /// Lists the Event Styles and Stage Styles to use, for a specific set of Target Collection Names, Attribute Value Appelations,
        /// Event Appelations and Stage Appelations.
        /// </summary>
        public List<EventAndStageStyleMappingObj> Mappings { get; set; } = new List<EventAndStageStyleMappingObj> { };
    }
}
