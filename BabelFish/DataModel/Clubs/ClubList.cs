using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace  ShootersTech.BabelFish.DataModel.Clubs {
    /// <summary>
    /// A list of Orion Club accounts. Only abbreviated data about the club is returned.
    /// </summary>
    public class ClubList {

        public ClubList() {
            Items = new List<ClubAbbr>();
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context) {
            if (Items == null)
                Items = new List<ClubAbbr>();
        }

        /// <summary>
        /// Returned, when the list of Clubs is too long. Use this value on the next call to return more of the complete list.An empty string means there are no more items.
        /// </summary>
        public string NextToken { get; set; }

        public List<ClubAbbr> Items { get; set; }
    }
}
