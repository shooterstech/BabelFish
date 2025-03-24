using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.Specification.Definitions;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// A COURSE OF FIRE defines the structure of events that make up a competition. 
    /// <para>This structure includes: </para>
    /// <list type="bullet">
    /// <item>Defining each event, both composite events and singular events. A composite event is made up of other events. A singular event is made up only of itself, and is almost always just a shot.</item>
    /// <item>EVENT STYLE and STAGE STYLE assignment to events. </item>
    /// <item>Range officer command script.</item>
    /// <item>EST Target configuration.</item>
    /// <item>Mapping of shots to singular events.</item>
    /// </list>
    /// <para>A COURSE OF FIRE should only describe an event that can be completed with one outing to the range. In other words, an athlete should be able to complete the course of fire with one trip to the range. A multi-day event is the combination of two or more COURSE OF FIRE, that is defined outside of this type.</para>
    /// </summary>
    public class CourseOfFire : Definition, IGetTargetCollectionDefinition, IGetScoreFormatCollectionDefinition, IGetEventAndStageStyleMapping {

        public CourseOfFire() : base() {
            Type = DefinitionType.COURSEOFFIRE;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);
        }

        [G_STJ_SER.JsonPropertyOrder( 11 )]
        [G_NS.JsonProperty( Order = 11 )]
        public COFTypeOptions COFType { get; set; }

        /// <summary>
        /// A human readable short name.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 12 )]
        [G_NS.JsonProperty( Order = 12 )]
        public string CommonName { get; set; } = string.Empty;

        /// <summary>
        /// Formatted as a SetName, the TargetCollectionDef to use to score shots. The specific TARGET definition to use is calculated with the 
        /// SegementGroupSegment.TargetSetIndex.
        /// Required.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 13 )]
        [G_NS.JsonProperty( Order = 13 )]
        [DefaultValue( "v1.0:ntparc:Air Rifle" )]
        public string TargetCollectionDef { get; set; } = "v1.0:ntparc:Air Rifle";

        /// <inheritdoc/>
        public async Task<TargetCollection> GetTargetCollectionDefinitionAsync()
        {
            SetName targetCollectionDef = Scopos.BabelFish.DataModel.Definitions.SetName.Parse(TargetCollectionDef);
            return await DefinitionCache.GetTargetCollectionDefinitionAsync(targetCollectionDef);
        }

        /// <summary>
        /// Helper function to return the SetName of a DefaultTargetDefinition given a collectionName
        /// </summary>
        /// <param name="targetCollectionName"></param>
        /// <returns></returns>
        public async Task<SetName> GetDefaultTargetDefinition( string targetCollectionName )
        {
            var targetCollectionDef = await GetTargetCollectionDefinitionAsync();
            foreach (var collection in targetCollectionDef.TargetCollections)
            {
                if(targetCollectionName == collection.TargetCollectionName)
                {
                    return Scopos.BabelFish.DataModel.Definitions.SetName.Parse(collection.TargetDefs.FirstOrDefault());
                }
            }
            return Scopos.BabelFish.DataModel.Definitions.SetName.Parse(targetCollectionDef.TargetCollections[0].TargetDefs.FirstOrDefault());
        }

        [G_STJ_SER.JsonPropertyOrder( 14 )]
        [G_NS.JsonProperty( Order = 14 )]
        /// <summary>
        /// The name of the Target Collection to use as the default when creating a new Course of Fire. 
        /// Must be a value specified in the TargetCollectionDef.
        /// </summary>
        public string DefaultTargetCollectionName { get; set; }

        /// <summary>
        /// The default expected diameter of the bullet shot at the target.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 15 )]
        [G_NS.JsonProperty( Order = 15 )]
        [DefaultValue(4.5)]
        public float DefaultExpectedDiameter { get; set; } = (float)4.5;

        /// <summary>
        /// The default bullet diameter to use for scoring, measured in mm.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 16 )]
        [G_NS.JsonProperty( Order = 16 )]
        [DefaultValue(4.5)]
        public float DefaultScoringDiameter { get; set; } = (float) 4.5;

        /// <summary>
        /// Formatted as a SetName, the ScoreFormatCollectionDef to use to display results to athletes and spectators. 
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 17 )]
        [G_NS.JsonProperty( Order = 17, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( "v1.0:orion:Standard Score Formats" )]
        public string ScoreFormatCollectionDef { get; set; } = "v1.0:orion:Standard Score Formats";

        /// <summary>
        /// The default ScoreConfig to use, within the ScoreFormatCollection. 
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 18 )]
        [G_NS.JsonProperty( Order = 18, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( "Decimal" )]
        public string ScoreConfigDefault { get; set; } = "Decimal";

        /// <inheritdoc />
        public async Task<ScoreFormatCollection> GetScoreFormatCollectionDefinitionAsync() {

            SetName scoreFormatCollectionSetName = Scopos.BabelFish.DataModel.Definitions.SetName.Parse( ScoreFormatCollectionDef );
            return await DefinitionCache.GetScoreFormatCollectionDefinitionAsync( scoreFormatCollectionSetName );
        }

        /// <inheritdoc />
        public async Task<EventAndStageStyleMapping> GetEventAndStageStyleMappingDefinitionAsync() {

            SetName eventAndStageStyleMappingSetName = Scopos.BabelFish.DataModel.Definitions.SetName.Parse( DefaultEventAndStageStyleMappingDef );
            return await DefinitionCache.GetEventAndStageStyleMappingDefinitionAsync( eventAndStageStyleMappingSetName );
        }

        /// <summary>
        /// The default Event and Stage Style Mapping file to use. 
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 19 )]
        [G_NS.JsonProperty( Order = 19 )]
        [DefaultValue( "v1.0:orion:Default" )]
        public string DefaultEventAndStageStyleMappingDef { get; set; } = "v1.0:orion:Default";

        /// <summary>
        /// The default Attribute Value to use to determine a user's Attribute Value Appellation when shooting this course of fire.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 20 )]
        [G_NS.JsonProperty( Order = 20 )]
        [DefaultValue( "v1.0:orion:Air Rifle Training Category" )]
		public string DefaultAttributeDef { get; set; } = "v1.0:orion:Air Rifle Training Category";

        /// <summary>
        /// Range command script with Paper Targets or EST Configuration. 
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 21 )]
        [G_NS.JsonProperty( Order = 21 )]
        public List<RangeScript> RangeScripts { get; set; } = new List<RangeScript>();

        /// <summary>
        /// A list of Events that make up this COURSE OF FIRE. These are the composite events, those that are made up of other child events.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 22 )]
        [G_NS.JsonProperty( Order = 22 )]
        public List<Event> Events { get; set; } = new List<Event>();

        /// <summary>
        /// A list of Singulars that make up this COURSE OF FIRE. These are the singular events, those that are not made up of other events. Almost always represents a singular shot.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 23 )]
        [G_NS.JsonProperty( Order = 23 )]
        public List<Singular> Singulars { get; set; } = new List<Singular>();

        /// <summary>
        /// A list of AbbreviatedFormats.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 24 )]
        [G_NS.JsonProperty( Order = 24 )]
        public List<AbbreviatedFormat> AbbreviatedFormats { get; set; } = new List<AbbreviatedFormat>();

		/// <inheritdoc />
		public override async Task<bool> GetMeetsSpecificationAsync() {
			var validation = new IsCourseOfFireValid();

			var meetsSpecification = await validation.IsSatisfiedByAsync( this );
			SpecificationMessages = validation.Messages;

			return meetsSpecification;
		}

	}
}
