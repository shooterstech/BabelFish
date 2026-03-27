using System.ComponentModel;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class MatchStructureOption : IReconfigurableRulebookObject {

        #region Private Variables
        private Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructors and Initialization
        #endregion

        #region Event Handlers

        #endregion

        #region Data Model Properties

        /// <summary>
        /// A human readable name for this Match Structure Option. This is not intended to be a unique identifier,
        /// but rather a descriptive name that can be used in user interfaces or reports to identify the option.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// A human readable description of this Match Structure Option. This should provide additional details about the option,
        /// such as when it should be used or any special considerations that should be taken into account. Like the Name property,
        /// this is not intended to be a unique identifier, but rather a descriptive field that can be used in user interfaces or reports to provide more context about the option.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The actual match structure associated with this option. This property contains the detailed configuration
        /// of the match structure, including rules, stages, and other relevant settings.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 3 )]
        [G_NS.JsonProperty( Order = 3 )]
        public MatchStructure MatchStructure { get; set; } = new MatchStructure();

        /// <inheritdoc/>
        [G_NS.JsonProperty( Order = 99 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;
        #endregion

        #region Methods
        #endregion
    }
}
