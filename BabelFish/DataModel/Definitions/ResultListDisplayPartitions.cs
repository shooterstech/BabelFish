using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class ResultListDisplayPartitions : IReconfigurableRulebookObject, ICopy<ResultListDisplayPartitions>
    {

        /// <inheritdoc />
        public ResultListDisplayPartitions Copy() {
            ResultListDisplayPartitions rldp = new ResultListDisplayPartitions();
            rldp.Header = this.Header.Copy();
            rldp.Body = this.Body.Copy();
            rldp.Footer = this.Footer.Copy();
            rldp.Children = this.Children.Copy();
            return rldp;
        }

        [JsonProperty( Order = 11 )]
        public ResultListDisplayPartition Header { get; set; } = new ResultListDisplayPartition();

        [JsonProperty( Order = 12 )]
        public ResultListDisplayPartition Body { get; set; } = new ResultListDisplayPartition();

        [JsonProperty( Order = 13 )]
        public ResultListDisplayPartition Children { get; set; } = new ResultListDisplayPartition();

        [JsonProperty( Order = 14 )]
        public ResultListDisplayPartition Footer { get; set; } = new ResultListDisplayPartition();

        /// <inheritdoc/>
        [JsonProperty(Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;
    }
}
