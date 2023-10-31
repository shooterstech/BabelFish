using Scopos.BabelFish.DataModel.Athena;
using Scopos.BabelFish.DataModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Describes a Team competing in a League.
    /// </summary>
    public class LeagueTeam {

        /// <summary>
        /// The team id assigned to bye weeks. 
        /// </summary>
        public const int ByeWeekTeamID = 2147483647;

        public int TeamID { get; set; }

        public string TeamName { get; set; }

        [DefaultValue("")]
        public string Hometown { get; set; }

        /// <summary>
        /// The team's orion license number.
        /// </summary>
        [DefaultValue( 0 )]
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
    }
}
