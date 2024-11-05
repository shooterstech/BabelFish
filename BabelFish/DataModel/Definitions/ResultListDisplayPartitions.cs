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

        public ResultListDisplayPartition Header { get; set; } = new ResultListDisplayPartition();

        public ResultListDisplayPartitionExtended Body { get; set; } = new ResultListDisplayPartitionExtended();

        public ResultListDisplayPartition Footer { get; set; } = new ResultListDisplayPartition();

        public ResultListDisplayPartitionExtended Children { get; set; } = new ResultListDisplayPartitionExtended();

        /// <inheritdoc/>
        [JsonProperty(Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;
    }
}
