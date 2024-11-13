using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Specifies how a row, in a ResultListIntermediatFormat should be decorated for styling.
    /// </summary>
    public class ResultListDisplayPartition : IReconfigurableRulebookObject, ICopy<ResultListDisplayPartition>
    {

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

        /// <inheritdoc/>
        public ResultListDisplayPartition Copy()
        {
            ResultListDisplayPartition rldp = new ResultListDisplayPartition();
            if (this.ClassList != null)
            {
                foreach (var cl in this.ClassList)
                {
                    rldp.ClassList.Add(cl);
                }
            }
            //may be unused, but I'd rather copy and not need it.
            if (this.RowClass != null)
            {
                foreach (var cl in this.RowClass)
                {
                    rldp.RowClass.Add(cl);
                }
            }
            return rldp;
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

        /// <summary>
        /// The list of css classes to assign to the rows within this Partition.
        /// </summary>
        [Obsolete( "Use .ClassList instead." )]
        public List<string> RowClass { get; set; } = new List<string>();

        /// <inheritdoc/>
        [JsonProperty(Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;
    }
}
