using System.Text.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace Scopos.BabelFish.DataModel.ScoposData {
        
    public enum ApplicationName
    {

        [Description( "orion" )]
        [EnumMember( Value = "orion" )]
        ORION,

        [Description( "athena" )]
        [EnumMember( Value = "athena" )]
        ATHENA,

        [Description("greengrass")]
        [EnumMember(Value = "greengrass")]
        GREENGRASS
    }

    public enum ReleasePhase
    {

        [Description("alpha")]
        [EnumMember(Value = "alpha")]
        ALPHA,

        [Description("beta")]
        [EnumMember(Value = "beta")]
        BETA,

        [Description("production")]
        [EnumMember(Value = "production")]
        PRODUCTION
    }
}
