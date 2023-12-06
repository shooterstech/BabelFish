using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class ResultListDisplayPartitions {

        public ResultListDisplayPartition Header { get; set; } = new ResultListDisplayPartition();

        public ResultListDisplayPartitionExtended Body { get; set; } = new ResultListDisplayPartitionExtended();

        public ResultListDisplayPartition Footer { get; set; } = new ResultListDisplayPartition();

        public ResultListDisplayPartitionExtended Children { get; set; } = new ResultListDisplayPartitionExtended();
    }
}
