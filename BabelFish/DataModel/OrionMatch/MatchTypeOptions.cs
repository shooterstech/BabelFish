using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum MatchTypeOptions {
        /*
         * Training
Practice Match
Postal Match
Local Match
League Game
Virtual Match
Regional Match
Regional Championship
National Match
National Championship
        */
        /// <summary>
        /// Unknown
        /// </summary>
        [Description( "" )][EnumMember( Value = "" )] UNKNOWN,
        /// <summary>
        /// Training
        /// </summary>
        [Description( "Training" )] [EnumMember( Value = "Training" )] TRAINING,
        /// <summary>
        /// Practice
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
