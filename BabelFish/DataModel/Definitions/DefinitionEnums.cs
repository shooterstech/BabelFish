using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Helpers;
using System.ComponentModel;
using System.Runtime.Serialization;

/*
 * This is a single file that contains multiple definitions for enums, all used within the Scopos.BabelFish.DataModel.Definitions namespace
 */

namespace Scopos.BabelFish.DataModel.Definitions {


    public enum AbbreviatedFormatDerivedOptions {

        [Description( "LAST(1)" )]
        [EnumMember( Value = "LAST(1)" )]
        LAST_1,

		[Description( "LAST(2)" )]
		[EnumMember( Value = "LAST(2)" )]
		LAST_2,

		[Description( "LAST(3)" )]
		[EnumMember( Value = "LAST(3)" )]
		LAST_3,

		[Description( "LAST(4)" )]
		[EnumMember( Value = "LAST(4)" )]
		LAST_4,

		[Description( "LAST(5)" )]
		[EnumMember( Value = "LAST(5)" )]
		LAST_5,

		[Description( "LAST(6)" )]
		[EnumMember( Value = "LAST(6)" )]
		LAST_6,

        [Description( "LAST(7)" )]
        [EnumMember( Value = "LAST(7)" )]
        LAST_7,

        [Description( "LAST(8)" )]
        [EnumMember( Value = "LAST(8)" )]
        LAST_8,

        [Description( "LAST(9)" )]
        [EnumMember( Value = "LAST(9)" )]
        LAST_9
    }

    /// <summary>
    /// The color of the aiming mark.
    /// </summary>
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
    public enum AttributeDesignation {

        [Description( "ATHLETE" )]
        [EnumMember( Value = "ATHLETE" )]
        ATHLETE,


        [Description( "CLUB" )]
        [EnumMember( Value = "CLUB" )] 
        CLUB,


        [Description( "MATCH OFFICIAL" )]
        [EnumMember( Value = "MATCH OFFICIAL" )] 
        MATCH_OFFICIAL,


        [Description( "TEAM" )]
        [EnumMember( Value = "TEAM" )] 
        TEAM,


        [Description( "TEAM OFFICIAL" )]
        [EnumMember( Value = "TEAM OFFICIAL" )] 
        TEAM_OFFICIAL,


        [Description( "USER" )]
        [EnumMember( Value = "USER" )] 
        USER,


        [Description( "HIDDEN" )]
        [EnumMember( Value = "HIDDEN" )] 
        HIDDEN
    }

    /// <summary>
    /// The size of barcode labels that should be used for printing. To avoid future name colision, the original product name is used, not the Orion 'small' or 'large' barcode label as used in the product.
    /// </summary>
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

    public enum CalculationVariableType {
        INTEGER,
        FLOAT,
        STRING,
        SCORE //Score Component, e.g. I, D, X
    }

    [Obsolete("Use RangeScriptType instead.")]
    public enum COFTypeOptions { 
        COMPETITION, 
        FORMALPRACTICE, 
        INFORMALPRACTICE, 
        DRILL, 
        GAME 
    };

    public enum CompetitionType {
        /// <summary>
        /// COMPETITION: Shows only record fire shots
        /// </summary>
        [Description( "COMPETITION" )]
        [EnumMember( Value = "COMPETITION" )] 
        COMPETITION,

        /// <summary>
        /// SIGHTER
        /// </summary>
        [Description( "SIGHTER" )]
        [EnumMember( Value = "SIGHTER" )] 
        SIGHTER,

