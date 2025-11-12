using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Score = Scopos.BabelFish.DataModel.Athena.Score;

namespace Scopos.BabelFish.DataActors.Tournaments {
    public class AverageMethod : MergeMethod {

        /// <summary>
        /// Each MergeMethod concrete class has a unique identifier. It is used in the serialization of MergedResultLists instances
        /// to identify how the Merged Result List should be calculated.
        /// </summary>
        public const string IDENTIFIER = "Average";

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public AverageMethod( TournamentMerger tournamentMerger, AverageMethodConfiguration configuration ) : base( tournamentMerger, configuration ) {

            this.TopLevelEventname = "Average";
        }

        /// <inheritdoc />
        public override async Task InitializeAsync() {

            this.TournamentMerger.AutoGenerateResultListFormat();
            this.TournamentMerger.AutoGenerateRankingRule();
        }

        public AverageMethodConfiguration MergeConfiguration {
            get {
                return (AverageMethodConfiguration)_mergeConfiguration;
            }
        }

        /// <inheritdoc />
        public override void Merge( ResultEvent re ) {

            EventScore mergedEventScore = new EventScore();
            int count = 0;

            foreach (var resultListMember in TournamentMerger.ResultListsMembers) {
                var key = ResultEvent.KeyForResultCofScore( resultListMember.MatchID, resultListMember.EventName );

                if (re.ResultCofScores.TryGetValue( key, out EventScore eventScore )) {
                    
                    if (eventScore.Score != null && ! eventScore.Score.IsZero) {
                        mergedEventScore.Score += eventScore.Score;
                        count++;

                        if (eventScore.Projected != null) {
                            if (eventScore.Projected.IsZero) {
                                mergedEventScore.Projected += eventScore.Score;
                            } else {
                                mergedEventScore.Projected += eventScore.Projected;
                            }
                        }
                    }
                }
            }

            mergedEventScore.Score /= count;
            mergedEventScore.Projected /= count;

            re.ResultCofScores[ResultEvent.KeyForResultCofScore( TournamentMerger.Tournament.TournamentId, this.TopLevelEventname) ] = mergedEventScore;
        }
    }
}
