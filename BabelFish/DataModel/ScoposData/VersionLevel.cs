using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Scopos.BabelFish.DataModel.ScoposData {
    
    public enum VersionLevel {

        [Description( "alpha" )]
        [EnumMember( Value = "alpha" )] 
        ALPHA,

        [Description( "beta" )]
        [EnumMember( Value = "beta" )] 
        BETA,

        [Description( "production" )]
        [EnumMember( Value = "production" )]
        PRODUCTION
    }
}
