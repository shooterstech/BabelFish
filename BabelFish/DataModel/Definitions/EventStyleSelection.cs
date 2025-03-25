using System.Text.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// In a COURSE OF FIRE definition, Events may be designated as either a EventType EVENT or STAGE. When doing so, these Events may be mapped to an EVENT STYLE or STAGE STYLE respectively. 
    /// Given the further inputs of Target Collection Name and AttributeValueAppellation a EventStyleMapping maps a EventAppellation to a EventStyle.
    /// </summary>
    public class EventStyleSelection: IReconfigurableRulebookObject, IGetEventStyleDefinition {

        /// <summary>
        /// The Event's appellation (name) to use when looking up the mapping. Event appellations are usually common across (printed) rulebooks that have different courses of fire.
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        public string EventAppellation { get; set; } = string.Empty;

        /// <summary>
        /// String formatted as a SetName. The EVENT STYLE definition to use in this mapping.
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        public string EventStyleDef { get; set; } = "v1.0:orion:Default";

        /// <inheritdoc/>
        [G_NS.JsonProperty( Order = 100 )]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;

        /// <inheritdoc/>
        /// <exception cref="XApiKeyNotSetException" ></exception>
        /// <exception cref="ScoposAPIException" ></exception>
        /// <exception cref="DefinitionNotFoundException" ></exception>
        public async Task<EventStyle> GetEventStyleDefinitionAsync() {

            var sb = SetName.Parse( EventStyleDef );
            return await DefinitionCache.GetEventStyleDefinitionAsync( sb );
        }

        /// <inheritdoc/>
        public override string ToString() {
            return $"EventStyleSelectin for {EventAppellation}";
        }
    }
}
