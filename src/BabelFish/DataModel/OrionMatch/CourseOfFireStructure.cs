using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Converters.Microsoft;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    ///
    /// <para>It is generally best to construct a new instance using the <see cref="FactoryAsync(SetName)"/> method.</para>
    /// </summary>
    /// <remarks>New with BabelFish 2.0 / Orion 3.0 DataModel</remarks>
    public class CourseOfFireStructure : IGetCourseOfFireDefinition {

        public CourseOfFireStructure() { }

        public static async Task<CourseOfFireStructure> FactoryAsync( SetName setName ) {

            if (setName.IsDefault)
                throw new DefinitionNotFoundException( "May not add a new COURSE OF FIRE using default." );

            var cof = await DefinitionCache.GetCourseOfFireDefinitionAsync( setName );

            var structure = new CourseOfFireStructure();
            structure.CourseOfFireDef = setName;
            structure.CourseOfFireName = cof.CommonName;
            structure.StartDate = DateTime.Today;
            structure.EndDate = DateTime.Today;
            structure.ScoreConfigName = cof.ScoreConfigDefault;
            structure.TargetCollectionName = cof.DefaultTargetCollectionName;
            if (!cof.RequiredAttributeDef.IsDefault)
                structure.Attributes.Add( await AttributeConfiguration.FactoryAsync( cof.RequiredAttributeDef ) );

            return structure;
        }

        /// <summary>
        /// Unique identifier, usually incremented, within a <see cref="Match"/>
        /// <para>The value of 0 is reserved and may not be used. </para>
        /// </summary>
        public int CourseOfFireId { get; set; } = 1;

        /// <summary>
        /// Human readable name given to this CourseOfFireStruccture. It is generally best if it is unique
        /// within a <see cref="Match"/>, but not required.
        /// </summary>
        public string CourseOfFireName { get; set; } = string.Empty;

        public SetName CourseOfFireDef { get; set; } = SetName.Parse( "v1.0:orion:Default" );

        /// <summary>
        /// The start date of this Course of Fire.
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// the end date of this Course of fire.
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Human readable description for this Course of Fire.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// SetName of the ScoreConfig used in this match.
        /// NOTE: The name of the ScoreFormatCollection is specified in the Course of Fire 
        /// </summary>
        /// <remarks>In the future MatchV2 class, Matches will be able to have multiple COURSES OF FIRE, and with each CourseOfFireDef will have its own ScoreConfigName. This property will be replaced.</remarks>
        public string ScoreConfigName { get; set; }

        /// <summary>
        /// Name of the TargetCollection used in this match.
        /// </summary>
        /// <remarks>In the future MatchV2 class, Matches will be able to have multiple COURSES OF FIRE, and with each CourseOfFireDef will have its own TargetColle3citonName. This property will be replaced.</remarks>
        public string TargetCollectionName { get; set; }

        [G_NS.JsonProperty( DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public EntryTypes TypesOfEntries { get; set; } = EntryTypes.INDIVIDUAL_AND_TEAM;

        public List<AttributeConfiguration> Attributes { get; set; } = new List<AttributeConfiguration>();

        /// <inheritdoc />
        public async Task<CourseOfFire> GetCourseOfFireDefinitionAsync() {
            return await DefinitionCache.GetCourseOfFireDefinitionAsync( CourseOfFireDef );
        }
    }
}
