using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.Clubs {

    /// <summary>
    /// Medea called these LicenseFeature
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum ClubLicenseCapability {

        [Description( "Allows customers to scan and score paper targets with Orion." )]
        [EnumMember( Value = "VIS_SCANNER" )]
        VIS_SCANNER,

        [Description( "Allows customer to access the online Scorecard app (which is now deprecated)." )]
        [EnumMember( Value = "SCORECARD" )]
        SCORECARD,

        [Description( "Allows customer to use the password protected CMP upload feature." )]
        [EnumMember( Value = "CMP_CT_UPLOAD" )]
        CMP_CT_UPLOAD,

        [Description( "Allows customer to access Scopos protected functionality." )]
        [EnumMember( Value = "PRIVILEGED" )]
        PRIVILEGED
    };
}
