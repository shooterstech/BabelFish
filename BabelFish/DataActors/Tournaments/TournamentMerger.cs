using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.Tournaments {

    public class TournamentMerger {

        public Tournament Tournament { get; private set; }

        /// <summary>
        /// The name of the merged result list.
        /// <para>Set during the construction of the TournamentMerger. Must be a name listed in the Tournament's .MergedResultLists property.</para>
        /// </summary>
        public string ResultName { get; private set; }

        private MergeMethod _mergeMethod;
        private MergedResultList _mergeResultList;

        private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private static OrionMatchAPIClient _apiClient = new OrionMatchAPIClient();

        /// <summary>
        /// The set of Result Lists that will be merged.
        /// </summary>
        public List<ResultList> ResultListsMembers { get; private set; } = new List<ResultList>();

        /// <summary>
        /// Dictionary of all the participants (teams or athletes) that competed in at least one of the
        /// Result List Members. 
        /// <para>The key is a unique id identifying the participant.</para>
        /// <para>The value is a ResultEvent instance that holds the scores from the Result List Member. This ResultEvent 
        /// instance has its dictionary called .ResultCofScores where the scores are stored.</para>
        /// </summary>
        private Dictionary<int, ResultEvent> _mergedResultEvents = new Dictionary<int, ResultEvent>();

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

        private TournamentMerger(  ) {
        }

        public static async Task<TournamentMerger> FactoryAsync( Tournament tournament, string resultName ) {

            //Test that resultName is found in the tournament's .MergedResultLists list.
            if (!tournament.MergedResultLists.Any( mrl => mrl.ResultName == resultName )) {
                var msg = $"The value for resultName, '{resultName}' was not found amongst the Tournament's MergedResultLists instances.";
                if (tournament.MergedResultLists.Count > 0) {
                    var expectedValues = string.Join( ", ", tournament.MergedResultLists.Select( item => item.ResultName ) );
                    msg += $" The value for resultName must be one of {expectedValues}.";
                } else {
                    msg += " The reason it was not found is, there are no .MergedResultLists to speak of.";
                }

                throw new ArgumentException( msg );
            }

            TournamentMerger tm = new TournamentMerger();
            tm.Tournament = tournament;
            tm.ResultName = resultName;

            tm._mergeResultList = tournament.MergedResultLists.First( mrl => mrl.ResultName == resultName );

            foreach( var rlm in tm._mergeResultList.ResultListMembers ) {
                var getResultListResponse = await _apiClient.GetResultListPublicAsync( rlm.MatchId, rlm.ResultName );
                if (getResultListResponse.HasOkStatusCode) {
                    tm.ResultListsMembers.Add( getResultListResponse.ResultList );
                }  else {
                    var msg = $"Could not add the Result List {rlm.ResultName} from {rlm.MatchId}. Received error '{getResultListResponse.OverallStatusCode}' and '{getResultListResponse.RestApiStatusCode}' instead.";
                    _logger.Error( msg );
                }
            }

            tm._mergeMethod = await MergeMethod.FactoryAsync( tm, tm._mergeResultList );

            return tm;
        }

        public void AutoGenerateResultListFormat() {
            var rlf = new ResultListFormat();
            rlf.SetDefaultValues();
            rlf.ScoreConfigDefault = _mergeResultList.ScoreConfigName;
            rlf.ScoreFormatCollectionDef = _mergeResultList.ScoreFormatCollectionDef.ToString();
            rlf.Fields.Clear();

            rlf.Fields.Add( new ResultListField() {
                FieldName = "Aggregate",
                Method = ResultFieldMethod.SCORE,
                Source = new FieldSource() {
                    Name = ResultEvent.KeyForResultCofScore( this.Tournament.TournamentId, _mergeMethod.TopLevelEventname ),
                    ScoreFormat = "Events"
                }
            } );

            for (int i = this._mergeResultList.ResultListMembers.Count - 1; i >= 0; i--) {
                var resultList = this.ResultListsMembers[i];
                var resultListMember = this._mergeResultList.ResultListMembers[i];

                var key = ResultEvent.KeyForResultCofScore( resultList.MatchID, resultList.EventName );

                rlf.Fields.Add( new ResultListField() {
                    FieldName = resultListMember.HeaderName,
                    Method = ResultFieldMethod.SCORE,
                    Source = new FieldSource() {
                        Name = key,
                        ScoreFormat = "Events"
                    }
                } );
            }

            rlf.Format.Columns.Clear();
            rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Rank",
                Body = "{RankOrSquadding} {RankDelta}",
                ClassSet = new List<ClassSet>() { new ClassSet() {
                    Name = "rlf-col-rank",
                    ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
                }}
            } );

            rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Participant",
                Body = "{DisplayName}",
                BodyLinkTo = LinkToOption.PublicProfile,
                ClassSet = new List<ClassSet>() { new ClassSet() {
                    Name = "rlf-col-participant",
                    ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
                }}
            } );

            for (int i = 0; i < this.ResultListsMembers.Count; i++) {
                var resultList = this.ResultListsMembers[i];
                var resultListMember = this._mergeResultList.ResultListMembers[i];

                rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                    Header = resultListMember.HeaderName,
                    Body = $"{{{resultListMember.HeaderName}}}",
                    ClassSet = new List<ClassSet>() { new ClassSet() {
                        Name = "rlf-col-event",
                        ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
                    }},
                    ShowWhen = new ShowWhenVariable() {
                        Condition = ShowWhenCondition.DIMENSION_LARGE
                    }
                } );
            }

            rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                Header = "Aggregate",
                Body = "{Aggregate}",
                ClassSet = new List<ClassSet>() { new ClassSet() {
                        Name = "rlf-col-event",
                        ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone()
                    }}
            } );

            this.ResultListFormat = rlf;

        }

        public void AutoGenerateRankingRule() {
            var rr = new RankingRule();
            rr.SetDefaultValues();
            var rankingRule = rr.RankingRules[0];
            rankingRule.Rules.Clear();

            //TODO: The next three lines of code assumes the Standard Score Formats. Need to make this more generic.
            var source = TieBreakingRuleScoreSource.D;
            if (_mergeResultList.ScoreConfigName != "Decimal")
                source = TieBreakingRuleScoreSource.IX;

            rankingRule.Rules.Add( new TieBreakingRuleScore() {
                EventName = ResultEvent.KeyForResultCofScore( this.Tournament.TournamentId, _mergeMethod.TopLevelEventname ),
                SortOrder = SortBy.DESCENDING,
                Source = source,
                Comment = "Auto generated default Tie Breaking Rule"
            } );

            for( int i = this.ResultListsMembers.Count - 1; i>=0; i-- ) {
                var resultList = this.ResultListsMembers[i];
                var resultListMember = this._mergeResultList.ResultListMembers[i];

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
                foreach( var mergingResultEvent in resultListMember.Items ) { 

                    if ( _mergedResultEvents.TryGetValue( mergingResultEvent.Participant.UniqueMergeId, out ResultEvent mergedResultEvent )) {
                        //This is from a Participant we previously found. 
                        //Add each of the EventScores to the mergedResultEvent under the ResultCofScores dictionary.
                        foreach( var es in mergingResultEvent.EventScores ) {
                            eventName = es.Key;
                            eventScore = es.Value;
                            key = ResultEvent.KeyForResultCofScore( resultListMember.MatchID, eventName );
                            mergedResultEvent.ResultCofScores[ key ] = eventScore;
                        }
                    } else {
                        //This is from a Participant we have not previously found. Create a new ResultEvent and store the top level score in it.
                        var newResultEvent = new ResultEvent();
                        newResultEvent.Participant = mergingResultEvent.Participant.Clone();
                        newResultEvent.ResultCofScores = new Dictionary<string, EventScore>();
                        newResultEvent.EventScores = new Dictionary<string, EventScore>();

                        //This is from a Participant we previously found. 
                        //Add each of the EventScores to the mergedResultEvent under the ResultCofScores dictionary.
                        foreach (var es in mergingResultEvent.EventScores) {
                            eventName = es.Key;
                            eventScore = es.Value;
                            key = ResultEvent.KeyForResultCofScore( resultListMember.MatchID, eventName );
                            newResultEvent.ResultCofScores[key] = eventScore;
                        }

                        _mergedResultEvents.Add( mergingResultEvent.Participant.UniqueMergeId, newResultEvent );
                    }
                }
            }

            ResultList rl = new ResultList();
            //Each ResultList instance needs a COURSE OF FIRE definition. However, these merged result lists are dynamic ... so not sure yet what to put as the .CourseOfFireDef
            rl.CourseOfFireDef = "v1.0:ntparc:40 Shot Standing";
            rl.EventName = _mergeResultList.ResultName;
            //EAch ResultEvent instance that we created in the above for loop, now becomes the basis of the .Items array in our new merged Result List.
            rl.Items.AddRange( _mergedResultEvents.Values );

            foreach( var re in rl.Items ) {
                _mergeMethod.Merge( re );
            }

            //EKA Note: After merging, I'm thinking we should also rank it.

            return rl;
        }
    }
}
