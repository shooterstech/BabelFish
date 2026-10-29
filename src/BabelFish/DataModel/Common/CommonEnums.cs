using System.ComponentModel;
using System.Runtime.Serialization;

namespace Scopos.BabelFish.DataModel.Common {

    /// <summary>
    /// Options to set how visible data may be. Commonly used in <seealso cref="AttributeValue.AttributeValue"/> or <seealso cref="Match"/> visibility. 
    /// </summary>
    /// <remarks>
    /// To parse string values into a VisibilityOption, use the <see cref="EnumHelper.ParseVisibilityOption(string)"/> or <see cref="EnumHelper.ParseVisibilityOption(int)"/>
    [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.VisibilityOptionJsonConverter ) )]
    public enum VisibilityOption {
        /*
         * Setting the values for the enum so we can have accurate comparisons, e.g. (PUBLIC > INTERNAL)
         * 
         * When serialized to JSON, the string values should be "Public", "Internal", "Protected", and "Private". This is handled by the VisibilityOptionJsonConverter class.
         */

        /// <summary>
        /// Globally readable, everyone may see this value.
        /// <para>This is the most visible option.</para>
        /// </summary>
        [Description( "Public" )]
        [EnumMember( Value = "Public" )]
        PUBLIC = 4,

        /// <summary>
        /// Only the owner of the data and people in a sharing relationship with the owner (e.g. a Coach) may see this value.
        /// The value may also be used to calculate derived attribute values.
        /// <para>This is the second highest visible option. Only PUBLIC is more visible.</para>
        /// </summary>
        [Description( "Internal" )]
        [EnumMember( Value = "Internal" )]
        INTERNAL = 3,

        /// <summary>
        /// Only the owner of the data and people in a sharing relationship with the owner (e.g. a Coach) may see this value.
        /// <para>This is the second least visible option. Only PRIVATE is less visible.</para>
        /// </summary>
        [Description( "Protected" )]
        [EnumMember( Value = "Protected" )]
        PROTECTED = 2,

        /// <summary>
        /// Only the owner of the data may see this value.
        /// <para>This is the least visible option.</para>
        /// </summary>
        [Description( "Private" )]
        [EnumMember( Value = "Private" )]
        PRIVATE = 1
    };
}
