using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Given the Target Collection Name, AttributeValueAppellation, and EventAppellation / StageAppellation, the Event and Stage Style Mapping
    /// defines how to map these inputs to an EventStyle or StageStyle. This is then used in the generation of a ResultCOF data structure.
    /// </summary>
    public class EventAndStageStyleMappingObj : IReconfigurableRulebookObject, IGetEventStyleDefinition, IGetStageStyleDefinition {

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

        /// <summary>
        /// The AttributeValueAppellation to use within this mapping.
        /// </summary>
        [JsonPropertyOrder ( 1 )]
        public List<string> AttributeValueAppellation { get; set; } = new List<string>();

        /// <summary>
        /// The TargetCollectionName to use within this mapping.
        /// </summary>
        [JsonPropertyOrder ( 2 )]
        public List<string> TargetCollectionName { get; set; } = new List<string>();

        /// <summary>
        /// The EventStyle definition to use if a mappings could not be found.
        /// </summary>
        [JsonPropertyOrder ( 3 )]
        [DefaultValue( "" )] //Purposefully setting the JSON serializer default value to an empty string, which does not equal the object initialzer default value.
        public string DefaultEventStyleDef { get; set; } = "v1.0:orion:Default";

        /// <summary>
        /// The StageStyle definition to use if a mapping could not be found.
        /// </summary>
        [JsonPropertyOrder ( 4 )]
        [DefaultValue( "" )] //Purposefully setting the JSON serializer default value to an empty string, which does not equal the object initialzer default value.
        public string DefaultStageStyleDef { get; set; } = "v1.0:orion:Default";

        public List<EventStyleSelection> EventStyleMappings { get; set; } = new List<EventStyleSelection> { };

        public List<StageStyleSelection> StageStyleMappings { get; set; } = new List<StageStyleSelection> { };

        /// <inheritdoc/>
        [DefaultValue( "" )]
        [JsonPropertyOrder( 99 )]
        public string Comment { get; set; } = string.Empty;

        /// <inheritdoc/>
        /// <remarks>Returns the EVENT STYLE definition referenced by the property DefaultEventStyleDef </remarks>
        /// <exception cref="XApiKeyNotSetException" ></exception>
        /// <exception cref="ScoposAPIException" ></exception>
        /// <exception cref="DefinitionNotFoundException" ></exception>
        public async Task<EventStyle> GetEventStyleDefinitionAsync() {

            var sb = SetName.Parse( DefaultEventStyleDef );
            return await DefinitionCache.GetEventStyleDefinitionAsync( sb );
        }

        /// <inheritdoc/>
        /// <remarks>Returns the STAGE STYLE definition referenced by the property DefaultStageStyleDef </remarks>
        /// <exception cref="XApiKeyNotSetException" ></exception>
        /// <exception cref="ScoposAPIException" ></exception>
        /// <exception cref="DefinitionNotFoundException" ></exception>
        public async Task<StageStyle> GetStageStyleDefinitionAsync() {

            var sb = SetName.Parse( DefaultStageStyleDef );
            return await DefinitionCache.GetStageStyleDefinitionAsync( sb );
        }
    }
}
