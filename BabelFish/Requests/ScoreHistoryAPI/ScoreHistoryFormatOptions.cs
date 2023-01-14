using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.Requests.ScoreHistoryAPI {

    /// <summary>
    /// Time span format options when requestion Score History or Score Average.
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum ScoreHistoryFormatOptions {
        /*
         * In order to get an Enum to serialize / deserialize to values with spaces have to do a couple of things.
         * First use the EnumMember(Value = "   ") attribute. This is what does the serialzing / deserialzing.
         * In order to get the Descriptions in code, have to use the Description attribute in conjunction with the
         * ExtensionMethod .Description() (Located in the ExtensionMethods.cs class).
        */
        /// <summary>
        /// Return values for each day
        /// </summary>
        [Description( "DAY" )] [EnumMember( Value = "DAY" )] DAY,

        /// <summary>
        /// Return values for each week
        /// </summary>
        [Description( "WEEK" )] [EnumMember( Value = "WEEK" )] WEEK,

        /// <summary>
        /// Return values for each month
        /// </summary>
        [Description( "MONTH" )] [EnumMember( Value = "MONTH" )] MONTH,

        /// <summary>
        /// Return values for each quarter (three months)
        /// </summary>
        [Description( "QUARTER" )] [EnumMember( Value = "QUARTER" )] QUARTER,

        /// <summary>
        /// Return values for each year
        /// </summary>
        [Description( "YEAR" )] [EnumMember( Value = "YEAR" )] YEAR
    }
}
