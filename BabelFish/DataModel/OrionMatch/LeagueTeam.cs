using Scopos.BabelFish.Converters;
using Scopos.BabelFish.Converters.Microsoft;
using Scopos.BabelFish.DataModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Describes a Team competing in a League.
    /// </summary>
    public class LeagueTeam {

        /// <summary>
        /// The team id assigned to bye weeks. 
        /// </summary>
        public const int ByeWeekTeamID = 2147483647;

        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {
            if (Schedule == null)
                Schedule = new List<LeagueGame>();
            if (SeasonAverage == null)
                SeasonAverage = new Athena.Score();
            if (Record == null)
                Record = new LeagueRecord();
        }

        public int TeamID { get; set; }

        public string TeamName { get; set; }

        [DefaultValue( "" )]
        public string Hometown { get; set; }

        /// <summary>
        /// The team's orion license number.
        /// </summary>
        [DefaultValue( 0 )]
        [JsonConverter( typeof( DefaultValueHandlingConverter<int> ) )]
        public int LicenseNumber { get; set; }


        [DefaultValue( "" )]
        public string ClubName { get; set; }


        [DefaultValue( "" )]
        public string PhotoURL { get; set; }


        [DefaultValue( "" )]
        public string Conference { get; set; }


        [DefaultValue( "" )]
        public string Division { get; set; }

        public Contact Coach { get; set; }

        public List<LeagueGame> Schedule {get; set; }

        /// <summary>
        /// J is the Integer score average.
        /// K is the Decimal score average.
        /// L is the inner ten average.
        /// </summary>
        public Scopos.BabelFish.DataModel.Athena.Score SeasonAverage { get; set; }

        /// <summary>
        /// Indicates the number of wins, loses, DNSs, DSQs the team has for the current League.
        /// </summary>
        public LeagueRecord Record { get; set; }
    }
}
