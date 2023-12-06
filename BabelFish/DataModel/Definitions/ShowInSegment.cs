using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class ShowInSegment {



        public ShowInSegment() {

        }

        [OnDeserialized]
        internal new void OnDeserializedMethod( StreamingContext context ) {

            if (StageLabel == null)
                StageLabel = new List<string>();
        }

        public List<string> StageLabel { get; set; } = new List<string>();

        [JsonConverter( typeof( StringEnumConverter ) )]
        public CompetitionType Competition { get; set; }

        /// <summary>
        /// Must be one of the following values
        /// ALL
        /// STRING (default)
        /// Past(n), where n is an integer
        /// </summary>
        [DefaultValue( "STRING" )]
        public string ShotPresentation { get; set; }

    }
}