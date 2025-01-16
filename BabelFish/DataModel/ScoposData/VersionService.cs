using System.Text.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace Scopos.BabelFish.DataModel.ScoposData {
        
    public enum VersionService {

        [Description( "orion" )]
        [EnumMember( Value = "orion" )]
        ORION,

        [Description( "athena" )]
        [EnumMember( Value = "athena" )]
        ATHENA
    }
}