        /// <summary>
        /// BOTH
        /// </summary>
        [Description( "BOTH" )]
        [EnumMember( Value = "BOTH" )] 
        BOTH
    }
    public enum DefinitionType {
        /*
         * In order to get an Enum to serialize / deserialize to values with spaces have to do a couple of things.
         * First use the EnumMember(Value = "   ") attribute. This is what does the serialzing / deserialzing.
         * In order to get the Descriptions in code, have to use the Description attribute in conjunction with the
         * ExtensionMethod .Description() (Located in the ExtensionMethods.cs class).
        */
        /// <summary>
        /// ATTRIBUTE Definition
        /// </summary>
        [Description( "ATTRIBUTE" )][EnumMember( Value = "ATTRIBUTE" )] ATTRIBUTE,

        /// <summary>
        /// COURSE OF FIRE Definition
        /// </summary>
        [Description( "COURSE OF FIRE" )][EnumMember( Value = "COURSE OF FIRE" )] COURSEOFFIRE,

        /// <summary>
        /// EVENT STYLE Definition
        /// </summary>
        [Description( "EVENT STYLE" )][EnumMember( Value = "EVENT STYLE" )] EVENTSTYLE,

        /// <summary>
        /// EVENT AND STAGE STYLE MAPPING Definition
        /// </summary>
        [Description( "EVENT AND STAGE STYLE MAPPING" )][EnumMember( Value = "EVENT AND STAGE STYLE MAPPING" )] EVENTANDSTAGESTYLEMAPPING,

        /// <summary>
        /// RANKING RULES Definition
        /// </summary>
        [Description( "RANKING RULES" )][EnumMember( Value = "RANKING RULES" )] RANKINGRULES,

        /// <summary>
        /// RESULT Definition
        /// </summary>
        [Description( "RESULT LIST FORMAT" )][EnumMember( Value = "RESULT LIST FORMAT" )] RESULTLISTFORMAT,

        /// <summary>
        /// SCORE FORMAT COLLECTION Definition
        /// </summary>
        [Description( "SCORE FORMAT COLLECTION" )][EnumMember( Value = "SCORE FORMAT COLLECTION" )] SCOREFORMATCOLLECTION,

        /// <summary>
        /// STAGE STYLE Definition
        /// </summary>
        [Description( "STAGE STYLE" )][EnumMember( Value = "STAGE STYLE" )] STAGESTYLE,

        /// <summary>
        /// TARGET Definition
        /// </summary>
        [Description( "TARGET" )][EnumMember( Value = "TARGET" )] TARGET,

        /// <summary>
        /// TARGET COLLECTION Definition
        /// </summary>
        [Description( "TARGET COLLECTION" )][EnumMember( Value = "TARGET COLLECTION" )] TARGETCOLLECTION
    }

    /// <summary>
    /// Defines the different high level disciplines in use with Shooting. Largely defined by the ISSF.
    /// </summary>
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


    /*
     * IMPORTANT NOTE
     * If one of the enum values in DisplayEventOptions gets updated, then the cooresponding
     * property in EventAssignments also needs to be updated. Located in 
     * Scopos.BabelFish.DataModel.Athena.AbstractEST
     */

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
    /// Indicates how the Score of an Event Composite is calculated.
    /// </summary>
    public enum EventCalculation {
        /// <summary>
        /// Calculate the score of the Event by summing the score of its children
        /// </summary>
        [Description( "SUM" )]
        [EnumMember( Value = "SUM" )]
        SUM,

        /// <summary>
        /// Calculate the score of the Event by taking the average of its children
        /// </summary>
        [Description( "AVERAGE" )]
        [EnumMember( Value = "AVERAGE" )]
        AVERAGE,

        /// <summary>
        /// Deprecated, and kept only for backwards capatibility. Future iterations should specigy the 10 in the CalculationMeta field
        /// </summary>
        [Description( "AVG(10)" )]
        [EnumMember( Value = "AVG(10)" )]
        [Obsolete("Use AVERAGE with CalculationVariables instead.")]
        AVG_TEN,

        /// <summary>
        /// Deprecated, and kept only for backwards capatibility. Future iterations should specigy the i, d in the CalculationMeta field
        /// </summary>
        [Description( "SUM(i, d)" )]
        [EnumMember( Value = "SUM(i, d)" )]
		[Obsolete( "Use SUM with CalculationVariables instead." )]
		ACCUMULATIVE_FINALS,

