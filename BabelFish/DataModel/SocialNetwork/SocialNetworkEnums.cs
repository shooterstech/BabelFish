using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Runtime.Serialization;
using System.Text.Json;


namespace Scopos.BabelFish.DataModel.SocialNetwork
{

    /// <summary>
    /// The Reconfigurable Rulebook Definition types.
    /// </summary>
    public enum SocialRelationshipName
    {
        /*
         * In order to get an Enum to serialize / deserialize to values with spaces have to do a couple of things.
         * First use the EnumMember(Value = "   ") attribute. This is what does the serialzing / deserialzing.
         * In order to get the Descriptions in code, have to use the Description attribute in conjunction with the
         * ExtensionMethod .Description() (Located in the ExtensionMethods.cs class).
        */
        /// <summary>
        /// Follows relationship
        /// </summary>
        [Description("Follow")][EnumMember(Value = "Follow")] FOLLOW,

        /// <summary>
        /// Blocks relationship
        /// </summary>
        [Description("Block")][EnumMember(Value = "Block")] BLOCK,

        /// <summary>
        /// Coaches relationship
        /// </summary>
        [Description("Coach")][EnumMember(Value = "Coach")] COACH,
    }
}
