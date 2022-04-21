using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BabelFish.DataModel.Definitions {
    public class ShowInSegment  {


        [JsonConverter( typeof( StringEnumConverter ) )]
        public enum CompetitionType {
            /*
             * In order to get an Enum to serialize / deserialize to values with spaces have to do a couple of things.
             * First use the EnumMember(Value = "   ") attribute. This is what does the serialzing / deserialzing.
             * In order to get the Descriptions in code, have to use the Description attribute in conjunction with the
             * ExtensionMethod .Description() (Located in the ExtensionMethods.cs class).
            */
            /// <summary>
            /// COMPETITION: Shows only record fire shots
            /// </summary>
            [Description( "COMPETITION" )] [EnumMember( Value = "COMPETITION" )] COMPETITION,

            /// <summary>
            /// SIGHTER
            /// </summary>
            [Description( "SIGHTER" )] [EnumMember( Value = "SIGHTER" )] SIGHTER,

            /// <summary>
            /// BOTH
            /// </summary>
            [Description( "BOTH" )] [EnumMember( Value = "BOTH" )] BOTH
        }

        private List<string> validationErrorList = new List<string>();

        public ShowInSegment() {

        }

        [DefaultValue(null)]
        public List<string> StageLabel { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public CompetitionType Competition { get; set; }

        /// <summary>
        /// Must be one of the following values
        /// ALL
        /// STRING (default)
        /// Past(n), where n is an integer
        /// </summary>
        [DefaultValue("STRING")]
        public string ShotPresentation { get; set; }

    }
}
