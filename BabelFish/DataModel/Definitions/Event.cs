using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NLog;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.DataModel.Definitions
{
    /// <summary>
    /// In the Reconfigurable Rulebook Events are defined using the well known Composite Pattern. 
    /// An Event is either a composite Event, that is made up of child Events, or it is a singular 
    /// Event that is a leaf. Within a COURSE OF FIRE Composite events are defined separately from Singular Events.
    /// </summary>
    public class Event : IReconfigurableRulebookObject, IGetResultListFormatDefinition {

        private List<string> validationErrorList = new List<string>();

        private Logger logger = LogManager.GetCurrentClassLogger();

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
        [JsonProperty(Order = 1)]
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
        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(Order = 2)]
        public EventtType EventType { get; set; } = EventtType.NONE;

        /// <summary>
        /// The children of this event identified by the EventName. The score for this event is added together from the scores of the children.
        /// Can either be a list of strings, or 
        /// {
        ///   "EventName" : "S{}",
        ///   "String" : 10,
        ///   "Values" : "1..500",
        ///   "StageLabel" : "S"
        /// }
        /// </summary>
        [JsonProperty(Order = 3)]
        public dynamic Children { get; set; }

        public List<string> GetChildrenEventNames() {
            List<string> listOfEventNames = new List<string>();

            try {
                if (Children is JArray) {
                    foreach (var childName in Children) {
                        listOfEventNames.Add( (string)childName );
                    }
                } else {
                    //It's a dictionary
                    
                    //var values = (string)Children["Values"];
                    //var valueIndexes = values.Split( ".." );
                    //int start = int.Parse( valueIndexes[0] );
                    //int stop = int.Parse( valueIndexes[1] );

                    ValueSeries vs = new ValueSeries((string)Children["Values"]);
                    var valueList = vs.GetAsList();

                    foreach (int i in valueList){
                        var eventName = (string)Children["EventName"];
                        listOfEventNames.Add( eventName.Replace( "{}", i.ToString() ) );
                    }
                }
            } catch (Exception ex) {
                logger.Error( ex );
            }
            return listOfEventNames;
        }

        /// <summary>
        /// The method to use to calculate the score of this event from the children. Must be one of the following:
        /// * SUM
        /// * AVG
        /// </summary>
        [JsonProperty(Order = 4)]
        public string Calculation { get; set; }

        /// <summary>
        /// The score format to use to display scores for this Event.
        /// The possible values are learned from the Score Format Collection.
        /// </summary>
        [JsonProperty(Order = 5)]
        public string ScoreFormat { get; set; }

        /// <summary>
        /// Formatted as a ValueSeries
        /// </summary>
        [JsonProperty(Order = 6)]
        [DefaultValue("")]
        public string Values { get; set; }

        /// <summary>
        /// StageStyleSelection determines how the resulting Result COF is mapped to a STAGE STYLE.
        /// </summary>
        [JsonProperty(Order = 11)]
        public StageStyleMapping StageStyleMapping { get; set; }

        /// <summary>
        /// EventStyleSelection determines how the resulting Result COF is mapped to a EVENT STYLE.
        /// </summary>
        [JsonProperty(Order = 13)]
        public EventStyleMapping EventStyleMapping { get; set; }

        /// <summary>
        /// The recommended Result List Format defintion to use when displaying a result list for this Event.
        /// </summary>
        [JsonProperty( Order = 14 )]
        [DefaultValue( "" )]
        public string ResultListFormatDef { get; set; } = string.Empty;

        /// <summary>
        /// The recommended Ranking Rules defintion to use when displaying a ranking list for this Event.
        /// </summary>
        [JsonProperty(Order = 15)]
        [DefaultValue("")]
        public string RankingRuleDef { get; set; } = string.Empty;

        /// <summary>
        /// Internal documentation comments. All text is ignored by the system.
        /// </summary>
        [JsonProperty(Order = 99)]
        [DefaultValue("")]
        public string Comment { get; set; }

        public override string ToString() {
            return $"{EventName} of {EventType}";
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException">Thrown if the value of .ResultListFormatDef could not be parsed. Which shouldn't happen.</exception>
        public async Task<ResultListFormat> GetResultListFormatDefinitionAsync() {

            SetName resultListFormatSetName = SetName.Parse( ResultListFormatDef );
            var getDefiniitonResponse = await DefinitionFetcher.FETCHER.GetResultListFormatDefinitionAsync( resultListFormatSetName );
            return getDefiniitonResponse.Definition;
        }
    }
}