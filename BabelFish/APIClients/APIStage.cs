using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Scopos.BabelFish.APIClients {

    /// <summary>
    /// Different REST API environments that may be callsed.
    /// </summary>

    public enum APIStage {
        [Description( "alpha" )]
        [EnumMember( Value = "alpha" )]
        ALPHA,

        [Description( "beta" )]
        [EnumMember( Value = "beta" )]
        BETA,

        [Description( "prodtest" )]
        [EnumMember( Value = "prodtest" )]
        PRODTEST,

        [Description( "production" )]
        [EnumMember( Value = "production" )]
        PRODUCTION
    }
}
