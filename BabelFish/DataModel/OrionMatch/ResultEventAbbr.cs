using System;
using System.Runtime.Serialization;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Describes an Event in a Match that has one or more Result Lists associated with it.
    /// </summary>
    [Serializable]
    public class ResultEventAbbr {

        public ResultEventAbbr() {
            ResultLists = new List<ResultListAbbr>();
        }

        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {
            if (ResultLists == null)
                ResultLists = new List<ResultListAbbr>();
        }

        public string DisplayName { get; set; }
        
        /// <summary>
        /// A list of Result Lists that are based on scores from this Event.
        /// </summary>
        public List<ResultListAbbr> ResultLists { get; set; }

        public string EventName { get; set; }
    }
}