        /// <summary>
        /// Deprecated, and kept only for backwards capatibility. Future iterations should specigy the i, d in the CalculationMeta field
        /// </summary>
        [Description( "SUM(i,d)" )]
        [EnumMember( Value = "SUM(i,d)" )]
		[Obsolete( "Use SUM with CalculationVariables instead." )]
		ACCUMULATIVE_FINALS_2,

        /// <summary>
        /// Reserved for Singularities that don't have children.
        /// </summary>
        [Description( "NONE" )]
        [EnumMember( Value = "NONE" )]
        NONE
    }

    public enum EventDerivationType {
        /// <summary>
        /// The Children of an Event are definted explicitly by name.
        /// </summary>
        EXPLICIT,

        /// <summary>
        /// The Children of an Event are derived based on the Event's Value Series
        /// </summary>
        DERIVED,

        EXPAND
    }

    /// <summary>
    /// The types of Events that exist. This is not meant to be an exhaustive list, but rather a well known list.
    /// NOTE EventtType is purposefully misspelled.
    /// </summary>
    public enum EventtType { NONE, EVENT, STAGE, SERIES, STRING, ROUND, SINGULAR }

    public enum FieldType {
        OPEN,
        CLOSED,
        SUGGEST
    }


    public enum LightIllumination { NONE, ON, OFF, DIM };


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

    public enum Months {

        /// <summary>
        /// The month of January
        /// </summary>
        [Description( "January" )]
        [EnumMember( Value = "January" )]
        January,

        /// <summary>
        /// The month of February
        /// </summary>
        [Description( "February" )]
        [EnumMember( Value = "February" )]
        February,

        /// <summary>
        /// The month of March
        /// </summary>
        [Description( "March" )]
        [EnumMember( Value = "March" )]
        March,

        /// <summary>
        /// The month of April
        /// </summary>
        [Description( "April" )]
        [EnumMember( Value = "April" )]
        April,

        /// <summary>
        /// The month of May
        /// </summary>
        [Description( "May" )]
        [EnumMember( Value = "May" )]
        May,

        /// <summary>
        /// The month of June
        /// </summary>
        [Description( "June" )]
        [EnumMember( Value = "JanJuneuary" )]
        June,

        /// <summary>
        /// The month of July
        /// </summary>
        [Description( "July" )]
        [EnumMember( Value = "July" )]
        July,

        /// <summary>
        /// The month of August
        /// </summary>
        [Description( "August" )]
        [EnumMember( Value = "August" )]
        August,

        /// <summary>
        /// The month of September
        /// </summary>
        [Description( "September" )]
        [EnumMember( Value = "September" )]
        September,

        /// <summary>
        /// The month of October
        /// </summary>
        [Description( "October" )]
        [EnumMember( Value = "October" )]
        October,

        /// <summary>
        /// The month of November
        /// </summary>
        [Description( "November" )]
        [EnumMember( Value = "November" )]
        November,

        /// <summary>
        /// The month of December
        /// </summary>
        [Description( "December" )]
        [EnumMember( Value = "December" )]
        December

	}

    public enum RangeScriptType {
        /// <summary>
        /// Range Script is designed to be ran by a Range Officer. Usually with multiple 
        /// firiring point receiivng the RS Commands synchornously.
        /// </summary>
        FORMAL_MATCH,

        /// <summary>
        /// Mimics a FORMAL_MATCH but the RS Commands are all automated, and each competitor
        /// can shoot asynchronously. 
        /// </summary>
        FORMAL_PRACTICE,

        /// <summary>
        /// Range Script is designed to allow the marksman to shoot slow fire on a target type
        /// for as long as they want.
        /// </summary>
        INFORMAL_PRACTICE,

        DRILL,

