using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Converters;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using NLog;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class ResultList : ITokenItems<ResultEvent>, IGetResultListFormatDefinition, IGetCourseOfFireDefinition, IGetRankingRuleDefinition, IPublishTransactions {

        private ResultStatus LocalStatus = ResultStatus.UNOFFICIAL;

        private Logger Logger = LogManager.GetCurrentClassLogger();

        public ResultList() {
            Items = new List<ResultEvent>();
        }

        [JsonProperty( Order = 1 )]
		[Obsolete( "Use .Metadata.MatchID" )]
		public string MatchID { get; set; } = string.Empty;

        /// <summary>
        /// Set name of the Ranking Rule definition used to rank this result list.
        /// </summary>
        [JsonProperty( Order = 2 )]
        public string RankingRuleDef { get; set; } = string.Empty;

        /// <summary>
        /// Set name of the Result List Format definition to use when displaying this result list.
        /// </summary>
        [JsonProperty( Order = 2 )]
        public string ResultListFormatDef { get; set; } = string.Empty;

        /// <summary>
        /// Indicates the completion status of this Result List. 
        /// If this is a Virtual Match, the overall Result List status is based on the composite statuses of each parent and child result list.
        /// </summary>
        [JsonProperty( Order = 3 )]
        [JsonConverter( typeof( StringEnumConverter ) )]
        public ResultStatus Status {
            get {
                if (Metadata == null || Metadata.Count == 0)
                    return ResultStatus.OFFICIAL;

                if (this.EndDate < DateTime.Today)
                    return ResultStatus.OFFICIAL;

                bool allFuture = true;
                bool oneIsIntermediate = false;
                bool oneIsUnofficial = false;
                foreach (var rlmd in Metadata.Values) {
                    allFuture &= rlmd.Status == ResultStatus.FUTURE;
                    oneIsIntermediate |= rlmd.Status == ResultStatus.INTERMEDIATE;
                    oneIsUnofficial |= rlmd.Status == ResultStatus.UNOFFICIAL;
                }

                if (allFuture)
                    return ResultStatus.FUTURE;
                if (oneIsIntermediate)
                    return ResultStatus.INTERMEDIATE;
                if (oneIsUnofficial)
                    return ResultStatus.UNOFFICIAL;
                return ResultStatus.OFFICIAL;
            }
        }

        /// <summary>
        /// The start date that the underlying event, in this Result List, started on.
        /// In a Virtual Match, this value is the composite value of each parent and child match.
        /// </summary>
        [JsonConverter( typeof( DateConverter ) )]
        public DateTime StartDate {
            get {
                if (Metadata == null || Metadata.Count == 0)
                    return DateTime.Today;

                DateTime startDate = DateTime.MaxValue;
                foreach( var rlmd in Metadata.Values ) {
                    if (rlmd.StartDate < startDate)
                        startDate = rlmd.StartDate;
                }
                return startDate;
            }
        }

        /// <summary>
        /// The end date that the underlying event, in this Result List, ended on.
        /// In a Virtual Match, this value is the composite value of each parent and child match.
        /// </summary>
        [JsonConverter( typeof( DateConverter ) )]
        public DateTime EndDate {
            get {
                if (Metadata == null || Metadata.Count == 0)
                    return DateTime.Today;

                DateTime endDate = DateTime.MinValue;
                foreach (var rlmd in Metadata.Values) {
                    if (rlmd.EndDate > endDate)
                        endDate = rlmd.EndDate;
                }
                return endDate;
            }
        }

        /// <summary>
        /// The Version string of the JSON document
        /// </summary>
        [JsonProperty( Order = 6 )]
        public string JSONVersion { get; set; } = string.Empty;

        [JsonProperty( Order = 7, DefaultValueHandling = DefaultValueHandling.Include )]
        [DefaultValue( false )]
        public bool Team { get; set; } = false;

        [JsonProperty( Order = 8 )]
        public string ParentID { get; set; } = string.Empty;

        [JsonProperty( Order = 50 )]
        public List<ResultEvent> Items { get; set; } = new List<ResultEvent>();

        /// <summary>
        /// Deprecated, use ResultName
        /// </summary>
        [Obsolete( "Deprecated, use ResultName" )]
        [JsonProperty( Order = 18 )]
        public string Name { get; set; } = string.Empty;

        [JsonProperty( Order = 11 )]
        public string ResultName { get; set; } = string.Empty;

        [JsonProperty( Order = 12 )]
        [JsonConverter( typeof( Scopos.BabelFish.Converters.DateTimeConverter ) )]
		public DateTime LastUpdated { get; set; } = new DateTime();

        [JsonProperty( Order = 13 )]
        public string OwnerId { get; set; } = string.Empty;

        /// <summary>
        /// Set to true if this ResultList is considered one of the most important and should be featured
        /// </summary>
        [JsonProperty( Order = 14, DefaultValueHandling = DefaultValueHandling.Include )]
        [DefaultValue( false )]
        public bool Primary { get; set; } = false;

        [JsonProperty( Order = 15 )]
        public string UniqueID { get; set; } = string.Empty;

        /// <summary>
        /// EventName is the name of the top level Event for this Result List.
        /// </summary>
        [JsonProperty( Order = 16 )]
        public string EventName { get; set; } = string.Empty;

        [JsonProperty( Order = 17 )]
        public string ResultListID { get; set; } = string.Empty;

        /// <summary>
        /// If True, Participants are listed in order of their projected score. 
        /// Should only ever be true if Status is FUTURE or INTERMEDIATE
        /// </summary>
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.Include )]
        [DefaultValue(false)]
        public bool Projected { get; set; } = false;

        /// <summary>
        /// Indicates if the Result List has been truncated. and the values in .Items are not all of the results.
        /// 
        /// Currently known to be set in the Reat API Get Result List lambda, when pulling result lists
        /// from dynamo that are too large. There is some remaining question if this field is needed. In 
        /// theory .Partial is the opposite of .HasMoreItems
        /// </summary>
        public bool Partial { get; set; } = false;

        /// <summary>
        /// The SetName of the Course of Fire
        /// </summary>
        public string CourseOfFireDef { get; set; } = string.Empty;

        public string ScoreConfigName { get; set; } = string.Empty;

        /// <inheritdoc />
        [DefaultValue( "" )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate )]
        public string NextToken { get; set; } = string.Empty;

        /// <inheritdoc />
        [DefaultValue(0)]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate )]
        public int Limit { get; set; } = 0;

        /// <inheritdoc />
        [DefaultValue( false )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate )]
        public bool HasMoreItems {
            get {
                return !string.IsNullOrEmpty( NextToken );
            }
        }

        /// <inheritdoc />
        [DefaultValue( "" )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate )]
        public string PublishTransactionId { get; set; } = string.Empty;

        /// <inheritdoc />
        [DefaultValue( 0 )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate )]
        public int TransactionSequence { get; set; } = 0;

        /// <inheritdoc />
        [DefaultValue( 1 )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate )]
        public int TransactionCount { get; set; } = 1;

        /// <summary>
        /// String holding the software (Orion Scoring System) and Version number of the software.
        /// </summary>
        [Obsolete( "Use .Metadata.Creator" )]
		public string Creator { get; set; }

        /// <summary>
        /// Key is the local match ID.
        /// Value is the Metadate for the generative match.
        /// When Orion generates a ResultList there will only be 1 value in Metadata.
        /// When a Virtual Match is merged, each parent / child ID will be listed.
        /// Local Matches will have exactly one value.
        /// </summary>
        public Dictionary<string, ResultListMetadata> Metadata { get; set; } = new Dictionary<string, ResultListMetadata>();

        /// <inheritdoc />
        /// <exception cref="ScoposAPIException">Thrown if the value for CourseOfFireDef is empty, or if the Get Definition call was unsuccessful.</exception>
        public async Task<CourseOfFire> GetCourseOfFireDefinitionAsync() {

            if (string.IsNullOrEmpty( CourseOfFireDef ))
                throw new ScoposAPIException( $"The value for CourseOfFireDef is empty or null.", Logger );

            SetName cofSetName = SetName.Parse( CourseOfFireDef );
            return await DefinitionCache.GetCourseOfFireDefinitionAsync( cofSetName );
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

        /// <inheritdoc />
        public override string ToString() {
            return $"ResultList for {ResultName}" ;
        }
    }
}
