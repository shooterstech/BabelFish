using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BabelFish.DataModel.Definitions;
using NLog;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.DataModel.Definitions
{
    /// <summary>
    /// In the Reconfigurable Rulebook Events are defined using the well known Composite Pattern. 
    /// An Event is either a composite Event, that is made up of child Events, or it is a singular 
    /// Event that is a leaf. Within a COURSE OF FIRE Composite events are defined separately from Singular Events.
    /// </summary>
    public abstract class Event : IReconfigurableRulebookObject, IGetResultListFormatDefinition, IGetRankingRuleDefinition {

        protected Logger Logger = LogManager.GetCurrentClassLogger();

        public Event()
        {
            ScoreFormat = "Events";
            Calculation = EventCalculation.SUM;
            EventType = EventtType.NONE;
        }

        /// <summary>
        /// Indicates if the Event's children are derived or explicit
        /// </summary>
        [JsonPropertyOrder( 1 )]
        public EventDerivationType Derivation { get; protected set; } = EventDerivationType.EXPLICIT;

        /// <summary>
        /// A unique name given to this Event.
        /// </summary>
        [JsonPropertyOrder( 2 )]
        public string EventName { get; set; }

        /// <summary>
        /// The type of event. Must be one of the following:
        /// * NONE
        /// * EVENT
        /// * STAGE
        /// * SERIES
        /// * STRING
        /// * SINGULAR
        /// </summary>
        [JsonPropertyOrder( 3 )]
        public EventtType EventType { get; set; } = EventtType.NONE;

        protected List<string> _children = new List<string>();

        /// <summary>
        /// The children of this event identified by the EventName. The score for this event is added together from the scores of the children.
        /// </summary>
        public virtual List<string> Children { 
            get { return _children; }
            set { throw new NotImplementedException( "Child classes must decide to implemtn Set Children or not." ); }
        }

        /// <summary>
        /// The method to use to calculate the score of this event from the children. Must be one of the following:
        /// * SUM
        /// </summary>
        [JsonPropertyOrder( 10 )]
        public EventCalculation Calculation { get; set; } = EventCalculation.SUM;

        /// <summary>
        /// Additional information needed for the Event Calculation score. The type of data is dependent on the 
        /// Calculation type. 
        /// </summary>
        [DefaultValue("")]
        public string CalculationMeta { get; set; } = string.Empty;

        /// <summary>
        /// The score format to use to display scores for this Event.
        /// The possible values are learned from the Score Format Collection.
        /// </summary>
        [JsonPropertyOrder( 11 )]
        public string ScoreFormat { get; set; } = "Events";


        /// <summary>
        /// StageStyleSelection determines how the resulting Result COF is mapped to a STAGE STYLE.
        /// </summary>
        [JsonPropertyOrder( 11 )]
        public StageStyleMapping StageStyleMapping { get; set; }

        /// <summary>
        /// EventStyleSelection determines how the resulting Result COF is mapped to a EVENT STYLE.
        /// </summary>
        [JsonPropertyOrder( 13 )]
        public EventStyleMapping EventStyleMapping { get; set; }

        /// <summary>
        /// The recommended Result List Format defintion to use when displaying a result list for this Event.
        /// </summary>
        [JsonPropertyOrder( 14 )]
        [DefaultValue( "" )]
        public string ResultListFormatDef { get; set; } = string.Empty;

        /// <summary>
        /// The recommended Ranking Rules defintion to use when displaying a ranking list for this Event.
        /// 
        /// EKA NOTE: We may have to make this an object, as depending on the Score Formate (e.g. Integer
        /// vs Decimal) we would have different RankingRuleDef. 
        /// </summary>
        [JsonPropertyOrder( 15 )]
        [DefaultValue("")]
        [Obsolete( "Use RankingRuleMapping instead." )]
        public string RankingRuleDef { get; set; } = string.Empty;

        /// <summary>
        /// A mapping of RankingRuleDef to use to sort scores from this Event, based on the ScoreConfigName.
        /// </summary>
        [JsonPropertyOrder( 16 )]
        public RankingRuleMapping RankingRuleMapping { get; set; }

        /// <summary>
        /// If the fields EventName and Values require interpretation, GetCompiledEvents
        /// interpres them and returns a new list of TieBreakingRules cooresponding to the interpretation.
        /// If interpretation is not required, then it returns a list of one tie breaking rule, itself.
        /// </summary>
        public virtual List<Event> GetCompiledEvents() {
            return new List<Event>() { this };
        }

        /// <summary>
        /// Internal documentation comments. All text is ignored by the system.
        /// </summary>
        [JsonPropertyOrder( 99 )]
        [DefaultValue("")]
        public string Comment { get; set; }

        public override string ToString() {
            return $"{EventName} of {EventType}";
        }

        /// <inheritdoc />
        /// <exception cref="ScoposAPIException">Thrown if the value for RankingRuleDef is empty, or if the Get Definition call was unsuccessful.</exception>
        public async Task<RankingRule> GetRankingRuleDefinitionAsync() {

            if (string.IsNullOrEmpty( RankingRuleDef ))
                throw new ScoposAPIException( $"The value for RankingRuleDef is empty or null." );

            SetName rrSetName = SetName.Parse( RankingRuleDef );
            return await DefinitionCache.GetRankingRuleDefinitionAsync( rrSetName );
        }

        /// <inheritdoc />
        /// <exception cref="ScoposAPIException">Thrown if the value for ResultListFormatDef is empty, or if the Get Definition call was unsuccessful.</exception>
        public async Task<ResultListFormat> GetResultListFormatDefinitionAsync() {

            if (string.IsNullOrEmpty( ResultListFormatDef ))
                throw new ScoposAPIException( $"The value for ResultListFormatDef is empty or null." );

            SetName rlfSetName = SetName.Parse( ResultListFormatDef );
            return await DefinitionCache.GetResultListFormatDefinitionAsync( rlfSetName );
        }
    }
}