        GAME
    }

    /// <summary>
    /// Mode given to the Result Engine telling it how it should calculate the Rank Delta.
    /// </summary>
	public enum ResultEngineCompareType {
		NONE,
		NOW,
		WINDOW_1_MINUTE,
		WINDOW_3_MINUTE,
		WINDOW_5_MINUTE
	}

	/// <summary>
	/// Specifies where the data is coming from for a Result List Field.
	/// </summary>
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
    public enum ScoringShape {
        /// <summary>
        /// Circle
        /// </summary>
        CIRCLE,

        /// <summary>
        /// Square
        /// </summary>
        SQUARE,

        SVG
    }


    public enum ShotMappingMethodType { SEQUENTIAL }

    /// <summary>
    /// The type of Boolean operation to apply in a ShowWhenEquation instance.
    /// </summary>

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
        /// Evaluates to true when the Result List Intermedaite Fromat .ResolutionWidth >= 325.
        /// </summary>
        /// <remarks>Value made up. As sometimes we need screen resolution less than a bootstrap SMALL.</remarks>
        DIMENSION_EXTRA_SMALL,

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
        /// Evaluates to true when the Result List report scores were shot on ESTs.
        /// </summary>
        /// <remarks>In a Virtual Match, this evalutes to true if one or more of the VM locations was shot on ESTs.</remarks>
        SHOT_ON_EST,

        /// <summary>
        /// Evalutes to true when the Result List reports scores were shot on paper targets.
        /// </summary>
        /// <remarks>In a Virtual Match, this evalutes to true if one or more of the VM locations was shot on Paper.</remarks>
        SHOT_ON_PAPER,

        /// <summary>
        /// Evaluates to true when the Result List Intermediate Format will be displayed on an interface that is considered user interface engageable. 
        /// </summary>
        ENGAGEABLE,

        /// <summary>
        /// Evaluates to true when the Result List Intermediate Format will be displayed on an interface that is not considered user interface engageable. 
        /// </summary>
        NOT_ENGAGEABLE,

		/// <summary>
		/// Evaluates to true when the Result List Intermediate Format will be displayed on an interface that the user does want to see "supplemental" information. 
		/// <para>Websites will generally have supplemental information turned on. Spectator displays and print outs generally do not.</para>
		/// </summary>
		SUPPLEMENTAL,

        /// <summary>
        /// Evaluates to true when the Result List Intermediate Format will be displayed on an interface that the user does not want to see "supplemental" information. 
        /// </summary>
        NOT_SUPPLEMENTAL,

        /// <summary>
        /// Evaluates to true on a column when any participant in the Result List has a shown Remark. In a row, evaluates to true with the participant has any Remark.
        /// </summary>
        HAS_ANY_SHOWN_REMARK,
        
        /// <summary>
        /// Evaluates to true when the Participant within the Result List has the Remark of Did Not Shoot
        /// </summary>
        HAS_SHOWN_REMARK_DNS,

        /// <summary>
        /// Evaluates to true when the Participant within the Result List has the Remark of Did Not Finish
        /// </summary>
        HAS_SHOWN_REMARK_DNF,

        /// <summary>
        /// Evaluates to true when the Participant within the Result List has the Remark of Disqualified
        /// </summary>
        HAS_SHOWN_REMARK_DSQ,

        /// <summary>
        /// Evaluates to true when the Participant within the Result List has the Remark of Bubble
        /// </summary>
        HAS_SHOWN_REMARK_BUBBLE,

        /// <summary>
        /// Evaluates to true when the Participant within the Result List has the Remark of Eliminated
        /// </summary>
        HAS_SHOWN_REMARK_ELIMINATED,

        /// <summary>
        /// Evaluates to true when the Participant within the Result List has the Remark of Qualified
        /// </summary>
        HAS_SHOWN_REMARK_QUALIFIED,

