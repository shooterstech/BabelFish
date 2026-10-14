using System.ComponentModel;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// A suggested <see cref="Attribute"/> that a user could use to construct their own <see cref="MatchStructure"/>.
    /// </summary>
    public class AttributeOption : IReconfigurableRulebookObject, IGetAttributeDefinition {


        #region Private Variables
        private Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Constructors and Initialization
        /// <summary>
        /// Public Constructor.
        /// </summary>
        public AttributeOption() { }
        #endregion

        #region Event Handlers

        #endregion

        #region Data Model Properties

        /// <summary>
        /// The attribute definition associated with this option. This property contains the detailed configuration
        /// of the attribute, including its name, type, and other relevant settings.
        /// </summary>
        public SetName AttributeDef { get; set; } = new SetName();

        /// <inheritdoc/>
        [G_NS.JsonProperty( Order = 99 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;
        #endregion

        #region Methods
        /// <inheritdoc/>
        public Task<Attribute> GetAttributeDefinitionAsync() {
            return DefinitionCache.GetAttributeDefinitionAsync( AttributeDef );
        }
        #endregion
    }
}
