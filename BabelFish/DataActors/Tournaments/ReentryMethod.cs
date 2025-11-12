using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.Tournaments {
    public class ReentryMethod : MergeMethod {

        /// <summary>
        /// Each MergeMethod concrete class has a unique identifier. It is used in the serialization of MergedResultLists instances
        /// to identify how the Merged Result List should be calculated.
        /// </summary>
        public const string IDENTIFIER = "Reentry";

        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private EventComposite _topLevelEvent;

        public ReentryMethod( TournamentMerger tournamentMerger, ReentryMethodConfiguration configuration ) : base( tournamentMerger, configuration ) {

        }

        public override async Task InitializeAsync() {
            var cof = await MergeConfiguration.GetCourseOfFireDefinitionAsync();
            _topLevelEvent = EventComposite.GrowEventTree( cof );
            this.TopLevelEventname = _topLevelEvent.EventName;
            this.TournamentMerger.ResultListFormat = await MergeConfiguration.GetResultListFormatDefinitionAsync();
            this.TournamentMerger.RankingRule = await MergeConfiguration.GetRankingRuleDefinitionAsync();
        }

        public ReentryMethodConfiguration MergeConfiguration {
            get {
                return (ReentryMethodConfiguration)_mergeConfiguration;
            }
        }

        /// <inheritdoc />
        public override void Merge( ResultEvent re ) {

            EventScore mergedEventScore = new EventScore();
            List<EventComposite> reentryEvents = _topLevelEvent.GetEvents( MergeConfiguration.EventType );
            string key;

            //We are looking for the highest scoring EventScore from all the Result Lists

            //TODO read this value from somewhere
            SetName standardScoreFormat = SetName.Parse( "v1.0:orion:StandardScoreFormat" );
            var comparer = new CompareEventScore( standardScoreFormat, "Integer" );
            List<EventScore> eventScores = new List<EventScore>();
            EventScore highestEventScore;

            foreach (var reentryEvent in reentryEvents) {
                eventScores.Clear();

                foreach (var resultListMember in TournamentMerger.ResultListsMembers) {
                    key = ResultEvent.KeyForResultCofScore( resultListMember.MatchID, reentryEvent.EventName );
                    if (re.ResultCofScores.TryGetValue( key, out var score )) {
                        eventScores.Add( score );
                        score.MatchId = resultListMember.MatchID;
                    }
                }

                if (eventScores.Count > 0) {
                    eventScores.Sort( comparer );
                    highestEventScore = eventScores[0];
                    re.EventScores.Add( reentryEvent.EventName, highestEventScore );
                    
                    foreach( var childEvent in reentryEvent.Children ) {
                        key = ResultEvent.KeyForResultCofScore( highestEventScore.MatchId, childEvent.EventName );
                        if ( re.ResultCofScores.TryGetValue( key, out var score )) {
                            re.EventScores.Add( childEvent.EventName, score );
                        }
                    }
                }
            }

            //TODO calculare the reentryEvent's paretns scores, all the way to the top level event.
            //These next few lines is meant to be temporary.
            if (MergeConfiguration.EventType != EventtType.EVENT) {
                EventScore aggregate = new EventScore();
                foreach (var reentryEvent in reentryEvents) {
                    if ( re.EventScores.TryGetValue( reentryEvent.EventName, out EventScore es )) {
                        aggregate.Score += es.Score;
                        //aggregate.Projected += es.Projected;
                    }
                }
                re.EventScores.Add( _topLevelEvent.EventName, aggregate );
            }
        }
    }
}
