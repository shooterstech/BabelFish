using System.ComponentModel;
using System.Runtime.Serialization;


namespace Scopos.BabelFish.DataModel.Clubs {

    public enum ClubAuthorizationRole {
        /*
         * These values should align with the values defined in the sql 'role' table.
         * Prepend each value with 'Club' to avoid confusion with match and global roles in the system.
         */

        /// <summary>
        /// Can do everything a Manager can plus assign the Admin role to other Club members.
        /// </summary>
        [Description( "Club Admin" )]
        [EnumMember( Value = "Club Admin" )]
        ADMIN,

        [Description( "Club Manager" )]
        [EnumMember( Value = "Club Manager" )]
        MANAGER,

        [Description( "Club Member" )]
        [EnumMember( Value = "Club Member" )]
        MEMBER,

        [Description( "Club Coach" )]
        [EnumMember( Value = "Club Coach" )]
        COACH,

        [Description( "Club Payer" )]
        [EnumMember( Value = "Club Payer" )]
        PAYER,

        [Description( "Club Technical Officer" )]
        [EnumMember( Value = "Club Technical Officer" )]
        TECHNICAL_OFFICER
    }

    /// <summary>
    /// Medea called these LicenseFeature
    /// </summary>
    [Obsolete( "This feature is no longer in use as of 2026-04. Also not included on LicenseFiles" )]
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

    public enum ClubLicenseType {

        /// <summary>
        /// Standard Orion for Clubs license.
        /// </summary>
        [Description( "INDIVIDUAL" )]
        [EnumMember( Value = "INDIVIDUAL" )]
        INDIVIDUAL,

        /// <summary>
        /// Limited functionality Orion for Clubs Home license.
        /// </summary>
        [Description( "HOME" )]
        [EnumMember( Value = "HOME" )]
        HOME,

        /// <summary>
        /// Site license for Orion for Clubs.
        /// </summary>
        [Description( "SITE" )]
        [EnumMember( Value = "SITE" )]
        SITE,

        /// <summary>
        /// Temporary Orion for Clubs license.
        /// </summary>
        [Description( "TEMPORARY" )]
        [EnumMember( Value = "TEMPORARY" )]
        TEMPORARY
    };

    public enum ClubOptions {

        /// <summary>
        /// The Club account has enabled their Rezults Team Page
        /// </summary>
        [Description( "ENABLEWWW" )]
        [EnumMember( Value = "ENABLEWWW" )]
        ENABLEWWW,

        /// <summary>
        /// The Club has choosen to auto accept Virtual Match invitations.
        /// </summary>
        [Description( "AUTOACCEPT" )]
        [EnumMember( Value = "AUTOACCEPT" )]
        AUTOACCEPT,

        /// <summary>
        /// I'm not entirely sure what we use this for
        /// </summary>
        [Description( "EMAILNOTIFY" )]
        [EnumMember( Value = "EMAILNOTIFY" )]
        EMAILNOTIFY,

        /// <summary>
        /// The Club has requested their POC / SEC contacts be added to Scopos' email list.
        /// </summary>
        [Description( "ADDTOEMAILLIST" )]
        [EnumMember( Value = "ADDTOEMAILLIST" )]
        ADDTOEMAILLIST,

        /// <summary>
        /// The Club has a valid mailing address.
        /// </summary>
        [Description( "VALIDADDRESS" )]
        [EnumMember( Value = "VALIDADDRESS" )]
        VALIDADDRESS
    }
}
