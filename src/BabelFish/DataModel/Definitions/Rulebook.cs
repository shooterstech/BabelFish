using static Scopos.BabelFish.DataActors.Specification.Definitions.RulebookSpecification;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class Rulebook : Definition {

        #region Private Variables
        private Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructors and Initialization
        public Rulebook() : base() {
            Type = DefinitionType.RULEBOOK;
        }
        #endregion

        #region Event Handlers

        #endregion

        #region Data Model Properties

        [G_STJ_SER.JsonPropertyOrder( 20 )]
        [G_NS.JsonProperty( Order = 20 )]
        public List<MatchStructureOption> MatchStructureOptions { get; set; } = new List<MatchStructureOption>();


        [G_STJ_SER.JsonPropertyOrder( 21 )]
        [G_NS.JsonProperty( Order = 21 )]
        public List<CourseOfFireOption> CourseOfFireOptions { get; set; } = new List<CourseOfFireOption>();


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
