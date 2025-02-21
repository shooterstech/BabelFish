using System.ComponentModel;
using BabelFish.DataModel.Definitions;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// In the Reconfigurable Rulebook Event Composites are defined using the well known Composite Pattern. 
    /// An Event Composite can either be an Event (that has children), or a Singular that does not have children. 
    /// 
    /// <para>An Event defines a real world event in a marksmanship competition. Includes events, stages, series, and strings. 
    /// Does not includes individual shots (as these are Singulars).</para>
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
        /// A unique name given to this Event.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 1 )]
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
		[G_STJ_SER.JsonPropertyOrder( 2 )]
        [G_NS.JsonProperty( Order = 2, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public EventtType EventType { get; set; } = EventtType.NONE;

        /// <summary>
        /// Indicates if the Event's children are derived or explicit
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 3 )]
        [G_NS.JsonProperty( Order = 3, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public EventDerivationType Derivation { get; protected set; } = EventDerivationType.EXPLICIT;

        protected List<string> _children = new List<string>();

        /// <summary>
        /// The children of this event identified by the EventName. The score for this event is added together from the scores of the children.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 4 )]
        [G_NS.JsonProperty( Order = 4 )]
        public virtual List<string> Children { 
            get { return _children; }
            set { throw new NotImplementedException( "Child classes must decide to implemtn Set Children or not." ); }
        }

        /// <summary>
        /// The method to use to calculate the score of this event from the children. Must be one of the following:
        /// * SUM
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 8 )]
        [G_NS.JsonProperty( Order = 8, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public EventCalculation Calculation { get; set; } = EventCalculation.SUM;

        /// <summary>
        /// Additional information needed for the Event Calculation score. The type of data is dependent on the 
        /// Calculation type. 
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 9 )]
        [G_NS.JsonProperty( Order = 9 )]
        [DefaultValue( "" )]
        public string CalculationMeta { get; set; } = string.Empty;

        /// <summary>
        /// The score format to use to display scores for this Event.
        /// The possible values are learned from the Score Format Collection.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 10 )]
        [G_NS.JsonProperty( Order = 10 )]
        public string ScoreFormat { get; set; } = "Events";


        /// <summary>
        /// StageStyleSelection determines how the resulting Result COF is mapped to a STAGE STYLE.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 11 )]
        [G_NS.JsonProperty( Order = 11 )]
        public StageStyleMapping StageStyleMapping { get; set; }

        /// <summary>
        /// EventStyleSelection determines how the resulting Result COF is mapped to a EVENT STYLE.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 12 )]
        [G_NS.JsonProperty( Order = 12 )]
        public EventStyleMapping EventStyleMapping { get; set; }

        /// <summary>
        /// The recommended Result List Format defintion to use when displaying a result list for this Event.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 13 )]
        [G_NS.JsonProperty( Order = 13 )]
        [DefaultValue( "" )]
        //[JsonConverter( typeof( ExcludeEmptyStringConverter ) )]
        public string ResultListFormatDef { get; set; } = string.Empty;

        /// <summary>
        /// The recommended Ranking Rules defintion to use when displaying a ranking list for this Event.
        /// 
        /// EKA NOTE: We may have to make this an object, as depending on the Score Formate (e.g. Integer
        /// vs Decimal) we would have different RankingRuleDef. 
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 14 )]
        [G_NS.JsonProperty( Order = 14 )]
        [DefaultValue("")]
        [Obsolete( "Use RankingRuleMapping instead." )]
        public string RankingRuleDef { get; set; } = string.Empty;

        /// <summary>
        /// A mapping of RankingRuleDef to use to sort scores from this Event, based on the ScoreConfigName.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 15 )]
        [G_NS.JsonProperty( Order = 15 )]
        public RankingRuleMapping RankingRuleMapping { get; set; }

        /// <summary>
        /// Indicates if this Event is outside of the Event Tree.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 16 )]
        [G_NS.JsonProperty( Order = 16 )]
        [DefaultValue( false )]
        public bool ExternalToEventTree { get; set; } = false;

        /// <summary>
        /// If the fields EventName and Values require interpretation, GetCompiledEvents
        /// interpres them and returns a new list of TieBreakingRules cooresponding to the interpretation.
        /// If interpretation is not required, then it returns a list of one tie breaking rule, itself.
        /// <para>Will always return a clone copy of the event, so the original is not modified by the caller.</para>
        /// </summary>
        /// 
        public virtual List<EventExplicit> GetCompiledEvents() {
            var events = new List<EventExplicit>();
            EventExplicit copy = new EventExplicit();
            copy.EventName = this.EventName;
            copy.EventType = this.EventType;
            copy.Children = this.Children;
            copy.Calculation = this.Calculation;
            copy.CalculationMeta = this.CalculationMeta;
            copy.ScoreFormat = this.ScoreFormat;
            copy.ResultListFormatDef = this.ResultListFormatDef;
            copy.StageStyleMapping = this.StageStyleMapping;
            copy.EventStyleMapping = this.EventStyleMapping;  
            copy.RankingRuleMapping = this.RankingRuleMapping;
            copy.ExternalToEventTree = this.ExternalToEventTree;
            if ( this.Derivation == EventDerivationType.EXPLICIT )
                copy.Comment = this.Comment;
            else
                copy.Comment = $"Compiled EventExplicit based on the Event named '{this.EventName}'.";
            
            events.Add( copy );
            return events;
        }

        /// <summary>
        /// Internal documentation comments. All text is ignored by the system.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 100 )]
        [G_NS.JsonProperty( Order = 100 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;

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