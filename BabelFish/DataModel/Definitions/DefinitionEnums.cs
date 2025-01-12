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


    /// <summary>
    /// The color of the aiming mark.
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum AimingMarkColor {
        /// <summary>
        /// The color white.
        /// </summary>
        WHITE,

        /// <summary>
        /// The color black
        /// </summary>
        BLACK
    }

    /// <summary>
    /// Defines what type of entity an attribute can be assigned to.
    /// </summary>
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

    /// <summary>
    /// The size of barcode labels that should be used for printing. To avoid future name colision, the original product name is used, not the Orion 'small' or 'large' barcode label as used in the product.
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum BarcodeLabelSize { 
        /// <summary>
        /// "Small barcode labels"
        /// </summary>
        OL385, 
        
        /// <summary>
        /// "Large barcode labels"
        /// </summary>
        OL161 
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
        /// Silhouette
        /// </summary>
        [Description( "SILHOUETTE" )]
        [EnumMember( Value = "SILHOUETTE" )]
		SILHOUETTE,

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
        SUGGEST
    }


    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum LightIllumination { NONE, ON, OFF, DIM };


    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum LinkToOption {
        /// <summary>
        /// Indicates that the Cell should link to the ResultCOF Page (sometimes called Individual Score Page). 
        /// </summary>
        [Description( "ResultCOF" )]
        [EnumMember( Value = "ResultCOF" )]
        ResultCOF,

        /// <summary>
        /// Indicates that the Cell should link to the athletes or team's public profile page, if they have one. 
        /// </summary>
        [Description( "PublicProfile" )]
        [EnumMember( Value = "PublicProfile" )]
        PublicProfile,

        /// <summary>
        /// Indicates that the Cell should not link to any page. Which is the default option. 
        /// </summary>
        [Description( "None" )]
        [EnumMember( Value = "None" )]
        None
    }

    /// <summary>
    /// Specifies where the data is coming from for a Result List Field.
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
        /// score is not known, then the absolute score is returned in its place.
        /// </summary>
        [Description( "ProjectedScore" )]
        [EnumMember( Value = "ProjectedScore" )]
        PROJECTED_SCORE,

        /// <summary>
        /// A value from one of the many common fields for the participant. For example Display Name, Country, or Hometown
        /// </summary>
        [Description( "ParticipantAttribute" )]
        [EnumMember( Value = "ParticipantAttribute" )]
        PARTICIPANT_ATTRIBUTE,

        /// <summary>
        /// Value from this participant's Attribute Values
        /// </summary>
        [Description( "Attribute" )]
        [EnumMember( Value = "Attribute" )]
        ATTRIBUTE,

        /// <summary>
        /// The score difference between the current participant and the leader (rank == 1) within the Result List.
        /// </summary>
        [Description( "Gap" )]
        [EnumMember( Value = "Gap" )]
        GAP,

        /// <summary>
        /// Information about any record this score may represent for  the participant. For example "PR" for personal record, 
        /// "RR" for Range Record, "SH" for Season High.
        /// </summary>
        [Description( "Record" )]
        [EnumMember( Value = "Record" )]
        RECORD

    }

    /// <summary>
    /// The Score Components from a Score object.
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum ScoreComponent {
        X,
        I,
        D,
        S,
        J,
        K,
        L
    }

    /// <summary>
    /// Describes the shape of an aiming mark.
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum ScoringShape {
        /// <summary>
        /// Circle
        /// </summary>
        CIRCLE,

        /// <summary>
        /// Square
        /// </summary>
        SQUARE
    }


    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum ShotMappingMethodType { SEQUENTIAL }

    /// <summary>
    /// The type of Boolean operation to apply in a ShowWhenEquation instance.
    /// </summary>

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum ShowWhenBoolean {
        /// <summary>
        /// AND operation
        /// </summary>
        AND,

        /// <summary>
        /// OR operation
        /// </summary>
        OR,

        /// <summary>
        /// XOR operation
        /// </summary>
        XOR,

        /// <summary>
        /// NAND operation
        /// </summary>
        NAND,

        /// <summary>
        /// NOR operation
        /// </summary>
        NOR,

        /// <summary>
        /// NXOR operation
        /// </summary>
        NXOR
    }

    /// <summary>
    /// Conditional variables that evaluate to true or false at runtime, to help decide if a column should be displayed or not.
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum ShowWhenCondition {
        /// <summary>
        /// Always evaluates to true.
        /// </summary>
        TRUE,

        /// <summary>
        /// Always evalutes to false.
        /// </summary>
        FALSE,

        /// <summary>
        /// Evaluates to true when the Result List Intermedaite Format's Result List's Status is FUTURE.
        /// </summary>
        RESULT_STATUS_FUTURE,

        /// <summary>
        /// Evaluates to true when the Result List Intermedaite Format's Result List's Status is INTERMEDIATE.
        /// </summary>
        RESULT_STATUS_INTERMEDIATE,

        /// <summary>
        /// Evaluates to true when the Result List Intermedaite Format's Result List's Status is UNOFFICIAL.
        /// </summary>
        RESULT_STATUS_UNOFFICIAL,

        /// <summary>
        /// Evaluates to true when the Result List Intermedaite Format's Result List's Status is OFFICIAL.
        /// </summary>
        RESULT_STATUS_OFFICIAL,

        /// <summary>
        /// Evaluates to true when the Result List Intermedaite Fromat .ResolutionWidth >= 576.
        /// </summary>
        /// <remarks>Value taken from Bootstrap 5's breakpoints.</remarks>
        DIMENSION_SMALL,

        /// <summary>
        /// Evaluates to true when the Result List Intermedaite Fromat .ResolutionWidth >= 768.
        /// </summary>
        /// <remarks>Value taken from Bootstrap 5's breakpoints.</remarks>
        DIMENSION_MEDIUM,

        /// <summary>
        /// Evaluates to true when the Result List Intermedaite Fromat .ResolutionWidth >= 992.
        /// </summary>
        /// <remarks>Value taken from Bootstrap 5's breakpoints.</remarks>
        DIMENSION_LARGE,

        /// <summary>
        /// Evaluates to true when the Result List Intermedaite Fromat .ResolutionWidth >= 1200.
        /// </summary>
        /// <remarks>Value taken from Bootstrap 5's breakpoints.</remarks>
        DIMENSION_EXTRA_LARGE,

        /// <summary>
        /// Evaluates to true when the Result List Intermedaite Fromat .ResolutionWidth >= 1400.
        /// </summary>
        /// <remarks>Value taken from Bootstrap 5's breakpoints.</remarks>
        DIMENSION_EXTRA_EXTRA_LARGE,

        /// <summary>
        /// Evaluates to true when the Result List Intermedaite Fromat .ResolutionWidth < 576.
        /// </summary>
        /// <remarks>Value taken from Bootstrap 5's breakpoints.</remarks>
        DIMENSION_LT_SMALL,

        /// <summary>
        /// Evaluates to true when the Result List Intermedaite Fromat .ResolutionWidth < 768.
        /// </summary>
        /// <remarks>Value taken from Bootstrap 5's breakpoints.</remarks>
        DIMENSION_LT_MEDIUM,

        /// <summary>
        /// Evaluates to true when the Result List Intermedaite Fromat .ResolutionWidth < 992.
        /// </summary>
        /// <remarks>Value taken from Bootstrap 5's breakpoints.</remarks>
        DIMENSION_LT_LARGE,

        /// <summary>
        /// Evaluates to true when the Result List Intermedaite Fromat .ResolutionWidth < 1200.
        /// </summary>
        /// <remarks>Value taken from Bootstrap 5's breakpoints.</remarks>
        DIMENSION_LT_EXTRA_LARGE,

        /// <summary>
        /// Evaluates to true when the Result List Intermedaite Fromat .ResolutionWidth < 1400.
        /// </summary>
        /// <remarks>Value taken from Bootstrap 5's breakpoints.</remarks>
        DIMENSION_LT_EXTRA_EXTRA_LARGE,

        /// <summary>
        /// Evaluates to true when the Result List Intermedaite Format's Result List's Match's Type is a Local Match.
        /// </summary>
        MATCH_TYPE_LOCAL,

        /// <summary>
        /// Evaluates to true when the Result List Intermedaite Format's Result List's Match's Type is a Virtual Match.
        /// </summary>
        MATCH_TYPE_VIRTUAL,

        /// <summary>
        /// Evaluates to true when the Result List Intermedaite Format's Result List's Match's Type is a Tournament.
        /// </summary>
        MATCH_TYPE_TOURNAMENT,

        /// <summary>
        /// Evaluates to true when the Result List Intermediate Format will be displayed on an interface that is considered user interface engageable. 
        /// </summary>
        ENGAGEABLE,

        /// <summary>
        /// Evaluates to true when the Result List Intermediate Format will be displayed on an interface that is not considered user interface engageable. 
        /// </summary>
        NOT_ENGAGEABLE
    }

    /// <summary>
    /// Concrete class id for a ShowWhenBase. Indicates if the ShowWhenBase abstract class is of concrete class ShowWhenVariable or ShowWhenEquation.
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum ShowWhenOperation {
        /// <summary>
        /// ShowWhenBased class is of concrete type ShowWhenVariable
        /// </summary>
        VARIABLE,

        /// <summary>
        /// ShowWhenBased class is of concrete type ShowWhenEquation
        /// </summary>
        EQUATION,

        /// <summary>
        /// ShowWhenBase class is of concrete type ShowWhenSegmentGroup
        /// </summary>
        SEGMENT_GROUP
    }


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
        BOOLEAN
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
