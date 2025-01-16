using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;


namespace Scopos.BabelFish.DataModel.Clubs {

    
    public enum ClubLicenseType {

        /// <summary>
        /// Standard Orion for Clubs license.
        /// </summary>
        [Description( "Individual" )]
        [EnumMember( Value = "INDIVIDUAL" )] 
        INDIVIDUAL,

        /// <summary>
        /// Limited functionality Orion for Clubs Home license.
        /// </summary>
        [Description( "Home" )]
        [EnumMember( Value = "HOME" )] 
        HOME,

        /// <summary>
        /// Site license for Orion for Clubs.
        /// </summary>
        [Description( "Site" )]
        [EnumMember( Value = "SITE" )] 
        SITE,

        /// <summary>
        /// Temporary Orion for Clubs license.
        /// </summary>
        [Description( "Temporary" )]
        [EnumMember( Value = "TEMPORARY" )] 
        TEMPORARY 
    };
}
