using System.Diagnostics;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.ResultListMerger {

    /// <summary>
    /// A ResultListMergerEngine does the heavy lifting of merging Result Lists together.
    /// <para>The <see cref="MergeAsync"/> method in particular (after initialization) identifies participants that are
    /// in one or more of the Match's <see cref="ResultListMember">members</see>, and then asks the <see cref="MergeMethod"/> to
    /// perform its merging calculation.</para>
    /// </summary>
    public class ResultListMergerEngine {

        #region Private Variables

        private MergeMethod _mergeMethod;

        private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private static OrionMatchAPIClient _apiClient = new OrionMatchAPIClient();
        private int _dynamicColumnIndex = 0;

        /// <summary>
        /// Dictionary of all the participants (teams or athletes) that competed in at least one of the
        /// Result List Members. 
        /// <para>The key is a unique id identifying the participant.</para>
        /// <para>The value is a ResultEvent instance that holds the scores from the Result List Member. This ResultEvent 
        /// instance has its dictionary called .ResultCofScores where the scores are stored.</para>
        /// </summary>
        private Dictionary<int, ResultEvent> _mergedResultEvents = new Dictionary<int, ResultEvent>();

        #endregion

        #region Constructors, Factory Methods, and Initialization Methods

        /// <summary>
        /// Constructor. Purposefully private, users must call <see cref="CreateAsync(Tournament, string)"/> instead.
        /// </summary>
        private ResultListMergerEngine() {
        }

        /// <summary>
        /// Factory method to construct a new ResultListMergerEngine instance.
        /// <para>It constructs an instance based on the passed in Tournament and result list name (that better be part of that Tournament).</para>
        /// </summary>
        /// <param name="tournament"></param>
        /// <param name="resultName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">If resultName is not found within the Tournament's MergedResultLists.</exception>
        public static async Task<ResultListMergerEngine> CreateAsync( MergedResultList mergedResultList ) {

            ResultListMergerEngine resultListMerger = new ResultListMergerEngine();

            resultListMerger.MergedResultList = mergedResultList;

            resultListMerger.ResultListsMembers = await resultListMerger.Container.ResultListFetcher.GetResultListsAsync( mergedResultList );

            resultListMerger._mergeMethod = await MergeMethod.CreateAsync( resultListMerger );
            //If the MergeMethod does not set values for ResultListFormat or RankingRule, then try and auto generate them.
            if (resultListMerger.ResultListFormat is null)
                resultListMerger.AutoGenerateResultListFormat();
            if (resultListMerger.RankingRule is null)
                resultListMerger.AutoGenerateRankingRule();

            return resultListMerger;
        }

        #endregion

        #region Event Handlers

        #endregion

        #region Data Model Properties
        //Of which there should not be any, as this class is a data actor, not a data model class.
        #endregion

        #region Helper Properties
        /// <summary>
        /// Container is a pointer to the container that holds <see cref="MergedResultList"/>, and presumable wants to calculate
        /// the results of merging result lists together. Both <see cref="Tournament"/> and <see cref="MatchStructure"/> can be this containers
        /// as they both implement <see cref="IMergedResultListContainer"/>.
        /// </summary>
        public IMergedResultListContainer Container {
            get {
                return this.MergedResultList.Container;
            }
        }

        /// <summary>
        /// The name of the merged result list.
        /// <para>Set during the construction of the ResultListMergerEngine. Must be a name listed in the Match's .MergedResultLists property.</para>
        /// </summary>
        public string ResultName {
            get {
                return this.MergedResultList.ResultName;
            }
        }

        /// <summary>
        /// The MergedResultList instance that this engine is merging the Result List Members for.
        /// </summary>
        public MergedResultList MergedResultList { get; private set; }

        /// <summary>
        /// The set of Result Lists that will be merged.
        /// </summary>
        public List<ResultList> ResultListsMembers { get; private set; } = new List<ResultList>();

        /// <summary>
        /// The RESULT LIST FORMAT to use to display this merged Result List.
        /// <para>Commonly the RESULT LIST FORMAT is auto generataed using the method .AutoGenerateResultListFormat()</para>
        /// </summary>
        public ResultListFormat? ResultListFormat { get; set; } = null;

        /// <summary>
        /// The RANKING RULE to use to rank the participants of this merged Result List.
        /// <para>Commonly the RANKING RULE is auto generated using the method .AutoGenerateRankingRule()</para>
        /// </summary>
        public RankingRule RankingRule { get; set; } = null;

        #endregion

        #region Methods

        /// <summary>
        /// Method to dynamically auto-generate a RESULT LIST FORMAT definition. If the concrete class MergeMethod can generate a result list format
        /// as indicated by <see cref="MergeMethod.CanGenerateResultListFormat"/>, the ResultListFormat generated by the concrete class <see cref="MergeMethod.GenerateResultListFormat"/>
        /// will be used. Otherwise, this method will generate a default result list format based on the <see cref="ResultListMember">members</see> and <see cref="MergeConfiguration">configuration</see>.
        /// <para>Typically not called directly (although you can), instead the <see cref="CreateAsync(MergedResultList)"/> calls this methos when it initializes.</para>
        /// <para>Users may also use their own specified RESULT LIST FORMAT by setting the ResultListFormat property.</para>
        /// </summary>
        public void AutoGenerateResultListFormat() {
            ResultListFormat rlf;
            if (_mergeMethod.CanGenerateResultListFormat) {
                // If CanGenerateResultListFormat is true, then we should be able to generate a result list format from the configuration. If we can't, then that's mean's the person who set up the configuration made a mistake.
                rlf = _mergeMethod.GenerateResultListFormat();

                if (rlf is not null) {
                    // This is the expected path.
                    this.ResultListFormat = rlf;
                    return;
                } else {
                    Debug.Fail( "MergedResultList.Configuration.CanGenerateResultListFormat was true, but GenerateResultListFormat() returned null. This should not happen, and likely means the person who set up the configuration didn't implement GenerateResultListFormat()." );
                    // Will fall through to the auto generation code below, which is better than crashing, but the results might not be what the user expected.
                }
            }

            rlf = new ResultListFormat();
            rlf.SetDefaultValues();
            rlf.ScoreConfigDefault = MergedResultList.Configuration.ScoreConfigName;
            rlf.ScoreFormatCollectionDef = MergedResultList.Configuration.ScoreFormatCollectionDef;
            rlf.Fields.Clear();

            rlf.Fields.Add( new ResultListField() {
                FieldName = "Aggregate",
                Method = ResultFieldMethod.SCORE,
                Source = new FieldSource() {
                    Name = _mergeMethod.TopLevelEventname,
                    ScoreFormat = "Events",
                    ScoreConfigName = MergedResultList.Configuration.ScoreConfigName
                }
            } );

            foreach (var additionalFields in _mergeMethod.AdditionalFields) {
                rlf.Fields.Add( additionalFields );
            }

            for (int i = this.MergedResultList.ResultListMembers.Count - 1; i >= 0; i--) {
                var resultList = this.ResultListsMembers[i];
                var resultListMember = this.MergedResultList.ResultListMembers[i];

                var key = ResultEvent.KeyForResultCofScore( resultList.MatchID, resultList.EventName );

                rlf.Fields.Add( new ResultListField() {
                    FieldName = resultListMember.HeaderName,
                    Method = ResultFieldMethod.SCORE,
                    Source = new FieldSource() {
                        Name = key,
                        ScoreFormat = "Events",
                        ScoreConfigName = MergedResultList.Configuration.ScoreConfigName
                    }
                } );
            }

            rlf.Format.Columns.Clear();
            if (MergedResultList.Configuration.IncludeRankColumn) {
                rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                    Header = "Rank",
                    Body = "{Rank} {RankDelta}",
                    BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{Rank} {RankDelta}"
                    }
                },
                    ClassSet = new List<ClassSet>() { new ClassSet() {
                    Name = "rlf-col-rank",
                    ShowWhen = ShowWhenVariable.CreateAlwaysShow()
                }}
                } );
            }

            rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Participant",
                Body = "{DisplayName}",
                BodyLinkTo = LinkToOption.PublicProfile,
                BodyValues = new List<ResultListCellValue>() {
                    new ResultListCellValue() {
                        Text = "{DisplayName}",
                        LinkTo = LinkToOption.PublicProfile
                    }
                },
                ClassSet = new List<ClassSet>() { new ClassSet() {
                    Name = "rlf-col-participant",
                    ShowWhen = ShowWhenVariable.CreateAlwaysShow()
                }}
            } );

            // Generate the additional columns here, so we have an accurate count.
            var additionalColumns = _mergeMethod.AdditionalDisplayColumns;

            // Determine the ShowWhen condition for the event columns based on the number of dynamic columns we have (which is based on the number of
            // Result List Members plus any additional columns from the MergeMethod). The more dynamic columns we have, the more likely we are to
            // want to hide them on smaller screens, so we set the ShowWhen condition accordingly.
            _dynamicColumnIndex = additionalColumns.Count;

            for (int i = 0; i < this.ResultListsMembers.Count; i++) {
                var resultList = this.ResultListsMembers[i];
                var resultListMember = this.MergedResultList.ResultListMembers[i];

                rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                    Header = resultListMember.HeaderName,
                    Body = $"{{{resultListMember.HeaderName}}}",
                    BodyValues = new List<ResultListCellValue>() {
                        new ResultListCellValue() {
                            Text = $"{{{resultListMember.HeaderName}}}"
                        }
                    },
                    ClassSet = new List<ClassSet>() { new ClassSet() {
                        Name = "rlf-col-event",
                        ShowWhen = ShowWhenVariable.CreateAlwaysShow()
                    }},
                    ShowWhen = new ShowWhenVariable() {
                        Condition = GetNextDynamicColumnShowWhen()
                    }
                } );
            }

            foreach (var additionalColumn in _mergeMethod.AdditionalDisplayColumns) {
                rlf.Format.Columns.Add( additionalColumn );
            }

            rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = _mergeMethod.TopLevelHeaderText,
                Body = "{Aggregate}",
                BodyValues = new List<ResultListCellValue>() {
                        new ResultListCellValue() {
                            Text = "{Aggregate}"
                        }
                    },
                ClassSet = new List<ClassSet>() { new ClassSet() {
                        Name = "rlf-col-event",
                        ShowWhen = ShowWhenVariable.CreateAlwaysShow()
                    }}
            } );

            this.ResultListFormat = rlf;

        }

        internal ShowWhenCondition GetNextDynamicColumnShowWhen() {
            switch (_dynamicColumnIndex++) {
                case 0:
                case 1:
                    return ShowWhenCondition.DIMENSION_SMALL;
                case 2:
                case 3:
                case 4:
                    return ShowWhenCondition.DIMENSION_MEDIUM;
                case 5:
                case 6:
                case 7:
                case 8:
                    return ShowWhenCondition.DIMENSION_LARGE;
                default:
                    return ShowWhenCondition.DIMENSION_EXTRA_LARGE;
            }
        }

        /// <summary>
        /// Method dynamically auto-generate a RANKING RULE definition. If the concrete class MergeMethod can generate a ranking rule as indicated by
        /// <see cref="MergeMethod.CanGenerateRankingRule"/>, the RankingRule generated by the concrete class <see cref="MergeMethod.GenerateRankingRule"/> will be used. Otherwise,
        /// it will generate a default ranking rule based on the <see cref="ResultListMember">members</see> and <see cref="MergeConfiguration">configuration</see>."/>
        /// <para>Typically not called directly (although you can), instead the <see cref="CreateAsync(MergedResultList)"/> calls this methos when it initializes.</para>
        /// <para>Users may also use their own specified RANKNG RULE by setting the .RankingRule property.</para>
        /// </summary>
        public void AutoGenerateRankingRule() {
            RankingRule rr;
            if (_mergeMethod.CanGenerateRankingRule) {
                // If CanGenerateRankingRule is true, then we should be able to generate a ranking rule from the configuration. If we can't, then that's mean's the person who set up the configuration made a mistake.
                rr = _mergeMethod.GenerateRankingRule();

                if (rr is not null) {
                    // This is the expected path.
                    this.RankingRule = rr;
                    return;
                } else {
                    Debug.Fail( "MergedResultList.Configuration.CanGenerateRankingRule was true, but GenerateRankingRule() returned null. This should not happen, and likely means the person who set up the configuration didn't implement GenerateRankingRule()." );
                    // Will fall through to the auto generation code below, which is better than crashing, but the results might not be what the user expected.
                }
            }

            rr = new RankingRule();
            rr.SetDefaultValues();
            var rankingRule = rr.RankingRules[0];
            rankingRule.Rules.Clear();

            //TODO: The next three lines of code assumes the Standard Score Formats. Need to make this more generic.
            var source = TieBreakingRuleScoreSource.D;
            if (this.MergedResultList.Configuration.ScoreConfigName != "Decimal")
                source = TieBreakingRuleScoreSource.IX;

            rankingRule.Rules.Add( new TieBreakingRuleScore() {
                EventName = _mergeMethod.TopLevelEventname,
                SortOrder = SortBy.DESCENDING,
                Source = source,
                Comment = "Auto generated default Tie Breaking Rule"
            } );

            for (int i = this.ResultListsMembers.Count - 1; i >= 0; i--) {
                var resultList = this.ResultListsMembers[i];
                var resultListMember = this.MergedResultList.ResultListMembers[i];

                var key = ResultEvent.KeyForResultCofScore( resultList.MatchID, resultList.EventName );

                rankingRule.Rules.Add( new TieBreakingRuleScore() {
                    EventName = key,
                    SortOrder = SortBy.DESCENDING,
                    Source = source,
                    Comment = "Auto generated default Tie Breaking Rule"
                } );
            }

            this.RankingRule = rr;
        }

        /// <summary>
        /// This method identifies participants that are
        /// in one or more of the Tournament <see cref="ResultListMember">members</see>, and then asks the <see cref="MergeMethod"/> to
        /// perform its merging calculation.
        /// </summary>
        /// <returns></returns>
        public async Task<ResultList> MergeAsync() {

            /*
             * The objective of this for loop is two things. 
             * 1) Make a list of participants (which might be athletes or teams) who competed in at least one of the Result List Members. 
             * 2) For each of these participants, make an easy to use dictionary of the top level scores they shot for each Result List Member.
             * The easy to use dictionary is the .ResultCofScores dictionary that each Result Event instance has.
             */

            //QUESTION should this method re-pull the Result List Members to get the latest versions of them ? 

            //Foreach Result List Member
            _mergedResultEvents.Clear();
            foreach (var resultListMember in ResultListsMembers) {

                //key to save to the ResultEvent.ResultCofScores dictionary
                //EKA Question: Can the key just be the Result List member's Match ID ? 
                string key;
                string eventName;
                EventScore eventScore;

                //Foreach participant in the Result List Member list of people or teams who shot.
                foreach (var mergingResultEvent in resultListMember.Items) {

                    if (_mergedResultEvents.TryGetValue( mergingResultEvent.Participant.UniqueMergeId, out ResultEvent mergedResultEvent )) {
                        //This is from a Participant we previously found. 
                        //Add each of the EventScores to the mergedResultEvent under the ResultCofScores dictionary.
                        foreach (var es in mergingResultEvent.EventScores) {
                            eventName = es.Key;
                            eventScore = es.Value;
                            key = ResultEvent.KeyForResultCofScore( resultListMember.MatchID, eventName );
                            mergedResultEvent.ResultCofScores[key] = eventScore;
                            eventScore.Participant = mergingResultEvent.Participant;
                        }
                    } else {
                        //This is from a Participant we have not previously found. Create a new ResultEvent and store the top level score in it.
                        var newResultEvent = new ResultEvent();
                        newResultEvent.Participant = mergingResultEvent.Participant.Clone();
                        newResultEvent.ResultCofScores = new Dictionary<string, EventScore>();
                        newResultEvent.EventScores = new Dictionary<string, EventScore>();

                        //Add each of the EventScores to the mergedResultEvent under the ResultCofScores dictionary.
                        foreach (var es in mergingResultEvent.EventScores) {
                            eventName = es.Key;
                            eventScore = es.Value;
                            key = ResultEvent.KeyForResultCofScore( resultListMember.MatchID, eventName );
                            newResultEvent.ResultCofScores[key] = eventScore;
                            eventScore.Participant = mergingResultEvent.Participant;
                        }

                        _mergedResultEvents.Add( mergingResultEvent.Participant.UniqueMergeId, newResultEvent );
                    }
                }
            }

            ResultList rl = new ResultList();
            // NOTE: Normally a ResultList requires a COURSE OF FIRE definition, but in this case we are merging together Result Lists that may have different
            // Course of Fire definitions, so we can't really assign a meaningful Course of Fire definition to this merged Result List.
            rl.CourseOfFireDef = new SetName();

            // rl.EventName is the same as the top level event name that the MergeMethod uses to calculate the merged score. Will be something like {MatchID}_{HumanReadableTopLevelEventName}.
            rl.EventName = _mergeMethod.TopLevelEventname;

            //EAch ResultEvent instance that we created in the above for loop, now becomes the basis of the .Items array in our new merged Result List.
            List<ResultEvent> thePotentials = new List<ResultEvent>();
            thePotentials.AddRange( _mergedResultEvents.Values );

            // NOTE The .merge() method below is what addes the top level event to each participant's .ResultCofScores dictionary.

            // And now we merge.
            foreach (var re in thePotentials) {
                if (_mergeMethod.Merge( re )) {
                    //Add to the final Result List if the MergeMethod's .Merge() method returns true, which means this participant should be included in the merged results.
                    rl.Items.Add( re );
                }
            }

            //EKA Note: After merging, I'm thinking we should also rank it.

            return rl;
        }

        #endregion
    }
}
