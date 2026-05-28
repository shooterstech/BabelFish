namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Describes an Event in a Match that has one or more Result Lists associated with it.
    /// </summary>
    [Serializable]
    public class ResultEventAbbr : ICheckSum {

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

        /// <inheritdoc />
        [G_NS.JsonIgnore]
        public string CheckSum { get; set; }

        /// <inheritdoc />
        public ulong CalculateChecksum() {
            var combined = $"{EventName}";
            var hash = Helpers.Common.Md5ToUlong( combined );

            foreach (var item in ResultLists) {
                hash = hash ^ item.CalculateChecksum();
            }
            return hash;
        }
    }
}
