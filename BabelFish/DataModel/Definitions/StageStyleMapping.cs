using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// In a COURSE OF FIRE definition, Events may be designated as either a EventType EVENT or STAGE. 
    /// When doing so, these Events may be mapped to an EVENT STYLE or STAGE STYLE respectively. 
    /// An EventStyleSelection or StageStyleSelection define how that mapping is to occur.
    /// </summary>
    public class StageStyleMapping {

        /// <summary>
        /// The default STAGE STYLE to use, if no mapping could be found. 
        /// </summary>
        [JsonProperty( Order = 1 )]
        public string DefaultDef { get; set; } = "v1.0:orion:Default";

        /// <summary>
        /// The Stage's appellation (name) to use when looking up the mapping. Event appellations are usually common across (printed) rulebooks that have different courses of fire.
        /// </summary>
        [JsonProperty(Order = 2)]
        [DefaultValue( "" )]
        public string StageAppellation { get; set; } = string.Empty;
    }
}
