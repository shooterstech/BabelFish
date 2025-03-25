using Scopos.BabelFish.DataActors.Specification.Definitions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// Given the Target Collection Name, AttributeValueAppellation, and EventAppellation / StageAppellation, the Event and Stage Style Mapping
    /// defines how to map these inputs to an EventStyle or StageStyle. This is then used in the generation of a ResultCOF data structure.
    /// </summary>
    public class EventAndStageStyleMapping : Definition, IGetEventStyleDefinitionList, IGetStageStyleDefinitionList {

        public EventAndStageStyleMapping() : base() {
            Type = DefinitionType.EVENTANDSTAGESTYLEMAPPING;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod( StreamingContext context ) {
            base.OnDeserializedMethod( context );

            if (Mappings == null)
                Mappings = new List<EventAndStageStyleMappingObj>();
        }

        /// <summary>
        /// The default Event Style and Stage Styles to use, when no matches can be found with in the .Mappings array. 
        /// </summary>
        [G_NS.JsonProperty( Order = 20  )]
        public EventAndStageStyleMappingObj DefaultMapping { get; set; } = new EventAndStageStyleMappingObj();

        /// <summary>
        /// Lists the Event Styles and Stage Styles to use, for a specific set of Target Collection Names, Attribute Value Appelations,
        /// Event Appelations and Stage Appelations.
        /// </summary>
        [G_NS.JsonProperty( Order = 20 )]
        public List<EventAndStageStyleMappingObj> Mappings { get; set; } = new List<EventAndStageStyleMappingObj> { };

        /// <inheritdoc />
        public override async Task<bool> GetMeetsSpecificationAsync() {
            var validation = new IsEventAndStageStyleMappingValid();

            var meetsSpecification = await validation.IsSatisfiedByAsync( this );
            SpecificationMessages = validation.Messages;

            return meetsSpecification;

        }

        /// <inheritdoc />
        /// <remarks>
        /// Returns all EVENT STYLES referenced within this EVENT AND STAGE STYLE MAPPING
        /// </remarks>
        /// <exception cref="XApiKeyNotSetException" ></exception>
        /// <exception cref="ScoposAPIException" ></exception>
        /// <exception cref="DefinitionNotFoundException" ></exception>
        public async Task<Dictionary<string, EventStyle>> GetEventStyleDefinitionListAsync() {
            var list = new Dictionary<string, EventStyle>();

            //DefaultMapping
            list[DefaultMapping.DefaultEventStyleDef] = await DefaultMapping.GetEventStyleDefinitionAsync();

            foreach( var esm in DefaultMapping.EventStyleMappings) {

                if ( ! list.ContainsKey( esm.EventStyleDef ) ) {
                    list[ esm.EventStyleDef ] = await esm.GetEventStyleDefinitionAsync( );
                }
            }

            //List of Mappings
            foreach (var mapping in Mappings) {
                if (!list.ContainsKey( mapping.DefaultEventStyleDef )) {
                    list[ mapping.DefaultEventStyleDef ] = await mapping.GetEventStyleDefinitionAsync( );
                }

                foreach (var esm in mapping.EventStyleMappings) {

                    if (!list.ContainsKey( esm.EventStyleDef )) {
                        list[esm.EventStyleDef] = await esm.GetEventStyleDefinitionAsync();
                    }
                }
            }

            return list;
        }

        /// <inheritdoc />
        /// <remarks>
        /// Returns all STAGE STYLES referenced within this EVENT AND STAGE STYLE MAPPING
        /// </remarks>
        /// <exception cref="XApiKeyNotSetException" ></exception>
        /// <exception cref="ScoposAPIException" ></exception>
        /// <exception cref="DefinitionNotFoundException" ></exception>
        public async Task<Dictionary<string, StageStyle>> GetStageStyleDefinitionListAsync() {
            var list = new Dictionary<string, StageStyle>();

            //DefaultMapping
            list[DefaultMapping.DefaultStageStyleDef] = await DefaultMapping.GetStageStyleDefinitionAsync();

            foreach (var esm in DefaultMapping.StageStyleMappings) {

                if (!list.ContainsKey( esm.StageStyleDef )) {
                    list[esm.StageStyleDef] = await esm.GetStageStyleDefinitionAsync();
                }
            }

            //List of Mappings
            foreach (var mapping in Mappings) {
                if (!list.ContainsKey( mapping.DefaultStageStyleDef )) {
                    list[mapping.DefaultStageStyleDef] = await mapping.GetStageStyleDefinitionAsync();
                }

                foreach (var esm in mapping.StageStyleMappings) {

                    if (!list.ContainsKey( esm.StageStyleDef )) {
                        list[esm.StageStyleDef] = await esm.GetStageStyleDefinitionAsync();
                    }
                }
            }

            return list;
        }

        /// <inheritdoc />
        public override bool SetDefaultValues() {
            base.SetDefaultValues();

            this.DefaultMapping = new EventAndStageStyleMappingObj();
            this.DefaultMapping.DefaultStageStyleDef = "v1.0:orion:Default";
            this.DefaultMapping.DefaultEventStyleDef = "v1.0:orion:Default";
            this.Mappings = new List<EventAndStageStyleMappingObj>();

            return true;
        }
    }
}
