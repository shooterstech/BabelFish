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
    public class Event : IReconfigurableRulebookObject, IGetResultListFormatDefinition, IGetRankingRuleDefinition {

        private Logger Logger = LogManager.GetCurrentClassLogger();

        public Event()
        {
            //Children = new List<string>();
            ScoreFormat = "Events";
            Calculation = "SUM";
            EventType = EventtType.NONE;
        }

        /// <summary>
        /// A unique name given to this Event.
        /// </summary>
        [JsonPropertyOrder( 1 )]
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
        [JsonPropertyOrder( 2 )]
        public EventtType EventType { get; set; } = EventtType.NONE;

        /// <summary>
        /// The children of this event identified by the EventName. The score for this event is added together from the scores of the children.
        /// </summary>
        [JsonPropertyOrder( 3 )]
        public List<string> Children { get; set; }

        /// <summary>
        /// The method to use to calculate the score of this event from the children. Must be one of the following:
        /// * SUM
        /// </summary>
        [JsonPropertyOrder( 4 )]
        public string Calculation { get; set; } = "SUM";

        /// <summary>
        /// The score format to use to display scores for this Event.
        /// The possible values are learned from the Score Format Collection.
        /// </summary>
        [JsonPropertyOrder( 5 )]
        public string ScoreFormat { get; set; } = "Events";

        /// <summary>
        /// Formatted as a ValueSeries
        /// </summary>
        [JsonPropertyOrder( 6 )]
        [DefaultValue("")]
        public string Values { get; set; }

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

        [JsonPropertyOrder( 16 )]
        public RankingRuleMapping RankingRuleMapping { get; set;}

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