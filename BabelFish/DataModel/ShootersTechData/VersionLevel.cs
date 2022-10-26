using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ShootersTech.BabelFish.DataModel.ShootersTechData {


    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum VersionLevel {

        [Description( "none" )]
        [EnumMember( Value = "none" )] NONE,

        [Description( "alpha" )]
        [EnumMember( Value = "alpha" )] ALPHA,

        [Description( "beta" )]
        [EnumMember( Value = "beta" )] BETA,

        [Description( "production" )]
        [EnumMember( Value = "production" )]
        PRODUCTION
    }
}