        /// <summary>
        /// Evaluates to true when the Participant within the Result List has the Remark of Qualified
        /// </summary>
        HAS_SHOWN_REMARK_FIRST,

        /// <summary>
        /// Evaluates to true when the Participant within the Result List has the Remark of Qualified
        /// </summary>
        HAS_SHOWN_REMARK_SECOND,

        /// <summary>
        /// Evaluates to true when the Participant within the Result List has the Remark of Qualified
        /// </summary>
        HAS_SHOWN_REMARK_THIRD,

        /// <summary>
        /// Evaluates to true when the Participant within the Result List has the Remark of Qualified
        /// </summary>
        HAS_SHOWN_REMARK_LEADER
    }

    /// <summary>
    /// Concrete class id for a ShowWhenBase. Indicates if the ShowWhenBase abstract class is of concrete class ShowWhenVariable or ShowWhenEquation.
    /// </summary>
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

    public enum TargetModel {
        SCORING_RING,
        HIT_MISS,
        TEST
    }

    public enum SingularType {
        [Description( "Shot" )]
        [EnumMember( Value = "Shot" )]
        SHOT,

        [Description( "Test" )]
        [EnumMember( Value = "Test" )]
        TEST
    }

    /// <summary>
    /// Specifies the method to use to compare two competitors.
    /// </summary>
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

    public enum TieBreakingRuleParticipantAttributeSource {
        FamilyName,
        GivenName,
        MiddleName,
        CompetitorNumber,
        DisplayName,
        DisplayNameShort,
        HomeTown,
        Country,
        Club
    }

    public enum TieBreakingRuleScoreSource {
        /// <summary>
        /// Use the integer score
        /// </summary>
        I,

        /// <summary>
        /// Use the decimal score
        /// </summary>
        D,

        /// <summary>
        /// Use the special sum rulebook score
        /// </summary>
        S,

        /// <summary>
        /// Use the inner ten score
        /// </summary>
        X,

        /// <summary>
        /// Use the integer socre, and if still tied then use the inner ten score
        /// </summary>
        IX,

        /// <summary>
        /// Use the special use case J score
        /// </summary>
        J,

        /// <summary>
        /// Use the special use case K score
        /// </summary>
        K,

        /// <summary>
        /// Use the special use case L score
        /// </summary>
        L
    }


    public enum TimerCommandOptions { 
        /// <summary>
        /// Does not effect the Range Timer
        /// </summary>
        NONE, 

        /// <summary>
        /// Starts the Range Timer
        /// </summary>
        START, 

        /// <summary>
        /// Pauses the Range Timer
        /// </summary>
        PAUSE, 

        /// <summary>
        /// Resumes the Range Timer after a pause.
        /// </summary>
        RESUME, 

        /// <summary>
        /// Stops the Range Timer
        /// </summary>
        STOP, 

        /// <summary>
        /// Sets the Range Timer to act as a Clock
        /// </summary>
        CLOCK, 

        /// <summary>
        /// To be filled in by Ben. 
        /// </summary>
        SEGMENT 
    };


    /// <summary>
    /// The type of data that is stored within an AttributeField. 
    /// C# Implementations should store and cast data as the following types.
    /// </summary>
    public enum ValueType {
        /// <summary>
        /// C# implementations should use DateTime objects. The time portion of the DateTime instance is ignored.
        /// </summary>
        [Description( "DATE" )]
        [EnumMember( Value = "DATE" )]
        DATE,

        /// <summary>
        /// C# implementations should use DateTime objects. The time portion of the DateTime instance is ignored.
        /// </summary>
        [Description( "DATE TIME" )]
        [EnumMember( Value = "DATE TIME" )]
        DATE_TIME,

        /// <summary>
        /// C# implementations should use TimeSpan objects.
        /// </summary>
        [Description( "TIME SPAN" )]
        [EnumMember( Value = "TIME SPAN" )]
        TIME_SPAN,

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
}
