using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class ResultListField : IReconfigurableRulebookObject, ICopy<ResultListField>
    {
        /// <summary>
        /// Public constructor
        /// </summary>
        public ResultListField() {
            Source = new FieldSource();
        }

        /// <inheritdoc/>
        public ResultListField Copy() {
            ResultListField rlf = new ResultListField();
            rlf.FieldName = this.FieldName;
            rlf.Method = this.Method;
            rlf.Source = this.Source.Copy();
            return rlf;
        }

        [OnDeserialized]
        internal void OnDeserializedMethod( StreamingContext context ) {
            
            if (Source == null) 
                Source = new FieldSource();

        }

        /// <summary>
        /// The unique name for this ResultField. Must be unique within the Fields list
        /// of a RESULT LIST FORMAT, including the common (pre-defined) fields.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Defines the type of data to be displayed.
        /// </summary>
        [JsonConverter( typeof( StringEnumConverter ) )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.Include )]
        public ResultFieldMethod Method { get; set; }

        /// <summary>
        /// With Method specifying the type of data, Source specified specifically where to pull the data. 
        /// The options for Source, are dependent on the value of Method.
        /// </summary>
        public FieldSource Source { get; set; }

        /// <inheritdoc/>
        public override string ToString() {
            return $"{FieldName} for {Method}";
        }

        /// <inheritdoc/>
        [JsonProperty(Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;
    }
}
