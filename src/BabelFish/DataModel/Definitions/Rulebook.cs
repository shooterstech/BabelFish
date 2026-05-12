using static Scopos.BabelFish.DataActors.Specification.Definitions.RulebookSpecification;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// A RULEBOOK is one of the top level Reconfigurable Rulebook definition types. It contains lists of
    /// options for Match Structures, Courses of Fire, and Attributes that a user can use to construct
    /// their own Match Structure and Match. 
    /// </summary>
    public class Rulebook : Definition {

        #region Private Variables
        private Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructors and Initialization
        /// <summary>
        /// Public constructor.
        /// </summary>
        public Rulebook() : base() {
            Type = DefinitionType.RULEBOOK;
        }
        #endregion

        #region Event Handlers

        #endregion

        #region Data Model Properties

        /// <summary>
        /// A set of pre-configured Match Structure templates that a user can use as starting points to construct their own Match Structure and Match.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 20 )]
        [G_NS.JsonProperty( Order = 20 )]
        public List<MatchStructureOption> MatchStructureOptions { get; set; } = new List<MatchStructureOption>();

        /// <summary>
        /// A suggested list of COURSE OF FIRE definitions that a user can use as starting points to construct their own Match Structure.
        /// 
        /// <para>These are not intended to be an exhaustive list of COURSES OF FIRE,
        /// but rather a curated selection of commonly used COURSES OF FIRE that can be easily incorporated into a Match Structure.</para>
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 21 )]
        [G_NS.JsonProperty( Order = 21 )]
        public List<CourseOfFireOption> CourseOfFireOptions { get; set; } = new List<CourseOfFireOption>();

        /// <summary>
        /// A suggested list of ATTRIBUTES definitions that a user can use as starting points to construct their own Match Structure.
        /// <para>These are not intended to be an exhaustive list of ATTRIBUTES,
        /// but rather a curated selection of commonly used ATTRIBUTES that can be easily incorporated into a Match Structure.</para>
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 22 )]
        [G_NS.JsonProperty( Order = 22 )]
        public List<AttributeOption> AttributeOptions { get; set; } = new List<AttributeOption>();
        #endregion

        #region Methods
        /// <inheritdoc />
        public override async Task<bool> GetMeetsSpecificationAsync() {
            var validation = new IsRulebookValid();

            var meetsSpecification = await validation.IsSatisfiedByAsync( this );
            SpecificationMessages = validation.Messages;

            return meetsSpecification;
        }
        #endregion
    }
}
