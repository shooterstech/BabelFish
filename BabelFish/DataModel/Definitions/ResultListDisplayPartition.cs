using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {
    
    public class ResultListDisplayPartition {

        public ResultListDisplayPartition() {
            RowClass = new List<string>();

            CellDefaultClass = new List<string>();
        }

        public ResultListDisplayPartition( string rowClassDefault, string cellClassDefault ) {
            if (!string.IsNullOrEmpty( rowClassDefault ))
                RowClass.Add( rowClassDefault );

            if (!string.IsNullOrEmpty( cellClassDefault ))
                CellDefaultClass.Add( cellClassDefault );

        }

        [OnDeserialized]
        internal void OnDeserializedMethod( StreamingContext context ) {
            if (RowClass == null)
                RowClass = new List<string>();

            if (CellDefaultClass == null)
                CellDefaultClass = new List<string>();
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

    /// <summary>
    /// The Extended class (I didnt' have a better name for it) includes the 
    /// RowLinkTo option. That doesnt' make sense to have in the Header and Footer rows.
    /// </summary>
    public class ResultListDisplayPartitionExtended : ResultListDisplayPartition {

        public ResultListDisplayPartitionExtended() : base() {
            RowLinkTo = new List<LinkToOption>();
        }

        public ResultListDisplayPartitionExtended( string rowClassDefault, string cellClassDefault ) : base( rowClassDefault, cellClassDefault ) {
            ;
        }

        [OnDeserialized]
        internal void OnDeserializedMethod( StreamingContext context ) {
            if (RowClass == null)
                RowClass = new List<string>();

            if (CellDefaultClass == null)
                CellDefaultClass = new List<string>();

            if (RowLinkTo == null)
                RowLinkTo = new List<LinkToOption>();
        }

        /// <summary>
        /// A list of external pages (abstractly speaking) that this row should provide links to. 
        /// </summary>
        public List<LinkToOption> RowLinkTo { get; set; } = new List<LinkToOption>();
    }
}
