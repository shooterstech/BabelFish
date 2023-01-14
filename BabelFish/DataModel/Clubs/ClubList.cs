using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace  Scopos.BabelFish.DataModel.Clubs {
    /// <summary>
    /// A list of Orion Club accounts. Only abbreviated data about the club is returned.
    /// </summary>
    public class ClubList : ITokenItems<ClubAbbr> {

        public ClubList() {
            Items = new List<ClubAbbr>();
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context) {
            if (Items == null)
                Items = new List<ClubAbbr>();
        }

        /// <summary>
        /// A list of ClubAbbr data objects.
        /// </summary>        
        public List<ClubAbbr> Items { get; set; }

        public override string ToString() {
            return $"ClubList with {Items.Count} items";
        }
    }
}
