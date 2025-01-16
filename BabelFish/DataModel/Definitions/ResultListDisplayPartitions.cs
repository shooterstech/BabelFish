using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class ResultListDisplayPartitions : IReconfigurableRulebookObject    {


        [OnDeserialized]
        internal void OnDeserializedMethod( StreamingContext context ) {
            if (Header == null)
                Header = new ResultListDisplayPartition();

            if (Body == null)
                Body = new ResultListDisplayPartition();

            if (Children == null)
                Children = new ResultListDisplayPartition();

            if (Footer == null)
                Footer = new ResultListDisplayPartition();
        }

        /// <summary>
        /// Describes the Header row.
        /// </summary>
        public ResultListDisplayPartition Header { get; set; } = new ResultListDisplayPartition();

        /// <summary>
        /// Describes the Body row.
        /// </summary>
        public ResultListDisplayPartition Body { get; set; } = new ResultListDisplayPartition();

        /// <summary>
        /// Describes the Child rows. 
        /// </summary>
        /// <remarks>An example of a Child row, are the team member rows under a team.</remarks>
        public ResultListDisplayPartition Children { get; set; } = new ResultListDisplayPartition();

        /// <summary>
        /// Describes the Footer row.
        /// </summary>
        public ResultListDisplayPartition Footer { get; set; } = new ResultListDisplayPartition();

        /// <inheritdoc/>
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;
    }
}
