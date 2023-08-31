using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// In a COURSE OF FIRE definition, Events may be designated as either a EventType EVENT or STAGE. When doing so, these Events may be mapped to an EVENT STYLE or STAGE STYLE respectively. 
    /// Given the further inputs of Target Collection Name and AttributeValueAppellation a StageStyleMapping maps a StageAppellation to a StageStyle.
    /// </summary>
    public class StageStyleSelection {

        /// <summary>
        /// The Stage's appellation (name) to use when looking up the mapping. Stage appellations are usually common across (printed) rulebooks that have different courses of fire.
        /// </summary>
        public string StageAppellation { get; set; } = string.Empty;

        /// <summary>
        /// String formatted as a SetName. The STAGE STYLE definition to use in this mapping.
        /// </summary>
        public string StageStyleDef { get; set; } = "v1.0:orion:Default";
    }
}
