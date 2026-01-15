using System.ComponentModel;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class ResultList : ITokenItems<ResultEvent>, IRLIFList, IGetResultListFormatDefinition, IGetCourseOfFireDefinition, IGetRankingRuleDefinition, IPublishTransactions {

        private ResultStatus _localStatus = ResultStatus.UNOFFICIAL;

        private Logger _logger = LogManager.GetCurrentClassLogger();

        public ResultList() {
            Items = new List<ResultEvent>();
        }

        /// <summary>
        /// The name of the match, that this Result List was generated from.
        /// </summary>
        [G_NS.JsonProperty( Order = 1 )]
        public string MatchName { get; set; } = string.Empty;

        /// <summary>
        /// The name of this Result List.
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        public string ResultName { get; set; } = string.Empty;

        /// <summary>
        /// EventName is the name of the top level Event for this Result List.
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        public string EventName { get; set; } = string.Empty;

        /// <summary>
        /// If this is a local match, returns the local match id.
        /// If this is from a virtual match, retur s the virtual match id.
        /// </summary>
        /// <remarks>This value is not serialized.</remarks>
        [G_NS.JsonIgnore]
        [G_STJ_SER.JsonIgnore]
        public string MatchID {
            get {
                if (this.Metadata.Count == 0) {
                    //This shouldn't happen
                    return "1.1.1.1";
                }

                if (this.Metadata.Count == 1)
                    return this.Metadata.First().Key;

                //Likely a Virtual Match
                foreach (var mId in this.Metadata.Keys) {
                    if (Scopos.BabelFish.DataModel.OrionMatch.MatchID.TryParse( mId, out var matchID )) {
                        if (matchID.League || matchID.VirtualMatchParent || matchID.MatchGroup) {
                            return mId;
                        }
                    }
                }

                //Um, not really sure how we got this far.
                return this.Metadata.First().Key;

            }
        }

        /// <summary>
        /// Indicates the completion status of this Result List. 
        /// If this is a Virtual Match, the overall Result List status is based on the composite statuses of each parent and child result list.
        /// </summary>
        /// <remarks>This value is calculated from .MetaData and is also serialzied.</remarks>
        [G_NS.JsonProperty( Order = 4, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        public ResultStatus Status {
            get {
                if (Metadata == null || Metadata.Count == 0)
                    return ResultStatus.OFFICIAL;

                if (Metadata.Count == 1) {
                    //This is likely a local match
                    var meta = Metadata.First().Value;

                    if (meta.EndDate < DateTime.Today)
                        return ResultStatus.OFFICIAL;

                    return meta.Status;
                }

                //If we get here, this would be a virtual match
                //Find the parent's meta data
                ResultListMetadata parentMetaData = null;
                MatchID matchId;
                foreach (var meta in Metadata) {
                    //The Key is the match id in string form
                    try {
                        matchId = new MatchID( meta.Key );
                        if (matchId.VirtualMatchParent) {
                            parentMetaData = meta.Value;
                            break;
                        }
                    } catch (FormatException fe) {
                        ; //Assume this is not the parent
                    }
                }

                /*
                # The rules of determining the status of the merged Result List is as follows
                # OFFICIAL if parent says its official, or today's date is past the end date set by the parent
                # FUTURE if parent and all children are FUTURE
                # INTERMEDIATE if the parent or one child says INTERMEDIATE
                # UNOFFICIAL if the parent and all children say UNOFFICIAL or OFFICIAL or in the past
                */

                //If the parent's end date is in the past, or it's status is official, then the VM status is official
                if (parentMetaData != null) {
                    if (parentMetaData.EndDate < DateTime.Today)
                        return ResultStatus.OFFICIAL;

                    if (parentMetaData.Status == ResultStatus.OFFICIAL)
                        return ResultStatus.OFFICIAL;
                }

                bool allFuture = true;
                bool oneIsIntermediate = false;
                bool oneIsUnofficial = false;
                foreach (var rlmd in Metadata.Values) {
                    if (rlmd.Creator == "No creator found.")
                        //Skip, since this child match was created with an old version of Orion that is unreliable reporting status.
                        continue;
                    allFuture &= rlmd.Status == ResultStatus.FUTURE;
                    oneIsIntermediate |= rlmd.Status == ResultStatus.INTERMEDIATE;
                    oneIsUnofficial |= (rlmd.Status == ResultStatus.UNOFFICIAL || rlmd.Status == ResultStatus.OFFICIAL || rlmd.EndDate < DateTime.Today);
                }

                if (allFuture)
                    return ResultStatus.FUTURE;
                if (oneIsIntermediate)
                    return ResultStatus.INTERMEDIATE;
                if (oneIsUnofficial)
                    return ResultStatus.UNOFFICIAL;

                //Theoretically, should never get here
                return ResultStatus.OFFICIAL;
            }
        }

        /// <summary>
        /// The MatchID that this Result List was generated from. It is called ParentID in case this is
        /// from a Virtual Match.
        /// </summary>
        [G_NS.JsonProperty( Order = 5 )]
        public string ParentID { get; set; } = string.Empty;

        /// <summary>
        /// The start date that the underlying event, in this Result List, started on.
        /// In a Virtual Match, this value is the composite value of each parent and child match.
        /// </summary>
        /// <remarks>This value is calculated from .MetaData and is also serialzied.</remarks>
        [G_NS.JsonProperty( Order = 6, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
        public DateTime StartDate {
            get {
                if (Metadata == null || Metadata.Count == 0)
                    return DateTime.Today;

                DateTime startDate = DateTime.MaxValue;
                foreach (var rlmd in Metadata.Values) {
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
        /// <remarks>This value is calculated from .MetaData and is also serialzied.</remarks>
        [G_NS.JsonProperty( Order = 7, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateOnlyConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
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
        /// Set to true if this ResultList is considered one of the most important and should be featured
        /// </summary>
        [G_NS.JsonProperty( Order = 8, DefaultValueHandling = G_NS.DefaultValueHandling.Include )]
        [DefaultValue( false )]
        public bool Primary { get; set; } = false;

        /// <summary>
        /// Indicates if this Result List is for a team event.
        /// </summary>
        [G_NS.JsonProperty( Order = 9 )]
        [DefaultValue( false )]
        public bool Team { get; set; } = false;

        /// <summary>
        /// If True, Participants are listed in order of their projected score. 
        /// Should only ever be true if Status is FUTURE or INTERMEDIATE
        /// </summary>
        [G_NS.JsonProperty( Order = 10 )]
        [DefaultValue( false )]
        public bool Projected { get; set; } = false;

        /// <summary>
        /// Indicates if the Result List has been truncated. and the values in .Items are not all of the results.
        /// 
        /// <para>Currently known to be set in the Reat API Get Result List lambda, when pulling result lists
        /// from dynamo that are too large. There is some remaining question if this field is needed. In 
        /// theory .Partial is the opposite of .HasMoreItems</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 11 )]
        public bool Partial { get; set; } = false;

        /// <summary>
        /// UTC time that this Result List was updated.
        /// </summary>
        [G_NS.JsonProperty( Order = 12 )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ScoposDateTimeConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateTimeConverter ) )]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Key is the local match ID.
        /// Value is the Metadate for the generative match.
        /// When Orion generates a ResultList there will only be 1 value in Metadata.
        /// When a Virtual Match is merged, each parent / child ID will be listed.
        /// Local Matches will have exactly one value.
        /// </summary>
        [G_NS.JsonProperty( Order = 15 )]
        public Dictionary<string, ResultListMetadata> Metadata { get; set; } = new Dictionary<string, ResultListMetadata>();

        /// <summary>
        /// Set name of the RANKING RULE definition used to rank this result list.
        /// </summary>
        [G_NS.JsonProperty( Order = 20 )]
        public string RankingRuleDef { get; set; } = string.Empty;

        /// <summary>
        /// The SetName of the Course of Fire
        /// </summary>
        [G_NS.JsonProperty( Order = 21 )]
        public string CourseOfFireDef { get; set; } = string.Empty;

        /// <summary>
        /// The ScoreConfigName to use, within the SCORE FORMAT COLLECTION definition to format scores.
        /// <para>NOTE: The SCORE FORMAT COLLECTION is specified within the RESULT LIST FORMAT definition.</para>
        /// </summary>
        [G_NS.JsonProperty( Order = 22 )]
        public string ScoreConfigName { get; set; } = string.Empty;

        /// <summary>
        /// Set name of the RESULT LIST FORMAT definition to use when displaying this result list.
        /// </summary>
        [G_NS.JsonProperty( Order = 23 )]
        public string ResultListFormatDef { get; set; } = string.Empty;

        /// <summary>
        /// On RESULT LIST FORMAT definitions that provided for the option, the user (usually the Match Director) may specify their own
        /// interpolated values for designated fields. These are known as UserDefinedText. There are at most three user defined fields in a
        /// RESULT LIST FORMAT (man definitions do not have any).
        /// <para>The most common example is a demographic spanning text field.</para>
        /// <para>Text values are interpolated with any common field or user defined field. The list is common fields is at
        /// <see href="https://support.scopos.tech/index.html?definition-resultlistfield.html">support.scopos.tech</see></para>
        /// <para>Example text values:</para>
        /// <list type="bullet">
        /// <item>"Competitor Number: {CompetitorNumber}, Hometown: {Hometown}</item>
        /// <item>"Club: {Organization}, Coach: {Coach}</item>
        /// </list>
        /// </summary>
        [G_NS.JsonProperty( Order = 24 )]
        public Dictionary<UserDefinedFieldNames, string> UserDefinedText { get; set; } = new Dictionary<UserDefinedFieldNames, string>() {
            [UserDefinedFieldNames.USER_DEFINED_FIELD_1] = string.Empty,
            [UserDefinedFieldNames.USER_DEFINED_FIELD_2] = string.Empty,
            [UserDefinedFieldNames.USER_DEFINED_FIELD_3] = string.Empty,
        };

        /// <summary>
        /// Newtonsoft.json helper method, to determine if UserDefinedText should be serialized.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeUserDefinedText() {
            //Serialized when UserDefinedText has at least one value that's not an empty string.
            return (UserDefinedText is not null) &&
                ((UserDefinedText.TryGetValue( UserDefinedFieldNames.USER_DEFINED_FIELD_1, out string text1 ) && !string.IsNullOrEmpty( text1 )) ||
                (UserDefinedText.TryGetValue( UserDefinedFieldNames.USER_DEFINED_FIELD_2, out string text2 ) && !string.IsNullOrEmpty( text2 )) ||
                (UserDefinedText.TryGetValue( UserDefinedFieldNames.USER_DEFINED_FIELD_3, out string text3 ) && !string.IsNullOrEmpty( text3 )));
        }

        /// <summary>
        /// The ranked (and should be sorted) list of participants.
        /// </summary>
        [G_NS.JsonProperty( Order = 30 )]
        public List<ResultEvent> Items { get; set; } = new List<ResultEvent>();

        /// <inheritdoc />
        public List<IRLIFItem> GetAsIRLItemsList() {
            return Items.ToList<IRLIFItem>();
        }

        /// <summary>
        /// The Version string of the JSON document
        /// </summary>
        [G_NS.JsonProperty( Order = 41 )]
        public string JSONVersion { get; set; } = string.Empty;

        #region ITokenItems implementation
        /// <inheritdoc />
        [DefaultValue( "" )]
        [G_NS.JsonProperty( Order = 50 )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.NextTokenConverter ) )]
        public string NextToken { get; set; } = string.Empty;

        /// <inheritdoc />
        [G_NS.JsonProperty( Order = 51 )]
        [DefaultValue( 0 )]
        public int Limit { get; set; } = 0;

        /// <inheritdoc />
        [G_NS.JsonProperty( Order = 52 )]
        [DefaultValue( false )]
        public bool HasMoreItems {
            get {
                return !string.IsNullOrEmpty( NextToken );
            }
        }
        #endregion

        #region IPublishTransactions Implementation
        /// <inheritdoc />
        [G_NS.JsonProperty( Order = 55 )]
        [DefaultValue( "" )]
        public string PublishTransactionId { get; set; } = string.Empty;

        /// <inheritdoc />
        [G_NS.JsonProperty( Order = 56 )]
        [DefaultValue( 0 )]
        public int TransactionSequence { get; set; } = 0;

        /// <inheritdoc />
        [G_NS.JsonProperty( Order = 57 )]
        [DefaultValue( 1 )]
        public int TransactionCount { get; set; } = 1;
        #endregion


        /// <summary>
        /// String holding the software (Orion Scoring System) and Version number of the software.
        /// </summary>
        [G_NS.JsonProperty( Order = 90 )]
        [Obsolete( "Use .Metadata.Creator" )]
        public string Creator { get; set; }

        /// <summary>
        /// The orion account or at home account who owns this match.
        /// </summary>
        /// <example>OrionAcct000001 or AtHomeAcct123456</example>
        [Obsolete( "Use .MetaData.OwnerId" )]
        [G_NS.JsonProperty( Order = 91 )]
        public string OwnerId { get; set; } = string.Empty;

        /// <summary>
        /// EKA Note Jan 2026: Not really sure what this property does. Likely a artjifact of saveing Result Lists in dynamo.
        /// </summary>
        [Obsolete( "Likely will remove soon." )]
        [G_NS.JsonProperty( Order = 92 )]
        public string UniqueID { get; set; } = string.Empty;

        /// <summary>
        /// EKA Note Jan 2026: Not really sure what this property does. Likely a artjifact of saveing Result Lists in dynamo.
        /// </summary>
        [Obsolete( "Likely will remove soon." )]
        [G_NS.JsonProperty( Order = 93 )]
        public string ResultListID { get; set; } = string.Empty;

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown if the value for CourseOfFireDef is empty, known to happen with older versions of Orion. </exception>
        /// <exception cref="XApiKeyNotSetException" ></exception>
        /// <exception cref="DefinitionNotFoundException" ></exception>
        /// <exception cref="ScoposAPIException" ></exception>
        public async Task<CourseOfFire> GetCourseOfFireDefinitionAsync() {

            if (string.IsNullOrEmpty( CourseOfFireDef ))
                throw new ArgumentNullException( $"The value for CourseOfFireDef is empty or null." );

            SetName cofSetName = SetName.Parse( CourseOfFireDef );
            return await DefinitionCache.GetCourseOfFireDefinitionAsync( cofSetName );
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown if the value for RankingRuleDef is empty, or if the Get Definition call was unsuccessful.</exception>
        /// <exception cref="XApiKeyNotSetException" ></exception>
        /// <exception cref="DefinitionNotFoundException" ></exception>
        /// <exception cref="ScoposAPIException" ></exception>
        public async Task<RankingRule> GetRankingRuleDefinitionAsync() {

            if (string.IsNullOrEmpty( RankingRuleDef ))
                throw new ArgumentNullException( $"The value for RankingRuleDef is empty or null." );

            SetName rrSetName = SetName.Parse( RankingRuleDef );
            return await DefinitionCache.GetRankingRuleDefinitionAsync( rrSetName );
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown if the value for ResultListFormatDef is empty, or if the Get Definition call was unsuccessful.</exception>
        /// <exception cref="XApiKeyNotSetException" ></exception>
        /// <exception cref="DefinitionNotFoundException" ></exception>
        /// <exception cref="ScoposAPIException" ></exception>
        public async Task<ResultListFormat> GetResultListFormatDefinitionAsync() {

            if (string.IsNullOrEmpty( ResultListFormatDef ))
                throw new ArgumentNullException( $"The value for ResultListFormatDef is empty or null." );

            SetName rlfSetName = SetName.Parse( ResultListFormatDef );
            return await DefinitionCache.GetResultListFormatDefinitionAsync( rlfSetName );
        }

        /// <summary>
        /// Facade property that returns the same as .ResultName
        /// </summary>
        /// <remarks>Property is not serialized.</remarks>
        [G_NS.JsonIgnore]
        public string Name {
            get {
                return this.ResultName;
            }
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"ResultList for {ResultName}";
        }

        /// <summary>
        /// Attempts to fine the ResultEvent in the list of Items, by the Result Cof ID.
        /// </summary>
        /// <param name="resultCofId"></param>
        /// <param name="resultEvent"></param>
        /// <returns></returns>
        public bool TryGetByResultCOFID( string resultCofId, out ResultEvent resultEvent ) {

            foreach (var re in this.Items) {
                if (re.ResultCOFID == resultCofId) {
                    resultEvent = re;
                    return true;
                }
            }

            resultEvent = null;
            return false;
        }
    }
}
