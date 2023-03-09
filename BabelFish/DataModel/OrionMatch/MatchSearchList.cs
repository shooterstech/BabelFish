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
            if (ShootingStyles == null)
                ShootingStyles = new List<string>();
        }

        /// <summary>
        /// Distance in miles to search.
        /// The default is 100 miles.
        /// </summary>
        public int Distance { get; set; } = 500;

        /// <summary>
        /// The start date of the match dates to search.
        /// The default value is the first day of the current month.
        /// </summary>
        public String StartDate { get; set; } = String.Empty;

        /// <summary>
        /// The end date of the match dates to search.
        /// The default value is the last day of the current month.
        /// </summary>
        public String EndDate { get; set; } = String.Empty;

        /// <summary>
        /// The shooting style to search or unassigned for all.
        /// </summary>
        public List<string> ShootingStyles { get; set; 
            //Currently choosing not to make this a list of ENUMs, as the possible list of Shooting Styles could grow or might become dynamic.
        } = new List<string>();

        /// <summary>
        /// The Logitude of an area to search.
        /// </summary>
        public double Longitude { get; set; } = -84.5063057;

        /// <summary>
        /// The Latitude of an area to search.
        /// </summary>
        public double Latitude { get; set; } = 38.0394328;

        /// <summary>
        /// The maximum number of search results to return. 
        /// </summary>
        public int Limit { get; set; } = 50;

        public List<MatchAbbr> Items { get; set; }

        /// <inheritdoc />
        public string NextToken { get; set; } = string.Empty;
    }
}
