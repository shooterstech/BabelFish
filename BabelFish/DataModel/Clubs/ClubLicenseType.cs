using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.DataModel.Clubs {

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum ClubLicenseType {

        [Description( "Standard Orion for Clubs license." )]
        [EnumMember( Value = "INDIVIDUAL" )] 
        INDIVIDUAL,


        [Description( "Limited functionality Orion for Clubs Home license." )]
        [EnumMember( Value = "HOME" )] 
        HOME,


        [Description( "Site license for Orion for Clubs." )]
        [EnumMember( Value = "SITE" )] 
        SITE,


        [Description( "Temporary Orion for Clubs license." )]
        [EnumMember( Value = "TEMPORARY" )] 
        TEMPORARY 
    };
}
