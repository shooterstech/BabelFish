﻿using System;
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

    [JsonConverter( typeof( StringEnumConverter ) ) ]
    public enum LeagueRankingRuleType {

        /// <summary>
        /// Rank teams by their wins and losses
        /// </summary>
        [Description( "Win Loss Record" )]
        [EnumMember( Value = "Win Loss Record" )]
        WIN_LOSS_RECORD,

        /// <summary>
        /// Rank teams by their seasonal average
        /// </summary>
        [Description( "Season Average" )]
        [EnumMember( Value = "Season Average" )]
        SEASON_AVERAGE,

        /// <summary>
        /// Rank teams by their league points, a combination wins and losses with seasonal average
        /// </summary>
        [Description( "League Points" )]
        [EnumMember( Value = "League Points" )]
        LEAGUE_POINTS,

        /// <summary>
        /// Rank teams alphabetically
        /// </summary>
        [Description( "Alphabetical" )]
        [EnumMember( Value = "Alphabetical" )]
        ALPHABETICAL,

        /// <summary>
        /// Rank teams by their hightest score
        /// </summary>
        [Description( "Highest Score" )]
        [EnumMember( Value = "Highest Score" )]
        HIGHEST_SCORE
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

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum MatchAuthorizationRole {
        [Description( "Create Incident Reports" )]
        [EnumMember( Value = "Create Incident Reports" )]
        CREATE_INCIDENT_REPORTS,

        [Description( "Read Incident Reports" )]
        [EnumMember( Value = "Read Incident Reports" )] 
        READ_INCIDENT_REPORTS,

        [Description( "Read Personal Incident Reports" )]
        [EnumMember( Value = "Read Personal Incident Reports" )]
        READ_PERSONAL_INCIDENT_REPORTS,

        [Description( "Update Incident Reports" )]
        [EnumMember( Value = "Update Incident Reports" )]
        UPDATE_INCIDENT_REPORTS,

        [Description( "Close Incident Reports" )]
        [EnumMember( Value = "Close Incident Reports" )]
        CLOSE_INCIDENT_REPORTS,

        [Description( "Create Target Images" )]
        [EnumMember( Value = "Create Target Images" )]
        CREATE_TARGET_IMAGES,

        [Description( "Read Scores" )]
        [EnumMember( Value = "Read Scores" )]
        READ_SCORES,

        [Description( "Read Personal Scores" )]
        [EnumMember( Value = "Read Personal Scores" )]
        READ_PERSONAL_SCORES,

        [Description( "Read Results" )]
        [EnumMember( Value = "Read Results" )]
        READ_RESULTS,

        [Description( "Read Personal Results" )]
        [EnumMember( Value = "Read Personal Results" )]
        READ_PERSONAL_RESULTS,

        [Description( "Read Squadding" )]
        [EnumMember( Value = "Read Squadding" )]
        READ_SQUADDING,

        [Description( "Read Personal Squadding" )]
        [EnumMember( Value = "Read Personal Squadding" )]
        READ_PERSONAL_SQUADDING,

        [Description( "Create Entries" )]
        [EnumMember( Value = "Create Entries" )]
        CREATE_ENTRIES,

        [Description( "Update Entries" )]
        [EnumMember( Value = "Update Entries" )]
        UPDATE_ENTRIES,

        [Description( "Delete Entries" )]
        [EnumMember( Value = "Delete Entries" )]
        DELETE_ENTRIES
    };

    /// <summary>
    /// "FUTURE", "INTERMEDIATE", "UNOFFICIAL", "OFFICIAL
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    [Serializable]
    public enum ResultStatus {
        /// <summary>
        /// The underlying event has not started yet. No scores to report.
        /// </summary>
        [Description("Future")]
        [EnumMember(Value = "FUTURE")]
        FUTURE,

        /// <summary>
        /// The underlying event has started but not yet complete. Only partial scores avalaible to report. The scores reported are the actual scores participants have shot.
        /// </summary>
        [Description("Intermediate")]
        [EnumMember(Value = "INTERMEDIATE")] 
        INTERMEDIATE,

        /// <summary>
        /// The underlying event has completed. All scores are in but not deemed Final yet. Likely a Challenge Period is still in progress.
        /// </summary>
        [Description("Unofficial")]
        [EnumMember(Value = "UNOFFICIAL")]
        UNOFFICIAL,

        /// <summary>
        /// The underlying event has completed, and all scores are final.
        /// </summary>
        [Description("Official")]
        [EnumMember(Value = "OFFICIAL")]
        OFFICIAL
    }
}
