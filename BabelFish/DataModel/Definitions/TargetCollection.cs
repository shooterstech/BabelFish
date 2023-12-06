using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class TargetCollection {


        /// <summary>
        /// A human readable name for this TARGET SET. Will be used by the COURSE OF FIRE to reference this TARGET SET.
        ///
        /// Required, may not be an empty string.
        /// </summary>
        [JsonProperty( Order = 1 )]
        public string TargetCollectionName { get; set; } = string.Empty;

        /// <summary>
        /// The list of TARGET definitions that compose this TARGET SET. 
        /// 
        /// Required, may not be an empty list. All values must be known TARGET definition set names.
        /// </summary>
        [JsonProperty( Order = 2 )]
        public List<string> TargetDefs { get; set; }

        /// <summary>
        /// This TARGET SET is intended for use on a range with thie Range Distance.
        /// </summary>
        [JsonProperty( Order = 3 )]
        public string RangeDistance { get; set; }

    }
}
