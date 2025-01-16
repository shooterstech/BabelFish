using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

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
        
        /// <inheritdoc />
        public string NextToken { get; set; } = string.Empty;

        /// <inheritdoc />
        public int Limit { get; set; } = 50;

        /// <inheritdoc />
        public bool HasMoreItems {
            get {
                return !string.IsNullOrEmpty( NextToken );
            }
        }

        public override string ToString() {
            return $"ClubList with {Items.Count} items";
        }
    }
}
