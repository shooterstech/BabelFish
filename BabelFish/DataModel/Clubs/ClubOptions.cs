using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json;

using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataModel.Clubs {

    
    public enum ClubOptions {

        /// <summary>
        /// The Club account has enabled their Rezults Team Page
        /// </summary>
        [Description( "ENABLEWWW" )]
        [EnumMember( Value = "ENABLEWWW" )]
        ENABLEWWW,

        /// <summary>
        /// The Club has choosen to auto accept Virtual Match invitations.
        /// </summary>
        [Description( "AUTOACCEPT" )]
        [EnumMember( Value = "AUTOACCEPT" )]
        AUTOACCEPT,

        /// <summary>
        /// I'm not entirely sure what we use this for
        /// </summary>
        [Description( "EMAILNOTIFY" )]
        [EnumMember( Value = "EMAILNOTIFY" )]
        EMAILNOTIFY,

        /// <summary>
        /// The Club has requested their POC / SEC contacts be added to Scopos' email list.
        /// </summary>
        [Description( "ADDTOEMAILLIST" )]
        [EnumMember( Value = "ADDTOEMAILLIST" )]
        ADDTOEMAILLIST,

        /// <summary>
        /// The Club has a valid mailing address.
        /// </summary>
        [Description( "VALIDADDRESS" )]
        [EnumMember( Value = "VALIDADDRESS" )]
        VALIDADDRESS
    }
}
