using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class ResultListAbbr {

        public ResultListAbbr() {

        }

        /// <summary>
        /// The name of the Result List. Will be unique within the Match.
        /// </summary>
        public string ResultName { get; set; }

        /// <summary>
        /// Unique identifier, within this match, for this Result List.
        /// </summary>
        public string ResultListID { get; set; }

        /// <summary>
        /// Boolean, indicating if this Result List is considered one of the primary (or featured) competition results in the match.
        /// </summary>
        public bool Primary { get; set; }

        /// <summary>
        /// Boolean, indicating if this Result List is for a Team competition.
        /// </summary>
        public bool Team { get; set; }

        /// <summary>
        /// Indicates the completion status of this Result List
        /// </summary>
        [JsonConverter( typeof( StringEnumConverter ) )]
        public ResultStatus Status { get; set; } = ResultStatus.FUTURE;

    }
}
