using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.Tournaments {

    public class TournamentMerger {

        public Match Tournament { get; private set; }
        public List<Match> MatchesToMerge { get; private set; } = new List<Match>();
        public string ResultListNameToMerge { get; set; } = string.Empty;

        private Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private OrionMatchAPIClient _apiClient = new OrionMatchAPIClient();

        private Dictionary<MatchID, ResultList> _subMatchResultLists = new Dictionary<MatchID, ResultList>();
        private Dictionary<int, ResultEvent> _mergedResultEvents = new Dictionary<int, ResultEvent>();

        public ResultListFormat? ResultListFormat { get; set; } = null;

        public RankingRule RankingRule { get; set; } = null;

        public TournamentMerger( Match tournament, string resultListNameToMerge ) {
            this.Tournament = tournament;
            this.ResultListNameToMerge = resultListNameToMerge;
        }

        public void AddMatch( Match match ) {
            if (MatchesToMerge.Contains( match )) return;
            MatchesToMerge.Add( match );
        }

        public void AutoGenerateResultListFormat () {
            throw new NotImplementedException();
        }

        public void AutoGenerateRankingRule() {
            throw new NotImplementedException();
        }

        public async Task<ResultList> MergeAsync() {

            foreach (var subMatch in MatchesToMerge) {
                //TODO Write in to parallel code
                //Make safe
                //Load rest of result lis
                var getResultListResponse = await _apiClient.GetResultListPublicAsync( subMatch.GetParentId(), this.ResultListNameToMerge );
                if ( getResultListResponse.HasOkStatusCode )
                    _subMatchResultLists.Add( subMatch.GetParentId(), getResultListResponse.ResultList );
            }

            foreach (var subMatchResultList in _subMatchResultLists) {
                foreach( var subMatchResultEvent in subMatchResultList.Value.Items ) { 
                    if ( _mergedResultEvents.TryGetValue( subMatchResultEvent.Participant.UniqueMergeId, out ResultEvent mergedResultEvent )) {
                        //This is from a Participant we previously found
                        if (subMatchResultEvent.EventScores.TryGetValue( subMatchResultList.Value.EventName, out EventScore eventScore )) {
                            mergedResultEvent.ResultCOFScores.Add( subMatchResultList.Key, eventScore );
                        }
                    } else {
                        //This is from a Participant we have not previously found
                        var newResultEvent = new ResultEvent();
                        newResultEvent.Participant = subMatchResultEvent.Participant.Clone();
                        newResultEvent.ResultCOFScores = new Dictionary<MatchID, EventScore>();
                        if (subMatchResultEvent.EventScores.TryGetValue( subMatchResultList.Value.EventName, out EventScore eventScore )) {
                            newResultEvent.ResultCOFScores.Add( subMatchResultList.Key, eventScore );
                        }
                        _mergedResultEvents.Add( subMatchResultEvent.Participant.UniqueMergeId, newResultEvent );
                    }
                }
            }

            ResultList rl = new ResultList();
            rl.Items.AddRange( _mergedResultEvents.Values );

            SumMethod sumMethodMerger = new SumMethod( this );
            foreach( var re in rl.Items ) {
                sumMethodMerger.Merge( re );
            }

            return rl;
        }
    }
}
