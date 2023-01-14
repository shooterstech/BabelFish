using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.Clubs {

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum ClubLicenseCapability {

        [Description( "Allows customers to scan and score paper targets with Orion." )]
        [EnumMember( Value = "VISSCANNER" )]
        VISSCANNER,

        [Description( "Allows customer to access the online Scorecard app (which is now deprecated)." )]
        [EnumMember( Value = "SCORECARD" )]
        SCORECARD,

        [Description( "Allows customer to use the password protected CMP upload feature." )]
        [EnumMember( Value = "CMPUPLOAD" )]
        CMPUPLOAD,

        [Description( "Allows customer to access Scopos protected functionality." )]
        [EnumMember( Value = "PRIVILEGED" )]
        PRIVILEGED 
    };
}
