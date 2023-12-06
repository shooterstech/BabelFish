using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scopos.BabelFish.Helpers;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum MatchAuthorizationRole {
        [Description( "Create Incident Reports" )]
        [EnumMember( Value = "Create Incident Reports" )]
        CREATE_INCIDENT_REPORTS,

        [Description( "Read Incident Reports" )]
        [EnumMember( Value = "Read Incident Reports" )] 
        READ_INCIDENT_REPORTS,

        [Description( "Read Personal Incident Reports" )]
        [EnumMember( Value = "Read Personal Incident Reports" )]
        READ_PERSONAL_INCIDENT_REPORTS,

        [Description( "Update Incident Reports" )]
        [EnumMember( Value = "Update Incident Reports" )]
        UPDATE_INCIDENT_REPORTS,

        [Description( "Close Incident Reports" )]
        [EnumMember( Value = "Close Incident Reports" )]
        CLOSE_INCIDENT_REPORTS,

        [Description( "Create Target Images" )]
        [EnumMember( Value = "Create Target Images" )]
        CREATE_TARGET_IMAGES,

        [Description( "Read Scores" )]
        [EnumMember( Value = "Read Scores" )]
        READ_SCORES,

        [Description( "Read Personal Scores" )]
        [EnumMember( Value = "Read Personal Scores" )]
        READ_PERSONAL_SCORES,

        [Description( "Read Results" )]
        [EnumMember( Value = "Read Results" )]
        READ_RESULTS,

        [Description( "Read Personal Results" )]
        [EnumMember( Value = "Read Personal Results" )]
        READ_PERSONAL_RESULTS,

        [Description( "Read Squadding" )]
        [EnumMember( Value = "Read Squadding" )]
        READ_SQUADDING,

        [Description( "Read Personal Squadding" )]
        [EnumMember( Value = "Read Personal Squadding" )]
        READ_PERSONAL_SQUADDING,

        [Description( "Create Entries" )]
        [EnumMember( Value = "Create Entries" )]
        CREATE_ENTRIES,

        [Description( "Update Entries" )]
        [EnumMember( Value = "Update Entries" )]
        UPDATE_ENTRIES,

        [Description( "Delete Entries" )]
        [EnumMember( Value = "Delete Entries" )]
        DELETE_ENTRIES
    };
}
