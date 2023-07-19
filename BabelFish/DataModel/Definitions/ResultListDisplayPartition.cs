using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    
    public class ResultListDisplayPartition {

        public ResultListDisplayPartition() { }

        public ResultListDisplayPartition( string rowClassDefault, string cellClassDefault ) {
            if (!string.IsNullOrEmpty( rowClassDefault ))
                RowClass.Add( rowClassDefault );

            if (!string.IsNullOrEmpty( cellClassDefault ))
                CellDefaultClass.Add( cellClassDefault );
        }

        /// <summary>
        /// The list of css classes to assign to the rows within this Partition.
        /// </summary>
        public List<string> RowClass { get; set; } = new List<string>();

        /// <summary>
        /// The list of css classes to assign, by default, to the cells within this partition.
        /// </summary>
        public List<string> CellDefaultClass { get; set; } = new List<string>();
    }
}
