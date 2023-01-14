using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataModel.Clubs {

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum ClubOptions {

        [Description( "The Club account has enabled their Rezults Team Page" )]
        [EnumMember( Value = "ENABLEWWW" )]
        ENABLEWWW,

        [Description( "The Club has choosen to auto accept Virtual Match invitations." )]
        [EnumMember( Value = "AUTOACCEPT" )]
        AUTOACCEPT,


        [Description( "I'm not entirely sure what we use this for" )]
        [EnumMember( Value = "EMAILNOTIFY" )]
        EMAILNOTIFY,


        [Description( "The Club has requested their POC / SEC contacts be added to Scopos' email list." )]
        [EnumMember( Value = "ADDTOEMAILLIST" )]
        ADDTOEMAILLIST,


        [Description( "The Club has a valid mailing address." )]
        [EnumMember( Value = "VALIDADDRESS" )]
        VALIDADDRESS
    }
}
