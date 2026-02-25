using System.ComponentModel;
using System.Runtime.Serialization;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.Specification.Definitions;

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
    public class CourseOfFire : Definition, IGetTargetCollectionDefinition, IGetScoreFormatCollectionDefinition, IGetEventAndStageStyleMapping, IGetAttributeDefinition {

        /// <summary>
        /// Constructor
        /// </summary>
        public CourseOfFire() : base() {
            Type = DefinitionType.COURSEOFFIRE;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod( StreamingContext context ) {
            base.OnDeserializedMethod( context );
        }

        /// <summary>
        /// A list of Events that make up this COURSE OF FIRE. These are the composite events, those that are made up of other child events.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 20 )]
        [G_NS.JsonProperty( Order = 20 )]
        public List<Event> Events { get; set; } = new List<Event>();

        /// <summary>
        /// A list of Singulars that make up this COURSE OF FIRE. These are the singular events, those that are not made up of other events. Almost always represents a singular shot.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 21 )]
        [G_NS.JsonProperty( Order = 21 )]
        public List<Singular> Singulars { get; set; } = new List<Singular>();

        /// <summary>
        /// Range command script with Paper Targets or EST Configuration. 
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 22 )]
        [G_NS.JsonProperty( Order = 22 )]
        public List<RangeScript> RangeScripts { get; set; } = new List<RangeScript>();

        /// <summary>
        /// A list of AbbreviatedFormats.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 23 )]
        [G_NS.JsonProperty( Order = 23 )]
        public List<AbbreviatedFormat> AbbreviatedFormats { get; set; } = new List<AbbreviatedFormat>();


        /// <summary>
        /// Formatted as a SetName, the TargetCollectionDef to use to score shots. The specific TARGET definition to use is calculated with the 
        /// SegementGroupSegment.TargetSetIndex.
        /// Required.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 24 )]
        [G_NS.JsonProperty( Order = 24, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( "v1.0:orion:Air Rifle" )]
        public string TargetCollectionDef { get; set; } = "v1.0:orion:Air Rifle";

        /// <summary>
        /// The name of the Target Collection to use as the default when creating a new Course of Fire. 
        /// Must be a value specified in the TargetCollectionDef.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 25 )]
        [G_NS.JsonProperty( Order = 25, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public string DefaultTargetCollectionName { get; set; } = "10m Air Rifle";


        /// <summary>
        /// Formatted as a SetName, the ScoreFormatCollectionDef to use to display results to athletes and spectators. 
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 26 )]
        [G_NS.JsonProperty( Order = 26, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( "v1.0:orion:Standard Score Formats" )]
        public string ScoreFormatCollectionDef { get; set; } = "v1.0:orion:Standard Score Formats";

        /// <summary>
        /// The default ScoreConfig to use, within the ScoreFormatCollection. 
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 27 )]
        [G_NS.JsonProperty( Order = 27, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( "Decimal" )]
        public string ScoreConfigDefault { get; set; } = "Decimal";

        /// <summary>
        /// The default Event and Stage Style Mapping file to use. 
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 28 )]
        [G_NS.JsonProperty( Order = 28, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( "v1.0:orion:Default" )]
        public string DefaultEventAndStageStyleMappingDef { get; set; } = "v1.0:orion:Default";

        /// <summary>
        /// The default Attribute Value to use to determine a user's Attribute Value Appellation when shooting this course of fire.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 29 )]
        [G_NS.JsonProperty( Order = 29, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( "" )]
        [Obsolete( "Use RequiredAttributeDef instead, which specifies a required Attribute that must be be used to determine a participant's Attribute Value Appellation when shooting this course of fire." )]
        public string DefaultAttributeDef { get; set; } = string.Empty;

        /// <summary>
        /// Formatted as a SetName, the default Attribute definition that specifies a required Attribute that must be used to determine a participant's Attribute Value Appellation,
        /// and with it their EVENT STYLE and STAGE STYLE when shooting this course of fire.
        /// <para>Specifying the default value of v1.0:orion:Default, means there are no required attributes for this COF. </para>
        /// <para>The specified ATTRIBUTE must be a simple apptribute with closed field values, each of which must specify an Attribute Value Appellation.</para>
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 29 )]
        [G_NS.JsonProperty( Order = 29 )]
        public SetName RequiredAttributeDef { get; set; } = Scopos.BabelFish.DataModel.Definitions.SetName.Parse( "v1.0:orion:Default" );

        /// <summary>
        /// Newtonsoft helper method to determine whether to serialize the DefaultAttributeDef property. We don't want to serialize it if it's set to the default value, which means there are no required attributes for this COF.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeDefaultAttributeDef() {
            // Don't serialize if the value is null or empty string, which means there are no required attributes for this COF.
            return RequiredAttributeDef.ToString() != "v1.0:orion:Default";
        }

        /// <summary>
        /// The default Result Engine Compare Type to use when calculating Rank Deltas. 
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 32 )]
        [G_NS.JsonProperty( Order = 30, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( ResultEngineCompareType.WINDOW_1_MINUTE )]
        public ResultEngineCompareType DefaultCompareType { get; set; } = ResultEngineCompareType.WINDOW_1_MINUTE;

        /// <summary>
        /// The default expected diameter of the bullet shot at the target.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 40 )]
        [G_NS.JsonProperty( Order = 40 )]
        [DefaultValue( 4.5 )]
        public float DefaultExpectedDiameter { get; set; } = (float)4.5;

        /// <summary>
        /// The default bullet diameter to use for scoring, measured in mm.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 41 )]
        [G_NS.JsonProperty( Order = 41, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( 4.5 )]
        public float DefaultScoringDiameter { get; set; } = (float)4.5;

        [G_STJ_SER.JsonPropertyOrder( 42 )]
        [G_NS.JsonProperty( Order = 42, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [Obsolete( "Use RangeScriptType, which is part of a RangeScript, instead." )]
        public COFTypeOptions COFType { get; set; }

        /// <inheritdoc />
        /// <exception cref="XApiKeyNotSetException" />
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public async Task<TargetCollection> GetTargetCollectionDefinitionAsync() {
            SetName targetCollectionDef = Scopos.BabelFish.DataModel.Definitions.SetName.Parse( TargetCollectionDef );
            return await DefinitionCache.GetTargetCollectionDefinitionAsync( targetCollectionDef );
        }

        /// <inheritdoc />
        /// <exception cref="XApiKeyNotSetException" />
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public async Task<ScoreFormatCollection> GetScoreFormatCollectionDefinitionAsync() {

            SetName scoreFormatCollectionSetName = Scopos.BabelFish.DataModel.Definitions.SetName.Parse( ScoreFormatCollectionDef );
            return await DefinitionCache.GetScoreFormatCollectionDefinitionAsync( scoreFormatCollectionSetName );
        }

        /// <inheritdoc />
        /// <remarks>Returns the EVENT AND STAGE STYLE MAPPING definition from .DefaultEventAndStageStyleMappingDef</remarks>
        /// <exception cref="XApiKeyNotSetException" />
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public async Task<EventAndStageStyleMapping> GetEventAndStageStyleMappingDefinitionAsync() {

            SetName eventAndStageStyleMappingSetName = Definitions.SetName.Parse( DefaultEventAndStageStyleMappingDef );
            return await DefinitionCache.GetEventAndStageStyleMappingDefinitionAsync( eventAndStageStyleMappingSetName );
        }


        /// <inheritdoc />
        /// <remarks>
        /// Returns the ATTRIBUTE definition from RequiredAttributeDef.
        /// </remarks>
        /// <exception cref="XApiKeyNotSetException" />
        /// <exception cref="DefinitionNotFoundException" />
        /// <exception cref="ScoposAPIException" />
        public async Task<Attribute> GetAttributeDefinitionAsync() {
            return await DefinitionCache.GetAttributeDefinitionAsync( this.RequiredAttributeDef );
        }

        /// <summary>
        /// Helper function to return the SetName of a DefaultTargetDefinition given a collectionName
        /// </summary>
        /// <param name="targetCollectionName"></param>
        /// <returns></returns>
        public async Task<SetName> GetDefaultTargetDefinition( string targetCollectionName ) {
            var targetCollectionDef = await GetTargetCollectionDefinitionAsync();
            return targetCollectionDef.GetTargetCollection( targetCollectionName ).TargetDefs[0];
        }

        /// <inheritdoc />
        public override async Task<bool> GetMeetsSpecificationAsync() {
            var validation = new IsCourseOfFireValid();

            var meetsSpecification = await validation.IsSatisfiedByAsync( this );
            SpecificationMessages = validation.Messages;

            return meetsSpecification;
        }

        public override bool ConvertValues() {
            base.ConvertValues();

            foreach (var rs in this.RangeScripts) {
                foreach (var sg in rs.SegmentGroups) {

                    sg.DefaultCommand.Parent = rs.DefaultCommand;
                    sg.DefaultSegment.Parent = rs.DefaultSegment;

                    foreach (var c in sg.Commands) {
                        c.Parent = sg.DefaultCommand;
                    }

                    foreach (var s in sg.Segments) {
                        s.Parent = sg.DefaultSegment;
                    }
                }
            }

            foreach (var e in this.Events) {
                if (e.EventType == EventtType.STAGE && e.TargetCollectionIndex < 0) {
                    e.TargetCollectionIndex = 0;
                }
            }

            return true;
        }

        /// <inheritdoc />
        public override bool SetDefaultValues() {
            base.SetDefaultValues();

            var topLvelEvent = new EventExplicit() {
                EventName = "Individual",
                EventType = EventtType.EVENT,
                EventStyleMapping = new EventStyleMapping(),
                ResultListFormatDef = "v1.0:orion:Default Individual",
                RankingRuleMapping = new RankingRuleMapping( "v1.0:orion:Generic Qualification" )
            };
            topLvelEvent.Children.Add( "ChildStage" );

            var stageEvent = new EventExplicit() {
                EventName = "Stage Event",
                EventType = EventtType.STAGE,
                StageStyleMapping = new StageStyleMapping(),
                ResultListFormatDef = "v1.0:orion:Default Individual",
                RankingRuleMapping = new RankingRuleMapping( "v1.0:orion:Generic Qualification" )
            };
            stageEvent.Children.Add( "String Event" );

            var stringEvent = new EventDerived() {
                EventName = "String Event",
                EventType = EventtType.STRING,
                ChildEventName = "S{}",
                ChildValues = new ValueSeries( "1..10" )
            };

            Events.Add( topLvelEvent );
            Events.Add( stageEvent );
            Events.Add( stringEvent );

            Singulars.Add( new Singular() {
                EventName = "S{}",
                Values = new ValueSeries( "1..10" ),
                StageLabel = "S"
            } );

            AbbreviatedFormats.Add( new AbbreviatedFormat() {
                FormatName = "Aggregate",
                EventName = "Individual",
                Children = new List<AbbreviatedFormatChild>() {
                    new AbbreviatedFormatChildExplicit() {EventName = "Stage Event" },
                    new AbbreviatedFormatChildExplicit() {EventName = "String Event" }
                }
            } );

            return true;

        }
    }
}
