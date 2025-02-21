﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class TargetCollectionModal: IReconfigurableRulebookObject {

        /// <summary>
        /// A human readable name for this Target Collection. Will be used by the COURSE OF FIRE to reference this Target Collection.
        ///
        /// Required, may not be an empty string.
        /// </summary>
        [JsonPropertyOrder ( 1 )]
        public string TargetCollectionName { get; set; } = string.Empty;

        /// <summary>
        /// The list of TARGET definitions that compose this Target Collection Modal. 
        /// 
        /// Required, may not be an empty list. All values must be known TARGET definition set names.
        /// </summary>
        [JsonPropertyOrder ( 2 )]
        public List<string> TargetDefs { get; set; } = new List<string>();

        /// <summary>
        /// This Target Collection Modal is intended for use on a range with thie Range Distance.
        /// </summary>
        [JsonPropertyOrder ( 3 )]
        public string RangeDistance { get; set; } = "10m";


        /// <inheritdoc/>
        [JsonPropertyOrder ( 99 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

    }
}
