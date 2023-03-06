using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using NLog;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    public class MatchSearchList : ITokenItems<MatchAbbr> {

        private Logger logger = LogManager.GetCurrentClassLogger();


        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {
            if (Items == null)
                Items = new List<MatchAbbr>();
        }

        public List<MatchAbbr> Items { get; set; }

        /// <inheritdoc />
        public string NextToken { get; set; } = string.Empty;
    }
}
