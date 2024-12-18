using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// The Reconfigurable Rulebook Definition types.
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum DefinitionType {
        /*
         * In order to get an Enum to serialize / deserialize to values with spaces have to do a couple of things.
         * First use the EnumMember(Value = "   ") attribute. This is what does the serialzing / deserialzing.
         * In order to get the Descriptions in code, have to use the Description attribute in conjunction with the
         * ExtensionMethod .Description() (Located in the ExtensionMethods.cs class).
        */
        /// <summary>
        /// ATTRIBUTE Definition
        /// </summary>
        [Description( "ATTRIBUTE" )] [EnumMember( Value = "ATTRIBUTE" )] ATTRIBUTE,

        /// <summary>
        /// COURSE OF FIRE Definition
        /// </summary>
        [Description( "COURSE OF FIRE" )] [EnumMember( Value = "COURSE OF FIRE" )] COURSEOFFIRE,

        /// <summary>
        /// EVENT STYLE Definition
        /// </summary>
        [Description( "EVENT STYLE" )] [EnumMember( Value = "EVENT STYLE" )] EVENTSTYLE,

        /// <summary>
        /// EVENT AND STAGE STYLE MAPPING Definition
        /// </summary>
        [Description( "EVENT AND STAGE STYLE MAPPING" )][EnumMember( Value = "EVENT AND STAGE STYLE MAPPING" )] EVENTANDSTAGESTYLEMAPPING,

        /// <summary>
        /// RANKING RULES Definition
        /// </summary>
        [Description( "RANKING RULES" )] [EnumMember( Value = "RANKING RULES" )] RANKINGRULES,

        /// <summary>
        /// RESULT Definition
        /// </summary>
        [Description( "RESULT LIST FORMAT" )] [EnumMember( Value = "RESULT LIST FORMAT" )] RESULTLISTFORMAT,

        /// <summary>
        /// SCORE FORMAT COLLECTION Definition
        /// </summary>
        [Description( "SCORE FORMAT COLLECTION" )] [EnumMember( Value = "SCORE FORMAT COLLECTION" )] SCOREFORMATCOLLECTION,

        /// <summary>
        /// STAGE STYLE Definition
        /// </summary>
        [Description( "STAGE STYLE" )] [EnumMember( Value = "STAGE STYLE" )] STAGESTYLE,

        /// <summary>
        /// TARGET Definition
        /// </summary>
        [Description( "TARGET" )] [EnumMember( Value = "TARGET" )] TARGET,

        /// <summary>
        /// TARGET COLLECTION Definition
        /// </summary>
        [Description( "TARGET COLLECTION" )] [EnumMember( Value = "TARGET COLLECTION" )] TARGETCOLLECTION
    }
}
