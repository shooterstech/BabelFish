using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Score = Scopos.BabelFish.DataModel.Athena.Score;

namespace Scopos.BabelFish.DataActors.Tournaments {
    public class SumMethod : MergeMethod {


        public SumMethod( TournamentMerger tournamentMerger, SumMethodConfiguration configuration ) : base( tournamentMerger, configuration ) {

        }

        public SumMethodConfiguration MergeConfiguration {
            get {
                return (SumMethodConfiguration)_mergeConfiguration;
            }
        }

        public override void Merge( ResultEvent re ) {

            EventScore mergedEventScore = new EventScore();

            foreach (var mergingResultList in TournamentMerger.ResultListsMembers) {
                var key = ResultEvent.KeyForResultCofScore( mergingResultList.MatchID, mergingResultList.EventName );

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

            re.ResultCofScores[ TournamentMerger.KeyToResultCofEventScore ] = mergedEventScore;
        }
    }
}
