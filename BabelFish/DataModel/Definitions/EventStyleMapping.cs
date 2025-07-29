using System.ComponentModel;
using System.Text.Json.Serialization;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// In a COURSE OF FIRE definition, Events may be designated as either a EventType EVENT or STAGE. 
    /// When doing so, these Events may be mapped to an EVENT STYLE or STAGE STYLE respectively. 
    /// An EventStyleSelection or StageStyleSelection define how that mapping is to occur.
    /// </summary>
    public class EventStyleMapping : IReconfigurableRulebookObject, IGetEventStyleDefinition {

        /// <summary>
        /// The default EVENT STYLE to use, if no mapping could be found. 
        /// </summary>
        [JsonPropertyOrder ( 1 )]
        public string DefaultDef { get; set; } = "v1.0:orion:Default";

        /// <summary>
        /// The Event's appellation (name) to use when looking up the mapping. Event appellations are usually common across (printed) rulebooks that have different courses of fire.
        /// </summary>
        [JsonPropertyOrder ( 2 )]
        [DefaultValue( "" )]
        public string EventAppellation { get; set; } = string.Empty;

        /// <inheritdoc/>
        [JsonPropertyOrder ( 99 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

        /// <inheritdoc/>
        /// <exception cref="ArgumentException">Thrown if the value of .DefaultDef could not be parsed. Which shouldn't happen.</exception>
        public async Task<EventStyle> GetEventStyleDefinitionAsync() {

            SetName eventStyleSetName = SetName.Parse( DefaultDef );
            return await DefinitionCache.GetEventStyleDefinitionAsync( eventStyleSetName );

        }
    }
}
