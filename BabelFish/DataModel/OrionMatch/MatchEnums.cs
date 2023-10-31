using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Specifies the type of season the league is. Preseason, regular season, or postseason
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum LeagueSeasonType {

        /// <summary>
        /// Preseason league
        /// </summary>
        [Description( "Preseason" )]
        [EnumMember( Value = "Preseason" )]
        PRESEASON,

        /// <summary>
        /// Regular season league
        /// </summary>
        [Description( "Regular" )]
        [EnumMember( Value = "Regular" )]
        REGULAR,

        /// <summary>
        /// Postseason League
        /// </summary>
        [Description( "Postseason" )]
        [EnumMember( Value = "Postseason" )]
        POSTSEASTON
    }

    /// <summary>
    /// 
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum LeagueVirtualType {

        /// <summary>
        /// This is a bye week game for the home team. 
        /// </summary>
        [Description( "Bye Week" )]
        [EnumMember( Value = "Bye Week" )]
        BYE_WEEK,

        /// <summary>
        /// Both the home team and away team are competing from their home ranges.
        /// </summary>
        [Description( "Virtual" )]
        [EnumMember( Value = "Virtual" )]
        VIRTUAL,

        /// <summary>
        /// The game is cancelled.
        /// </summary>
        [Description( "Cancelled" )]
        [EnumMember( Value = "Cancelled" )]
        CANCELLED,

        /// <summary>
        /// This is a forced bye week game for the home team. 
        /// </summary>
        [Description( "Forced Bye Week" )]
        [EnumMember( Value = "Forced Bye Week" )]
        FORCED_BYE_WEEK,

        /// <summary>
        /// The game is scheduled, but not yet released to the teams for competition.
        /// </summary>
        [Description( "Not Set" )]
        [EnumMember( Value = "Not Set" )]
        NOT_SET,

        /// <summary>
        /// The game is will competed at the home team's range.
        /// </summary>
        [Description( "Local" )]
        [EnumMember( Value = "Local" )]
        LOCAL
    }


    [JsonConverter( typeof( StringEnumConverter ) )]
	[Serializable]
	public enum MatchTypeOptions {
        /// <summary>
        /// Unknown
        /// </summary>
        [Description( "" )][EnumMember( Value = "" )] UNKNOWN,
        /// <summary>
        /// Training (this value is usually set by Orion)
        /// </summary>
        [Description( "Training" )] [EnumMember( Value = "Training" )] TRAINING,
        /// <summary>
        /// Practice (this value is usually set by Athena)
        /// </summary>
        [Description( "Practice" )][EnumMember( Value = "Practice" )] PRACTICE,
        /// <summary>
        /// Practice Match
        /// </summary>
        [Description( "Practice Match" )][EnumMember( Value = "Practice Match" )] PRACTICE_MATCH,
        /// <summary>
        /// Postal Match
        /// </summary>
        [Description( "Postal Match" )] [EnumMember( Value = "Postal Match" )] POSTAL_MATCH,
        /// <summary>
        /// Local Match
        /// </summary>
        [Description( "Local Match" )][EnumMember( Value = "Local Match" )] LOCAL_MATCH,
        /// <summary>
        /// League Game
        /// </summary>
        [Description( "League Game" )] [EnumMember( Value = "League Game" )] LEAGUE_GAME,
        /// <summary>
        /// League Game
        /// </summary>
        [Description( "Virtual Match" )][EnumMember( Value = "Virtual Match" )] VIRTUAL_MATCH,
        /// <summary>
        /// League Championship
        /// </summary>
        [Description( "League Championship" )] [EnumMember( Value = "League Championship" )] LEAGUE_CHAMPIONSHIP,
        /// <summary>
        /// Regional Match
        /// </summary>
        [Description( "Regional Match" )] [EnumMember( Value = "Regional Match" )] REGIONAL_MATCH,
        /// <summary>
        /// Regional Championship
        /// </summary>
        [Description( "Regional Championship" )] [EnumMember( Value = "Regional Championship" )] REGIONAL_CHAMPIONSHIP,
        /// <summary>
        /// National Match
        /// </summary>
        [Description( "National Match" )] [EnumMember( Value = "National Match" )] NATIONAL_MATCH,
        /// <summary>
        /// National Championship
        /// </summary>
        [Description( "National Championship" )] [EnumMember( Value = "National Championship" )] NATIONAL_CHAMPIONSHIP

    }
}
