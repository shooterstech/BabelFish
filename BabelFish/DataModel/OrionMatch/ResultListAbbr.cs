using System.ComponentModel;


namespace Scopos.BabelFish.DataModel.OrionMatch {

    public class ResultListAbbr {

        public ResultListAbbr() {

        }

        /// <summary>
        /// The name of the Result List. Will be unique within the Match.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
        public string ResultName { get; set; }

        /// <summary>
        /// Unique identifier, within this match, for this Result List.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2 )]
        [Obsolete( "ResultName should be unique within a match." )]
        public string ResultListID { get; set; }

        /// <summary>
        /// Boolean, indicating if this Result List is considered one of the primary (or featured) competition results in the match.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 3 )]
        [G_NS.JsonProperty( Order = 3 )]
        public bool Primary { get; set; }

        /// <summary>
        /// Boolean, indicating if this Result List is for a Team competition.
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 4 )]
        [G_NS.JsonProperty( Order = 4 )]
        public bool Team { get; set; }

        /// <summary>
        /// Indicates the completion status of this Result List
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 5 )]
        [G_NS.JsonProperty( Order = 5, DefaultValueHandling = G_NS.DefaultValueHandling.Populate )]
        [DefaultValue( ResultStatus.FUTURE )]
        public ResultStatus Status { get; set; } = ResultStatus.FUTURE;

    }
}
