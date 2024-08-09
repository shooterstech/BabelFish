using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {
    
    public class ResultListDisplayPartition {

        public ResultListDisplayPartition() {
            RowClass = new List<string>();
        }

        public ResultListDisplayPartition( string rowClassDefault ) {
            if (!string.IsNullOrEmpty( rowClassDefault ))
                RowClass.Add( rowClassDefault );

            if (!string.IsNullOrEmpty( rowClassDefault ))
                ClassList.Add( rowClassDefault );

        }

        [OnDeserialized]
        internal void OnDeserializedMethod( StreamingContext context ) {
            if (RowClass == null)
                RowClass = new List<string>();

            if (ClassList == null)
                ClassList = new List<string>();
        }

        /// <summary>
        /// The list of css classes to assign to the rows within this Partition.
        /// </summary>
        public List<string> ClassList { get; set; } = new List<string>();

        /// <summary>
        /// The list of css classes to assign to the rows within this Partition.
        /// </summary>
        [Obsolete( "Use .ClassList instead." )]
        public List<string> RowClass { get; set; } = new List<string>();
    }

    /// <summary>
    /// The Extended class (I didnt' have a better name for it) includes the 
    /// RowLinkTo option. That doesnt' make sense to have in the Header and Footer rows.
    /// </summary>
    public class ResultListDisplayPartitionExtended : ResultListDisplayPartition {

        public ResultListDisplayPartitionExtended() : base() {
            RowLinkTo = new List<LinkToOption>();
        }

        public ResultListDisplayPartitionExtended( string rowClassDefault ) : base( rowClassDefault ) {
            ;
        }

        [OnDeserialized]
        internal void OnDeserializedMethod( StreamingContext context ) {
            if (RowClass == null)
                RowClass = new List<string>();

            if (ClassList == null)
                ClassList = new List<string>();

            if (RowLinkTo == null)
                RowLinkTo = new List<LinkToOption>();
        }

        /// <summary>
        /// A list of external pages (abstractly speaking) that this row should provide links to. 
        /// </summary>
        public List<LinkToOption> RowLinkTo { get; set; } = new List<LinkToOption>();
    }
}
