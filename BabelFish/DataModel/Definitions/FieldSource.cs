using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class FieldSource {

        /// <summary>
        /// When the ResultField.Method == Score, Name is the name 
        /// of the Event to pull the participant's score from.
        /// </summary>
        public string Name { get; set; } = string.Empty;


        /// <summary>
        /// When the ResultField.Method == Score, ScoreFormat is the name 
        /// of the Score Format to use. The list of optional Score Format names 
        /// is defined in the Score Format Definition (e.g. v1.0:orion:Standard Score Formats)
        /// </summary>
        public string ScoreFormat { get; set; } = string.Empty;

        /// <summary>
        /// When the ResultField.Method == Gap, Value is the rank of the participant
        /// to comare agaisnt. -1 means the participant directly ahead, VAlues > 0
        /// mean that specific spot (1 being the leader, or 8 being cut to make Final).
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue( 0 )]
        public int Value { get; set; } = 0;
    }
}
