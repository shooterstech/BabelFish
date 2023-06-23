using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// In a COURSE OF FIRE definition, Events may be designated as either a EventType EVENT or STAGE. When doing so, these Events may be mapped to an EVENT STYLE or STAGE STYLE respectively. 
    /// Given the further inputs of Target Collection Name and AttributeValueAppellation a EventStyleMapping maps a EventAppellation to a EventStyle.
    /// </summary>
    public class EventStyleSelection {

        /// <summary>
        /// The Event's appellation (name) to use when looking up the mapping. Event appellations are usually common across (printed) rulebooks that have different courses of fire.
        /// </summary>
        public string EventAppellation { get; set; } = string.Empty;

        /// <summary>
        /// String formatted as a SetName. The EVENT STYLE definition to use in this mapping.
        /// </summary>
        public string EventStyleDef { get; set; } = "v1.0:orion:Default";
    }
}
