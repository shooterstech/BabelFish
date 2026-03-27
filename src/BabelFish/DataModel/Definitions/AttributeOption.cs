using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// A suggested <see cref="Attribute"/> that a user could use to construct their own <see cref="CourseOfFireStructure"/>.
    /// </summary>
    public class AttributeOption : IReconfigurableRulebookObject {


        #region Private Variables
        private Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructors and Initialization
        #endregion

        #region Event Handlers

        #endregion

        #region Data Model Properties

        /// <summary>
        /// The attribute definition associated with this option. This property contains the detailed configuration
        /// of the attribute, including its name, type, and other relevant settings.
        /// </summary>
        public SetName AttributeDef { get; set; } = SetName.DEFAULT;

        /// <inheritdoc/>
        [G_NS.JsonProperty( Order = 99 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;
        #endregion

        #region Methods
        #endregion
    }
}
