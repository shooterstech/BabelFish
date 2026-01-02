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

        [Description("greengrassv2Deployment")]
        [EnumMember(Value = "greengrassv2Deployment")]
        GREENGRASSDEPLOYMENT
    }

    public enum ReleasePhase
    {

        [Description("alpha")]
        [EnumMember(Value = "alpha")]
        ALPHA,

        [Description("beta")]
        [EnumMember(Value = "beta")]
        BETA,

        [Description( "integrated" )]
        [EnumMember( Value = "integrated" )]
        INTEGRATED,

        [Description("production")]
        [EnumMember(Value = "production")]
        PRODUCTION
    }
}
