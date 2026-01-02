using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace Scopos.BabelFish.DataModel.Common
{

    public enum VisibilityOption
    {
        /// <summary>
        /// Only the owner of the data may see this value.
        /// </summary>
        [Description( "Private" )]
        [EnumMember( Value = "Private" )]
        PRIVATE,

        /// <summary>
        /// Only the owner of the data may see this value. However, the value may be used in the calculation of a derived attribute value that is PUBLIC or PROTECTED
        /// </summary>
        [Description( "Internal" )]
        [EnumMember( Value = "Internal" )]
        INTERNAL,

        /// <summary>
        /// Only the owner, and people in a sharing relationship with the owner (e.g. a Coach) may see thsi value.
        /// </summary>
        [Description( "Protected" )]
        [EnumMember( Value = "Protected" )]
        PROTECTED,

        /// <summary>
        /// Globally readable, everyone may see this value.
        /// </summary>
        [Description( "Public" )]
        [EnumMember( Value = "Public" )]
        PUBLIC
    };
}
