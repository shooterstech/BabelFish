using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Scopos.BabelFish.DataModel.ScoposData {


    [JsonConverter( typeof( StringEnumConverter ) )]
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
