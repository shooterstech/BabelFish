using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Describes the columns, rows, and cells of the Result List Format's commpiled intermediate format.
    /// </summary>
    public class ResultListFormatDetail : IReconfigurableRulebookObject, ICopy<ResultListFormatDetail>
    {

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

        /// <inheritdoc />
        public ResultListFormatDetail Copy()
        {
            ResultListFormatDetail rlfd = new ResultListFormatDetail();
            //Display and Display for team gonna be weird.
            rlfd.Display = this.Display.Copy();
            rlfd.DisplayForTeam = this.DisplayForTeam.Copy();
            if (this.Columns != null)
            {
                foreach (var rlfdc in this.Columns)
                {
                    rlfd.Columns.Add(rlfdc.Copy());
                }
            }
            return rlfd;
        }

        /// <summary>
        /// Describes the columns of the compiled intermediate format. Including which fields to display, the text in the
        /// header and footer, and when to show or hide each column.
        /// </summary>
        [JsonProperty( Order = 11  )]
        public List<ResultListDisplayColumn> Columns { get; set; }

        /// <summary>
        /// Describes the default header, body, child, and footer rows of the compiled intermediate format. 
        /// </summary>
        [JsonProperty( Order = 12 )]
        public ResultListDisplayPartitions Display {  get; set; }

        /// <summary>
        /// If the Result List that is using this RESULT LIST FORMAT definition is a team event, then DisplayForTeam describes the 
        /// header, body, child, and footer rows of the compiled intermediate format to be used (instead of the default Display property). 
        /// </summary>
        [JsonProperty( Order = 13 )]
        public ResultListDisplayPartitions DisplayForTeam { get; set; }

        /// <inheritdoc/>
        [JsonProperty(Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;
    }
}
