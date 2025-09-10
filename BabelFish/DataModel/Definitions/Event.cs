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
    public abstract class Event : IReconfigurableRulebookObject, IGetResultListFormatDefinition, IGetRankingRuleDefinition, IGetRankingRuleDefinitionList {

        protected static Logger Logger = LogManager.GetCurrentClassLogger();

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
            
		    set {
                // Child classes must decide to implemtn Set Children or not.
                // Making the set operation a no-op to avoid unexpected exceptions.
                Logger.Warn( $"Set Children unexpectedly called for '{EventName},' an Event of Derivation {Derivation}." );
			}
        }

		/// <summary>
		/// The method to use to calculate the score of this event from the children. Must be one of the following:
		/// * SUM  (may have CalculationVariables of type CalculationVariablesString)
		/// * AVERAGE (may have CalculationVariables of type CalculationVariableInteger)
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>SUM: CalculationVariables are used to determine how the “S” the special sum score component is derived. 
		/// If there are 0 CalculationVariables S is calculated by the sum of each child event’s S. If there are more then 1, 
		/// then there should be one CalculationVariable for each child, and must be of VariableType SCORE. For example, 
		/// if the values are “I”, and then “D”, it means S is calculated by taking the I (integer) component of the first 
		/// child’s score, plus the D (decimal) component of the second child’s score.
		/// </item>
		/// <item>
		/// AVERAGE: Must be one CalculationVariable, of VariableType INTEGER. Used to specify the number of shots in a 
        /// series to calculate the average to. For example, if the value is 10, then the Event Score is [average shot score] * 10. 
		/// </item>
		/// </list>
		/// </remarks>
		[G_STJ_SER.JsonPropertyOrder( 8 )]
        [G_NS.JsonProperty( Order = 8, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        public EventCalculation Calculation { get; set; } = EventCalculation.SUM;

        /// <summary>
        /// Additional information needed for the Event Calculation score. The type of data is dependent on the 
        /// Calculation type. 
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 9 )]
        [G_NS.JsonProperty( Order = 9 )]
        public List<CalculationVariable> CalculationVariables { get; set; } = new List<CalculationVariable>();

        public bool ShouldSerializeCalculationVariables() {
            return CalculationVariables != null && CalculationVariables.Count > 0;  
        }

        /// <summary>
        /// The score format to use to display scores for this Event.
        /// The possible values are learned from the Score Format Collection.
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 10 )]
        [G_NS.JsonProperty( Order = 10 )]
        public string ScoreFormat { get; set; } = "Events";


        /// <summary>
        /// StageStyleSelection determines how the resulting Result COF is mapped to a STAGE STYLE.
        /// <para>
        /// When an Event defines a StageStyleMapping, it means that all descendent shots fired within that Event
        /// will have been fired on the same target and withing the same STAGE STYLE. This method returns a list of 
        /// EventComposites that have defined StageStyleMappings, and thus have distinct STAGE STYLES.
        /// </para>
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 11 )]
        [G_NS.JsonProperty( Order = 11 )]
        public StageStyleMapping StageStyleMapping { get; set; }

        /// <summary>
        /// Helper method to identify Events that define a StageStyleMapping.
        /// <para>
        /// When an Event defines a StageStyleMapping, it means that all descendent shots fired within that Event
        /// will have been fired on the same target and withing the same STAGE STYLE. This method returns a list of 
        /// EventComposites that have defined StageStyleMappings, and thus have distinct STAGE STYLES.
        /// </para>
        /// </summary>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public bool IsATopLevelStageStyle {
            get {
                return this.StageStyleMapping != null;
            }
        }

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
        public RankingRuleMapping RankingRuleMapping { get; set; } = new RankingRuleMapping();

        public bool ShouldSerializeRankingRuleMapping() {

            if (RankingRuleMapping.Count == 0)
                return false;

            if (RankingRuleMapping.Count == 1 &&
                RankingRuleMapping.TryGetValue( RankingRuleMapping.DEFAULTDEF, out string rankingRuleDef ) &&
                rankingRuleDef == RankingRuleMapping.DEFAULT_RANKING_RULE_DEF)
                return false;

            return true;
        }

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
            copy.CalculationVariables = this.CalculationVariables;
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
        /// <remarks>
        /// It is a best practice to check for null or empty string for the value of .RankingRuleDef. This is because .RankingRuleDef is not 
        /// a required property and is allowed to be empty. If it is empty, calling this function GetRankingRuleDefinitionAsync() will throw
        /// an exception.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the value for RankingRuleDef is empty</exception>
        /// <exception cref="DefinitionNotFoundException"></exception>
        /// <exception cref="ScoposAPIException"></exception>
        [Obsolete( "Use GetRankingRuleDefinitionListAsync() instead.")]
        public async Task<RankingRule> GetRankingRuleDefinitionAsync() {

            if (string.IsNullOrEmpty( RankingRuleDef ))
                throw new ArgumentNullException( $"The value for RankingRuleDef is empty or null." );

            SetName rrSetName = SetName.Parse( RankingRuleDef );
            return await DefinitionCache.GetRankingRuleDefinitionAsync( rrSetName );
        }

        /// <inheritdoc />
        /// <remarks>
        /// Returns a list of RANKING RULE definitions referenced by the property .RankingRuleMapping.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the value for ResultListFormatDef is empty.</exception>
        /// <exception cref="DefinitionNotFoundException"></exception>
        /// <exception cref="ScoposAPIException"></exception>
        public async Task<Dictionary<string, RankingRule>> GetRankingRuleDefinitionListAsync() {
            var list = new Dictionary<string, RankingRule>();
            if (this.RankingRuleMapping != null) {
                foreach (var rrmSetName in this.RankingRuleMapping.Values) {
                    if ( ! list.ContainsKey( rrmSetName ) ) {
                        var sn = SetName.Parse( rrmSetName );
                        list[rrmSetName] = await DefinitionCache.GetRankingRuleDefinitionAsync( sn );
                    }
                }
            }

            return list;
        }

        /// <inheritdoc />
        /// <remarks>
        /// It is a best practice to check for null or empty string for the value of .ResultListFormatDef. This is because .ResultListFormatDef is not 
        /// a required property and is allowed to be empty. If it is empty, calling this function GetResultListFormatDefinitionAsync() will throw
        /// an exception.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the value for ResultListFormatDef is empty.</exception>
        /// <exception cref="DefinitionNotFoundException"></exception>
        /// <exception cref="ScoposAPIException"></exception>
        public async Task<ResultListFormat> GetResultListFormatDefinitionAsync() {

            if (string.IsNullOrEmpty( ResultListFormatDef ))
                throw new ArgumentNullException( $"The value for ResultListFormatDef is empty or null." );

            SetName rlfSetName = SetName.Parse( ResultListFormatDef );
            return await DefinitionCache.GetResultListFormatDefinitionAsync( rlfSetName );
        }
    }
}