using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class ResultListDisplayPartitions {

        public ResultListDisplayPartition Header { get; set; } = new ResultListDisplayPartition("H_Row", "H_Default");

        public ResultListDisplayPartition Body { get; set; } = new ResultListDisplayPartition( "B_Row", "B_Default" );

        public ResultListDisplayPartition Footer { get; set; } = new ResultListDisplayPartition( "F_Row", "F_Default" );

        public ResultListDisplayPartition Children { get; set; } = new ResultListDisplayPartition( "C_Row", "C_Default" );
    }
}
