using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Score = Scopos.BabelFish.DataModel.Athena.Score;

namespace Scopos.BabelFish.DataActors.Tournaments {
    public class SumMethod : MergeMethod {

        /// <summary>
        /// Each MergeMethod concrete class has a unique identifier. It is used in the serialization of MergedResultLists instances
        /// to identify how the Merged Result List should be calculated.
        /// </summary>
        public const string IDENTIFIER = "Sum";

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public SumMethod( TournamentMerger tournamentMerger, SumMethodConfiguration configuration ) : base( tournamentMerger, configuration ) {

            this.TopLevelEventname = "Aggregate";
        }

        /// <inheritdoc />
        public override async Task InitializeAsync() {

            this.TournamentMerger.AutoGenerateResultListFormat();
            this.TournamentMerger.AutoGenerateRankingRule();
        }

        public SumMethodConfiguration MergeConfiguration {
            get {
                return (SumMethodConfiguration)_mergeConfiguration;
            }
        }

        /// <inheritdoc />
        public override void Merge( ResultEvent re ) {

            EventScore mergedEventScore = new EventScore();
            

            foreach (var resultListMember in TournamentMerger.ResultListsMembers) {
                var key = ResultEvent.KeyForResultCofScore( resultListMember.MatchID, resultListMember.EventName );

                if (re.ResultCofScores.TryGetValue( key, out EventScore eventScore )) {
                    if (eventScore.Score != null) {
                        mergedEventScore.Score += eventScore.Score;

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

            re.ResultCofScores[ResultEvent.KeyForResultCofScore( TournamentMerger.Tournament.TournamentId, this.TopLevelEventname) ] = mergedEventScore;
        }
    }
}
