using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace ShootersTech.BabelFish.DataModel.ShootersTechData {


    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum VersionService {

        [Description( "none" )]
        [EnumMember( Value = "none" )]
        NONE,

        [Description( "orion" )]
        [EnumMember( Value = "orion" )]
        ORION,

        [Description( "athena" )]
        [EnumMember( Value = "athena" )]
        ATHENA
    }
}
