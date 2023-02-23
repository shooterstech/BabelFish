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

    /// <summary>
    /// The type of data that is stored within an AttributeField. 
    /// C# Implementations should store and cast data as the following types.
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum ValueType {
        /// <summary>
        /// C# implementations should use DateTime objects. The time portion of the DateTime instance is ignored.
        /// </summary>
        [Description( "DATE" )]
        [EnumMember( Value = "DATE" )]
        DATE,

        /// <summary>
        /// C# implementations should use DateTime objects.
        /// </summary>
        [Description( "DATE TIME" )]
        [EnumMember( Value = "DATE TIME" )]
        DATE_TIME,

        /// <summary>
        /// C# implementations should use TimeSpan objects.
        /// </summary>
        [Description( "TIME" )]
        [EnumMember( Value = "TIME" )]
        TIME,

        /// <summary>
        /// C# implementations should use strings.
        /// </summary>
        [Description( "STRING" )]
        [EnumMember( Value = "STRING" )]
        STRING,

        /// <summary>
        /// C# implementations should use ints.
        /// </summary>
        [Description( "INTEGER" )]
        [EnumMember( Value = "INTEGER" )]
        INTEGER,

        /// <summary>
        /// C# implementations should use floats or doubles.
        /// </summary>
        [Description( "FLOAT" )]
        [EnumMember( Value = "FLOAT" )]
        FLOAT,

        /// <summary>
        /// C# implementations should use bools.
        /// </summary>
        [Description( "BOOLEAN" )]
        [EnumMember( Value = "BOOLEAN" )]
        BOOLEAN,


        /// <summary>
        /// C# implementations should use SetName objects.
        /// </summary>
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
