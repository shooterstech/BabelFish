using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        [JsonPropertyOrder ( 1 )]
        public string LeagueID { get; set; } = string.Empty;

        /// <summary>
        /// Human readable name of the league.
        /// </summary>
        [JsonPropertyOrder ( 2 )]
        public string LeagueName { get; set; } = string.Empty;

        /// <summary>
        /// Unigue identifier of the LeagueNetworkID. A value of 0 indicates this League is not a member of a League Network.
        /// </summary>
        [DefaultValue(0)]
        [JsonPropertyOrder ( 3 )]
        public int LeagueNetworkID { get; set; } = 0;

        /// <summary>
        /// The name of the League Network. An empty string indicates this league is not functioning within a League Network.
        /// </summary>
        [JsonPropertyOrder ( 4 )]
        public string LeagueNetworkName { get; set; }

        [DefaultValue( 0 )]
        [JsonPropertyOrder ( 5 )]
        public int SeasonID { get; set; }

        [JsonPropertyOrder ( 6 )]
        public string SeasonName { get; set; }

        [JsonPropertyOrder ( 7 )]
        
        public LeagueSeasonType SeasonType { get; set; }

        [JsonPropertyOrder ( 8 )]
        public LeagueConfiguration Configuration { get; set; }

        [JsonPropertyOrder( 9)]
        public List<string> ConferenceList { get; set; } = new List<string>();

        [JsonPropertyOrder( 10)]
        public List<string> DivisionList { get; set; } = new List<string>();
    }
}
