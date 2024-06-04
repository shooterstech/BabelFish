using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Describes the intermediate format for cells of data within a Result List. 
    /// </summary>
    public class ResultListFormatDetail {

        public ResultListFormatDetail() {
            Display = new ResultListDisplayPartitions();

            Columns = new List<ResultListDisplayColumn>();
        }

        [OnDeserialized]
        internal void OnDeserializedMethod( StreamingContext context ) {
            if (Display == null)
                Display = new ResultListDisplayPartitions();

            if (Columns == null)
                Columns = new List<ResultListDisplayColumn>();
        }

        public ResultListDisplayPartitions Display {  get; set; }

        /// <summary>
        /// If the Result List is a team event, use DisplayForTeam to style the rows. If DisplayForTeam
        /// is null/empty the use .Display.
        /// </summary>
        public ResultListDisplayPartitions DisplayForTeam { get; set; }


        public List<ResultListDisplayColumn> Columns { get; set; }
    }
}
