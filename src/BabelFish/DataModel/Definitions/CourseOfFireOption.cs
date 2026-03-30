using System.ComponentModel;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// A suggested <see cref="CourseOfFire"/> that a user could use to construct their own <see cref="MatchStructure"/>.
    /// </summary>
    public class CourseOfFireOption : IReconfigurableRulebookObject, IGetCourseOfFireDefinition {

        #region Private Variables
        private Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructors and Initialization

        /// <summary>
        /// Public Constructor.
        /// </summary>
        public CourseOfFireOption() { }
        #endregion

        #region Event Handlers

        #endregion

        #region Data Model Properties
        /// <summary>
        /// The course of fire definition associated with this option. This property contains the detailed configuration
        /// of the course of fire, including stages, targets, and other relevant settings.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 3 )]
        [G_NS.JsonProperty( Order = 3 )]
        public SetName CourseOfFireDef { get; set; } = SetName.DEFAULT;

        /// <inheritdoc/>
        [G_NS.JsonProperty( Order = 99 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;
        #endregion

        #region Methods
        /// <inheritdoc/>
        public Task<CourseOfFire> GetCourseOfFireDefinitionAsync() {
            return DefinitionCache.GetCourseOfFireDefinitionAsync( CourseOfFireDef );
        }

        #endregion
    }
}
