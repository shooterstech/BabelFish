using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class CourseOfFireOption : IReconfigurableRulebookObject {

        #region Private Variables
        private Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructors and Initialization
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
        #endregion
    }
}
