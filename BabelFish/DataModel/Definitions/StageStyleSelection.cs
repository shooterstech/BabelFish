using System.ComponentModel;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// In a COURSE OF FIRE definition, Events may be designated as either a EventType EVENT or STAGE. When doing so, these Events may be mapped to an EVENT STYLE or STAGE STYLE respectively. 
    /// Given the further inputs of Target Collection Name and AttributeValueAppellation a StageStyleMapping maps a StageAppellation to a StageStyle.
    /// </summary>
    public class StageStyleSelection: IReconfigurableRulebookObject, IGetStageStyleDefinition {

        /// <summary>
        /// The Stage's appellation (name) to use when looking up the mapping. Stage appellations are usually common across (printed) rulebooks that have different courses of fire.
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        public string StageAppellation { get; set; } = string.Empty;

        /// <summary>
        /// String formatted as a SetName. The STAGE STYLE definition to use in this mapping.
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        public string StageStyleDef { get; set; } = "v1.0:orion:Default";

        /// <inheritdoc/>
        [G_NS.JsonProperty( Order = 100 )]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;

        /// <inheritdoc/>
        /// <exception cref="XApiKeyNotSetException" ></exception>
        /// <exception cref="ScoposAPIException" ></exception>
        /// <exception cref="DefinitionNotFoundException" ></exception>
        public async Task<StageStyle> GetStageStyleDefinitionAsync() {
            
            var sb = SetName.Parse( StageStyleDef );
            return await DefinitionCache.GetStageStyleDefinitionAsync( sb );
        }

        /// <inheritdoc/>
        public override string ToString() {
            return $"EventStyleSelectin for {StageAppellation}";
        }
    }
}
