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
    /// Describes the intermediate format for cells of data within a Result List. 
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

        public ResultListDisplayPartitions Display {  get; set; }

        /// <summary>
        /// If the Result List is a team event, use DisplayForTeam to style the rows. If DisplayForTeam
        /// is null/empty the use .Display.
        /// </summary>
        public ResultListDisplayPartitions DisplayForTeam { get; set; }


        public List<ResultListDisplayColumn> Columns { get; set; }

        /// <inheritdoc/>
        [JsonProperty(Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;
    }
}
