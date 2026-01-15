namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Describes an Event in a Match that has one or more Result Lists associated with it.
    /// </summary>
    [Serializable]
    public class ResultEventAbbr {

        /// <summary>
        /// Default public constructor
        /// </summary>
        public ResultEventAbbr() {
            ResultLists = new List<ResultListAbbr>();
        }

        /// <summary>
        /// Human readable name for this ResultEventAbbr.
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        [Obsolete( "Will be removed soon, as almost always DisplayName is equal to EventName." )]
        public string DisplayName { get; set; }

        /// <summary>
        /// The Event Name, as defined in the COURSE OF FIRE definiton, that all 
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        public string EventName { get; set; }

        /// <summary>
        /// A list of Result Lists that are based on scores from this Event.
        /// </summary>
        [G_NS.JsonProperty( Order = 4 )]
        public List<ResultListAbbr> ResultLists { get; set; } = new List<ResultListAbbr>();
    }
}
