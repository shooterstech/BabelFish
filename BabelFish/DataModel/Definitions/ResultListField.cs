using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class ResultListField {

        public ResultListField() {
            Source = new FieldSource();
        }

        [OnDeserialized]
        internal void OnDeserializedMethod( StreamingContext context ) {
            
            if (Source == null) 
                Source = new FieldSource();

        }

        /// <summary>
        /// The unique name for this ResultField. Must be unique within the Fields list
        /// of a ResultLitFormat.
        /// FieldNames of Rank, DisplayName, ResultCOFID, and UserID may not be used, as these are always implicitly added.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Specifies the type of data that will be displayed. At a high level where the data for this ResultField is coming from.
        /// </summary>
        [JsonConverter( typeof( StringEnumConverter ) )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.Include )]
        public ResultFieldMethod Method { get; set; }

        /// <summary>
        /// With Method specifying the type of data, Source specified specifically where to pull the data. 
        /// The options for Source, are dependent on the value of Method.
        /// </summary>
        public FieldSource Source { get; set; }

        public override string ToString() {
            return $"{FieldName} for {Method}";
        }
    }
}
