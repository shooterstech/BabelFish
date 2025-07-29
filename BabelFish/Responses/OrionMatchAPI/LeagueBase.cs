﻿using System.ComponentModel;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    /*
     * Choosing to keep LeagueBase in the Scopos.BabelFish.Responses.OrionMatchAPI namespace, instead of
     * Scopos.BabelFish.DataModel.OrionMatch, as these properties are mostly 'helpeer' properties that the 
     * API includes. And do not describe unique data model values.
     * 
     * EKA, Dec 2023
     */

    /// <summary>
    /// LeagueBase contains common properties that many Get League xyz calls have in common.
    /// </summary>
    public abstract class LeagueBase {

        /// <summary>
        /// Unique LeagueID for this League. Takes the form of a MatchID
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        public string LeagueID { get; set; } = string.Empty;

        /// <summary>
        /// Human readable name of the league.
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        public string LeagueName { get; set; } = string.Empty;

        /// <summary>
        /// Unigue identifier of the LeagueNetworkID. A value of 0 indicates this League is not a member of a League Network.
        /// </summary>
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.DefaultValueHandlingConverter<int> ) )]
        [G_NS.JsonProperty( Order = 3 )]
        [DefaultValue( 0 )]
        public int LeagueNetworkID { get; set; } = 0;

        /// <summary>
        /// The name of the League Network. An empty string indicates this league is not functioning within a League Network.
        /// </summary>
        [G_NS.JsonProperty( Order = 4 )]
        public string LeagueNetworkName { get; set; }

        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.DefaultValueHandlingConverter<int> ) )]
        [G_NS.JsonProperty( Order = 5 )]
        [DefaultValue( 0 )]
        public int SeasonID { get; set; }

        [G_NS.JsonProperty( Order = 6 )]
        public string SeasonName { get; set; }

        [G_NS.JsonProperty( Order = 7 )]

        public LeagueSeasonType SeasonType { get; set; }

        [G_NS.JsonProperty( Order = 8 )]
        public LeagueConfiguration Configuration { get; set; }

        [G_NS.JsonProperty( Order = 9 )]
        public List<string> ConferenceList { get; set; } = new List<string>();

        [G_NS.JsonProperty( Order = 10 )]
        public List<string> DivisionList { get; set; } = new List<string>();
    }
}
