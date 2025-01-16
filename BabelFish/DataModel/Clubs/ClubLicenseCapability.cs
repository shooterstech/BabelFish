using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;


namespace Scopos.BabelFish.DataModel.Clubs {

    /// <summary>
    /// Medea called these LicenseFeature
    /// </summary>
    public enum ClubLicenseCapability {

        /// <summary>
        /// Allows customers to scan and score paper targets with Orion.
        /// </summary>
        [Description( "VIS Scanner" )]
        [EnumMember( Value = "VIS_SCANNER" )]
        VIS_SCANNER,

        /// <summary>
        /// Allows customer to access the online Scorecard app (which is now deprecated).
        /// </summary>
        [Description( "Scorecard" )]
        [EnumMember( Value = "SCORECARD" )]
        SCORECARD,

        /// <summary>
        /// Allows customer to use the password protected CMP upload feature.
        /// </summary>
        [Description( "CMP Internal Competition Tracker Upload" )]
        [EnumMember( Value = "CMP_CT_UPLOAD" )]
        CMP_CT_UPLOAD,

        /// <summary>
        /// Allows customer to access Scopos protected functionality.
        /// </summary>
        [Description( "Privileged" )]
        [EnumMember( Value = "PRIVILEGED" )]
        PRIVILEGED
    };
}
