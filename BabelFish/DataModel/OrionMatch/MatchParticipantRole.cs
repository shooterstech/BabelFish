using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    [JsonConverter( typeof( StringEnumConverter ) )]
	[Serializable]
	public enum MatchParticipantRole {

        /// <summary>
        /// An athlete or competitor
        /// </summary>
        [Description( "Athlete" )]
        [EnumMember( Value = "Athlete" )] 
        ATHLETE,

        /// <summary>
        /// A coach
        /// </summary>
        [Description( "Coach" )]
        [EnumMember( Value = "Coach" )]
        COACH,

        /// <summary>
        /// A statistical officer
        /// </summary>
        [Description( "Stat Officer" )]
        [EnumMember( Value = "Stat Officer" )]
        STATISTICAL_OFFICER,

        /// <summary>
        /// A range officer
        /// </summary>
        [Description( "Range Officer" )]
        [EnumMember( Value = "Range Officer" )]
        RANGE_OFFICER,

        /// <summary>
        /// Registration
        /// </summary>
        [Description( "Registration" )]
        [EnumMember( Value = "Registration" )]
        REGISTRATION,

        NONE
    }
}
