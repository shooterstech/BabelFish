using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class ResultListDisplayPartitions : IReconfigurableRulebookObject, ICopy<ResultListDisplayPartitions>
    {


        [OnDeserialized]
        internal void OnDeserializedMethod( StreamingContext context ) {
            if (Header == null)
                Header = new ResultListDisplayPartition();

            if (Body == null)
                Body = new ResultListDisplayPartition();

            if (Children == null)
                Children = new ResultListDisplayPartition();

            if (Footer == null)
                Footer = new ResultListDisplayPartition();
        }

        /// <inheritdoc />
        public ResultListDisplayPartitions Copy() {
            ResultListDisplayPartitions rldp = new ResultListDisplayPartitions();
            rldp.Header = this.Header.Copy();
            rldp.Body = this.Body.Copy();
            rldp.Footer = this.Footer.Copy();
            rldp.Children = this.Children.Copy();
            return rldp;
        }

        /// <summary>
        /// Describes the Header row.
        /// </summary>
        [JsonProperty( Order = 11 )]
        public ResultListDisplayPartition Header { get; set; } = new ResultListDisplayPartition();

        /// <summary>
        /// Describes the Body row.
        /// </summary>
        [JsonProperty( Order = 12 )]
        public ResultListDisplayPartition Body { get; set; } = new ResultListDisplayPartition();

        /// <summary>
        /// Describes the Child rows. 
        /// </summary>
        /// <remarks>An example of a Child row, are the team member rows under a team.</remarks>
        [JsonProperty( Order = 13 )]
        public ResultListDisplayPartition Children { get; set; } = new ResultListDisplayPartition();

        /// <summary>
        /// Describes the Footer row.
        /// </summary>
        [JsonProperty( Order = 14 )]
        public ResultListDisplayPartition Footer { get; set; } = new ResultListDisplayPartition();

        /// <inheritdoc/>
        [JsonProperty(Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;
    }
}
