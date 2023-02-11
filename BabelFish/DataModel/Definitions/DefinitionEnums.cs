using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scopos.BabelFish.Helpers;
using System.ComponentModel;
using System.Runtime.Serialization;

/*
 * This is a single file that contains multiple definitions for enums, all used within the Scopos.BabelFish.DataModel.Definitions namespace
 */

namespace Scopos.BabelFish.DataModel.Definitions {

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum TimerCommandOptions { NONE, START, PAUSE, RESUME, STOP, CLOCK };

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum LightIllumination { NONE, ON, OFF, DIM };


    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum DisplayEventOptions {
        NONE, Default, QualificationPreEvent, QualificationPostEvent, QualificationCallToLine, QualificationRemoveEquipment,
        QualificationStart, QualificationStop, QualificationPreparationPeriodStart, QualificationPreparationPeriodStop, QualificationSightersStart, QualificationSightersStop,
        QualificationStageStart, QualificationStageStop, QualificationTargetChangeStart, QualificationTargetChangeStop, QualificationChangeOverStart, QualificationChangeOverStop,
        QualificationExtraTimeStart, QualificationExtraTimeStop, QualificationUnscheduledCeaseFire, QualificationBoatInImpactArea, QualificationAlibiStart, QualificationAlibiStop,
        FinalPreEvent, FinalPostEvent, FinalCallToLine, FinalRemoveEquipment, FinalCommentary, FinalStart, FinalStop, FinalPreparationPeriodStart, FinalPreparationPeriodStop, FinalSightersStart, FinalSightersStop,
        FinalAthleteIntroductionStart, FinalAthleteIntroductionStop, FinalStageStart, FinalStageStop, FinalEliminationStageStart, FinalEliminationStageStop,
        FinalChangeOverStart, FinalChangeOverStop, FinalAthleteEliminated, FinalThirdPlaceAnnounced, FinalSecondPlaceAnnounced, FinalFirstPlaceAnnounced,
        SpecialEventOne, SpecialEventTwo, SpecialEventThree, SpecialEventFour, SafetyBriefing
    }


    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum ValueType {
        [Description( "DATE" )]
        [EnumMember( Value = "DATE" )]
        DATE,

        [Description( "DATE TIME" )]
        [EnumMember( Value = "DATE TIME" )]
        DATE_TIME,

        [Description( "TIME" )]
        [EnumMember( Value = "TIME" )]
        TIME,

        [Description( "STRING" )]
        [EnumMember( Value = "STRING" )]
        STRING,

        [Description( "INTEGER" )]
        [EnumMember( Value = "INTEGER" )]
        INTEGER,

        [Description( "FLOAT" )]
        [EnumMember( Value = "FLOAT" )]
        FLOAT,

        [Description( "BOOLEAN" )]
        [EnumMember( Value = "BOOLEAN" )]
        BOOLEAN,

        [Description( "SET NAME" )]
        [EnumMember( Value = "SET NAME" )]
        SETNAME
    }

    public enum FieldType {
        OPEN,
        CLOSED,
        SUGGEST,
        DERIVED
    }
}
