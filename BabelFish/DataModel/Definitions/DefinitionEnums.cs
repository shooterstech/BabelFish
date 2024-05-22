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
    public enum AttributeDesignation {

        [Description( "ATHLETE" )]
        [EnumMember( Value = "ATHLETE" )]
        ATHLETE,


        [Description( "CLUB" )]
        [EnumMember( Value = "CLUB" )] CLUB,


        [Description( "MATCH OFFICIAL" )]
        [EnumMember( Value = "MATCH OFFICIAL" )] MATCH_OFFICIAL,


        [Description( "TEAM" )]
        [EnumMember( Value = "TEAM" )] TEAM,


        [Description( "TEAM OFFICIAL" )]
        [EnumMember( Value = "TEAM OFFICIAL" )] TEAM_OFFICIAL,


        [Description( "USER" )]
        [EnumMember( Value = "USER" )] USER,


        [Description( "HIDDEN" )]
        [EnumMember( Value = "HIDDEN" )] HIDDEN
    }

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum COFTypeOptions { COMPETITION, FORMALPRACTICE, INFORMALPRACTICE, DRILL, GAME };

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum CompetitionType {
        /// <summary>
        /// COMPETITION: Shows only record fire shots
        /// </summary>
        [Description( "COMPETITION" )][EnumMember( Value = "COMPETITION" )] COMPETITION,

        /// <summary>
        /// SIGHTER
        /// </summary>
        [Description( "SIGHTER" )][EnumMember( Value = "SIGHTER" )] SIGHTER,

        /// <summary>
        /// BOTH
        /// </summary>
        [Description( "BOTH" )][EnumMember( Value = "BOTH" )] BOTH
    }

    /// <summary>
    /// Defines the different high level disciplines in use with Shooting. Largely defined by the ISSF.
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum DisciplineType {
        /// <summary>
        /// The Discipline Archery
        /// </summary>
        [Description( "ARCHERY" )][EnumMember( Value = "ARCHERY" )] ARCHERY,

        /// <summary>
        /// The Discipline Biathlon
        /// </summary>
        [Description( "BIATHLON" )][EnumMember( Value = "BIATHLON" )] BIATHLON,

        /// <summary>
        /// Hybrid Discipline, which is when two or more Disciplins are used together.
        /// </summary>
        [Description( "HYBRID" )][EnumMember( Value = "HYBRID" )] HYBRID,

        /// <summary>
        /// The Pistol Discipline
        /// </summary>
        [Description( "PISTOL" )][EnumMember( Value = "PISTOL" )] PISTOL,

        /// <summary>
        /// The Rifle Discipline
        /// </summary>
        [Description( "RIFLE" )][EnumMember( Value = "RIFLE" )] RIFLE,

        /// <summary>
        /// The Running Target Discipline
        /// </summary>
        [Description( "RUNNING TARGET" )][EnumMember( Value = "RUNNING TARGET" )] RUNNINGTARGET,

        /// <summary>
        /// Shotgun Discipline
        /// </summary>
        [Description( "SHOTGUN" )][EnumMember( Value = "SHOTGUN" )] SHOTGUN,

        /// <summary>
        /// Not Applicable
        /// </summary>
        [Description( "NOT APPLICABLE" )][EnumMember( Value = "NOT APPLICABLE" )] NA,

		/// <summary>
		/// Benchrest
		/// </summary>
		[Description( "BENCHREST" )][EnumMember( Value = "BENCHREST" )] BENCHREST
	}


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
    /// The types of Events that exist. This is not meant to be an exhaustive list, but rather a well known list.
    /// NOTE EventtType is purposefully misspelled.
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum EventtType { NONE, EVENT, STAGE, SERIES, STRING, SINGULAR }

    public enum FieldType {
        OPEN,
        CLOSED,
        SUGGEST,
        DERIVED
    }


    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum LightIllumination { NONE, ON, OFF, DIM };


    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum LinkToOption {
        /// <summary>
        /// Indicates that the Cell or Row should link to the ResultCOF Page (sometimes called Individual Score Page). 
        /// </summary>
        [Description( "ResultCOF" )]
        [EnumMember( Value = "ResultCOF" )]
        ResultCOF,

        /// <summary>
        /// Indicates that the Cell or Row should link to the athletes or team's public profile page, if they have one. 
        /// </summary>
        [Description( "PublicProfile" )]
        [EnumMember( Value = "PublicProfile" )]
        PublicProfile,

        /// <summary>
        /// Indicates that the Cell or Row should not link to any other page. Which is the default option. 
        /// </summary>
        [Description( "None" )]
        [EnumMember( Value = "None" )]
        None
    }

    /// <summary>
    /// In a Result List Format, Fields describe the data to dispaly. The ResultFieldMethod
    /// specifies where the data is coming from.
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum ResultFieldMethod {
        /// <summary>
        /// This is the absolute score the Participant has shot.
        /// </summary>
        [Description( "Score" )]
        [EnumMember( Value = "Score" )]
        SCORE,

        /// <summary>
        /// This is the score the Participant is projected to have when they finish. If a Projected
        /// score is nto known, then the absolute score is returned in its place.
        /// </summary>
        [Description( "ProjectedScore" )]
        [EnumMember( Value = "ProjectedScore" )]
        PROJECTED_SCORE,

        [Description( "ParticipantAttribute" )]
        [EnumMember( Value = "ParticipantAttribute" )]
        PARTICIPANT_ATTRIBUTE,

        [Description( "Attribute" )]
        [EnumMember( Value = "Attribute" )]
        ATTRIBUTE
    }


    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum ShotMappingMethodType { SEQUENTIAL }


    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum SingularType {
        [Description( "Shot" )]
        [EnumMember( Value = "Shot" )]
        SHOT,

        [Description( "Test" )]
        [EnumMember( Value = "Test" )]
        TEST
    }

	[JsonConverter( typeof( StringEnumConverter ) )]
	public enum SpecialOptions {
		[Description( "GroupMode" )]
		[EnumMember( Value = "GroupMode" )]
		GROUP_MODE,

		[Description( "ShotCalling" )]
		[EnumMember( Value = "ShotCalling" )]
		SHOT_CALLING
	}


	[JsonConverter( typeof( StringEnumConverter ) )]
    public enum TimerCommandOptions { NONE, START, PAUSE, RESUME, STOP, CLOCK };


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

    /// <summary>
    /// Specifies the method to use to compare two competitors.
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum TieBreakingRuleMethod {

        /// <summary>
        /// Use a value from the Score class.
        /// </summary>
        [Description( "Score" )]
        [EnumMember( Value = "Score" )]
        SCORE,

        /// <summary>
        /// Counts the number of EventType=Singular with Integer score equal to Source.
        /// </summary>
        [Description( "CountOf" )]
        [EnumMember( Value = "CountOf" )]
        COUNT_OF,

        /// <summary>
        /// Use a value from the Participant class
        /// </summary>
        [Description( "ParticipantAttribute" )]
        [EnumMember( Value = "ParticipantAttribute" )]
        PARTICIPANT_ATTRIBUTE,

        /// <summary>
        /// Use a value from the Particiipant's Attributes. Attribute must be a Simple Attribute.
        /// </summary>
        [Description( "Attribute" )]
        [EnumMember( Value = "Attribute" )]
        ATTRIBUTE
    }
}
