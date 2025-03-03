using BabelFish.DataModel.Definitions;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Specifies how a row, in a ResultListIntermediatFormat should be decorated for styling.
    /// </summary>
    public class ResultListDisplayPartition : IReconfigurableRulebookObject {

        /// <summary>
        /// Default constructor.
        /// </summary>
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
        /// <remarks>
        /// <list type="bullet">
        /// <listheader>While any css calss values may be used, the common values are:</listheader>
        /// <item>rlf-row-header</item>
        /// <item>rlf-row-athlete</item>
        /// <item>rlf-row-team</item>
        /// <item>rlf-row-child</item>
        /// <item>rlf-row-footer</item>
        /// </list>"
        /// </remarks>
        public List<string> ClassList { get; set; } = new List<string>();

        public List<ClassSet> ClassSet { get; set; } = new List<ClassSet>();

        /// <summary>
        /// The list of css classes to assign to the rows within this Partition.
        /// </summary>
        [Obsolete( "Use .ClassList instead." )]
        public List<string> RowClass { get; set; } = new List<string>();

        /// <inheritdoc/>
        [JsonPropertyOrder( 99 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;
    }
}
