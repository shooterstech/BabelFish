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

        public List<ResultList> ResultListsToMerge { get; private set; } = new List<ResultList>();
        private Dictionary<int, ResultEvent> _mergedResultEvents = new Dictionary<int, ResultEvent>();

        public ResultListFormat? ResultListFormat { get; set; } = null;

        public RankingRule RankingRule { get; set; } = null;

        public TournamentMerger( Match tournament, string mergedEventName ) {
            this.Tournament = tournament;
            this.MergedEventName = mergedEventName;
        }

        public void AddResultList( ResultList resultList ) {
            if (ResultListsToMerge.Contains( resultList )) return;
            ResultListsToMerge.Add( resultList );
        }

        public void AutoGenerateResultListFormat () {
            throw new NotImplementedException();
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

            for( int i = this.ResultListsToMerge.Count - 1; i>=0; i-- ) {
                var resultList = this.ResultListsToMerge[i];
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

            foreach (var mergingResultList in ResultListsToMerge) {
                //key to save to the ResultEvent.ResultCofScores dictionary
                var key = ResultEvent.KeyForResultCofScore( mergingResultList.MatchID, mergingResultList.EventName );

                foreach( var mergingResultEvent in mergingResultList.Items ) { 
                    if ( _mergedResultEvents.TryGetValue( mergingResultEvent.Participant.UniqueMergeId, out ResultEvent mergedResultEvent )) {
                        //This is from a Participant we previously found
                        if (mergingResultEvent.EventScores.TryGetValue( mergingResultList.EventName, out EventScore eventScore )) {
                            mergedResultEvent.ResultCofScores.Add( key, eventScore );
                        }
                    } else {
                        //This is from a Participant we have not previously found
                        var newResultEvent = new ResultEvent();
                        newResultEvent.Participant = mergingResultEvent.Participant.Clone();
                        newResultEvent.ResultCofScores = new Dictionary<string, EventScore>();
                        if (mergingResultEvent.EventScores.TryGetValue( mergingResultList.EventName, out EventScore eventScore )) {
                            newResultEvent.ResultCofScores.Add( key, eventScore );
                        }
                        _mergedResultEvents.Add( mergingResultEvent.Participant.UniqueMergeId, newResultEvent );
                    }
                }
            }

            ResultList rl = new ResultList();
            rl.CourseOfFireDef = "v1.0:ntparc:40 Shot Standing";
            rl.Items.AddRange( _mergedResultEvents.Values );

            SumMethod sumMethodMerger = new SumMethod( this );
            foreach( var re in rl.Items ) {
                sumMethodMerger.Merge( re );
            }

            return rl;
        }
    }
}
