using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.Tournaments {

    public class TournamentMerger {

        public Match Tournament { get; private set; }
        public string MergedEventName { get; private set; }

        private Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private OrionMatchAPIClient _apiClient = new OrionMatchAPIClient();

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

        public TournamentMerger( Match tournament, string mergedEventName ) {
            this.Tournament = tournament;
            this.MergedEventName = mergedEventName;
        }

        /// <summary>
        /// Adds a new Result List as a Result List Member.
        /// </summary>
        /// <param name="resultList"></param>
        public void AddResultListMember( ResultList resultList ) {
            if (ResultListsMembers.Contains( resultList )) return;
            ResultListsMembers.Add( resultList );
        }

        public void AutoGenerateResultListFormat() {
            var rlf = new ResultListFormat();
            rlf.SetDefaultValues();
            rlf.Fields.Clear();

            rlf.Fields.Add( new ResultListField() {
                FieldName = "Aggregate",
                Method = ResultFieldMethod.SCORE,
                Source = new FieldSource() {
                    Name = this.KeyToResultCofEventScore,
                    ScoreFormat = "Events"
                }
            } );

            List<string> fieldNames = new List<string>() { "Shotgun", "Rifle", "Pistol" };

            for (int i = this.ResultListsMembers.Count - 1; i >= 0; i--) {
                var resultList = this.ResultListsMembers[i];
                var key = ResultEvent.KeyForResultCofScore( resultList.MatchID, resultList.EventName );

                rlf.Fields.Add( new ResultListField() {
                    FieldName = fieldNames[i],
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

            for (int i = this.ResultListsMembers.Count - 1; i >= 0; i--) {
                rlf.Format.Columns.Add( new ResultListDisplayColumn() {
                    Header = fieldNames[i],
                    Body = "{" + fieldNames[i] + "}",
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

            rankingRule.Rules.Add( new TieBreakingRuleScore() {
                EventName = this.KeyToResultCofEventScore,
                SortOrder = SortBy.DESCENDING,
                Source = TieBreakingRuleScoreSource.IX,
                Comment = "Auto generated default Tie Breaking Rule"
            } );

            for( int i = this.ResultListsMembers.Count - 1; i>=0; i-- ) {
                var resultList = this.ResultListsMembers[i];
                var key = ResultEvent.KeyForResultCofScore( resultList.MatchID, resultList.EventName );

                rankingRule.Rules.Add( new TieBreakingRuleScore() {
                    EventName = key,
                    SortOrder = SortBy.DESCENDING,
                    Source = TieBreakingRuleScoreSource.IX,
                    Comment = "Auto generated default Tie Breaking Rule"
                } );
            }

            this.RankingRule = rr;
        }

        public string KeyToResultCofEventScore {
            get {
                return ResultEvent.KeyForResultCofScore( this.Tournament.MatchID, this.MergedEventName );
            }
        }

        public async Task<ResultList> MergeAsync() {

            /*
             * The objective of this for loop is two things. 
             * 1) Make a list of participants (which might be athletes or teams) who competed in at least one of the Result List Members. 
             * 2) For each of these participants, make an easy to use dictionary of the top level scores they shot for each Result List Member.
             * The easy to use dictionary is the .ResultCofScores dictionary that each Result Event instance has.
             */

            //Foreach Result List Member
            _mergedResultEvents.Clear();
            foreach (var resultListMember in ResultListsMembers) {

                //key to save to the ResultEvent.ResultCofScores dictionary
                //EKA Question: Can the key just be the Result List member's Match ID ? 
                var key = ResultEvent.KeyForResultCofScore( resultListMember.MatchID, resultListMember.EventName );

                //Foreach participant in the Result List Member list of people or teams who shot.
                foreach( var mergingResultEvent in resultListMember.Items ) { 

                    if ( _mergedResultEvents.TryGetValue( mergingResultEvent.Participant.UniqueMergeId, out ResultEvent mergedResultEvent )) {
                        //This is from a Participant we previously found. Add the top level scores to existing Result Event instance.
                        if (mergingResultEvent.EventScores.TryGetValue( resultListMember.EventName, out EventScore eventScore )) {
                            mergedResultEvent.ResultCofScores.Add( key, eventScore );
                        }
                    } else {
                        //This is from a Participant we have not previously found. Create a new ResultEvent and store the top level score in it.
                        var newResultEvent = new ResultEvent();
                        newResultEvent.Participant = mergingResultEvent.Participant.Clone();
                        newResultEvent.ResultCofScores = new Dictionary<string, EventScore>();
                        if (mergingResultEvent.EventScores.TryGetValue( resultListMember.EventName, out EventScore eventScore )) {
                            newResultEvent.ResultCofScores.Add( key, eventScore );
                        }
                        _mergedResultEvents.Add( mergingResultEvent.Participant.UniqueMergeId, newResultEvent );
                    }
                }
            }

            ResultList rl = new ResultList();
            //Each ResultList instance needs a COURSE OF FIRE definition. However, these merged result lists are dynamic ... so not sure yet what to put as the .CourseOfFireDef
            rl.CourseOfFireDef = "v1.0:ntparc:40 Shot Standing";
            rl.EventName = this.KeyToResultCofEventScore;
            //EAch ResultEvent instance that we created in the above for loop, now becomes the basis of the .Items array in our new merged Result List.
            rl.Items.AddRange( _mergedResultEvents.Values );

            SumMethod sumMethodMerger = new SumMethod( this );
            foreach( var re in rl.Items ) {
                sumMethodMerger.Merge( re );
            }

            //EKA Note: After merging, I'm thinking we should also rank it.

            return rl;
        }
    }
}
