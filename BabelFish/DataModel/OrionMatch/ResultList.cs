using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Converters;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class ResultList : ITokenItems<ResultEvent>, IGetResultListFormatDefinition, IGetCourseOfFireDefinition {

        private ResultStatus LocalStatus = ResultStatus.UNOFFICIAL;

        public ResultList() {
            Items = new List<ResultEvent>();
        }

        [JsonProperty( Order = 1 )]
		[Obsolete( "Use .Metadata.MatchID" )]
		public string MatchID { get; set; } = string.Empty;

        /// <summary>
        /// Set name of the Ranking Rule definition
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
                    return ResultStatus.UNOFFICIAL;

                bool allFuture = true;
                bool allUnofficial = true;
                bool allOfficial = true;
                foreach (var rlmd in Metadata.Values) {
                    allFuture &= rlmd.Status == ResultStatus.FUTURE;
                    allUnofficial &= rlmd.Status == ResultStatus.UNOFFICIAL;
                    allOfficial &= rlmd.Status == ResultStatus.OFFICIAL;
                }

                if (allFuture)
                    return ResultStatus.FUTURE;
                if (allUnofficial)
                    return ResultStatus.UNOFFICIAL;
                if (allOfficial)
                    return ResultStatus.OFFICIAL;
                return ResultStatus.INTERMEDIATE;
            }
            set {
                //To make Status JSON serialziable, we need to have a SET method. Choosing 
                //not to do anything, set the status is based on the .MetaData.Status
                ;
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

        [JsonProperty( Order = 7 )]
        public bool Team { get; set; } = false;

        [JsonProperty( Order = 8 )]
        public string ParentID { get; set; } = string.Empty;

        /// <summary>
        /// The relative importance / sort order of this ResultList within the match
        /// </summary>
        [JsonProperty( Order = 9 )]
        public int SortOrder { get; set; } = 0;

        [JsonProperty( Order = 10 )]
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
        [JsonConverter( typeof( DateTimeConverter ) )]
        [Obsolete( "Use .Metadata.LastUpdated" )]
		public DateTime LastUpdated { get; set; } = new DateTime();

        [JsonProperty( Order = 13 )]
        public string OwnerId { get; set; } = string.Empty;

        /// <summary>
        /// Set to true if this ResultList is considered one of the most important and should be featured
        /// </summary>
        [JsonProperty( Order = 14 )]
        public bool Primary { get; set; } = false;

        [JsonProperty( Order = 15 )]
        public string UniqueID { get; set; } = string.Empty;

        [JsonProperty( Order = 16 )]
        public string EventName { get; set; } = string.Empty;

        [JsonProperty( Order = 17 )]
        public string ResultListID { get; set; } = string.Empty;

        public bool Preliminary { get; set; } = false;


        /// <summary>
        /// The SetName of the Course of Fire
        /// </summary>
        public string CourseOfFireDef { get; set; } = string.Empty;

        public string ScoreConfigName { get; set; } = string.Empty;

        /// <inheritdoc />
        public string NextToken { get; set; } = string.Empty;

        /// <inheritdoc />
        public int Limit { get; set; } = 50;

        /// <inheritdoc />
        public bool HasMoreItems {
            get {
                return !string.IsNullOrEmpty( NextToken );
            }
        }

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
		public Dictionary<string, ResultListMetadata> Metadata { get; set; }

		/// <inheritdoc />
		public async Task<CourseOfFire> GetCourseOfFireDefinitionAsync() {

            if (string.IsNullOrEmpty( CourseOfFireDef ))
                return null;

            SetName cofSetName = SetName.Parse( CourseOfFireDef );
            var getDefiniitonResponse = await DefinitionFetcher.FETCHER.GetCourseOfFireDefinitionAsync( cofSetName );
            return getDefiniitonResponse.Definition;
        }

        /// <inheritdoc />
        public async Task<ResultListFormat> GetResultListFormatDefinitionAsync() {

            if (string.IsNullOrEmpty( ResultListFormatDef ))
                return null;

            SetName rlfSetName = SetName.Parse( ResultListFormatDef );
            var getDefiniitonResponse = await DefinitionFetcher.FETCHER.GetResultListFormatDefinitionAsync( rlfSetName );
            return getDefiniitonResponse.Definition;
        }

        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append( "ResultList for " );
            foo.Append( ResultName );
            return foo.ToString();
        }
    }
}
