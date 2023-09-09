using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class ResultListDisplayPartitions {

        public ResultListDisplayPartition Header { get; set; } = new ResultListDisplayPartition("H_Row", "H_Default");

        public ResultListDisplayPartitionExtended Body { get; set; } = new ResultListDisplayPartitionExtended( "B_Row", "B_Default" );

        public ResultListDisplayPartition Footer { get; set; } = new ResultListDisplayPartition( "F_Row", "F_Default" );

        public ResultListDisplayPartitionExtended Children { get; set; } = new ResultListDisplayPartitionExtended( "C_Row", "C_Default" );
    }
}
