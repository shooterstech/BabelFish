using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Score = Scopos.BabelFish.DataModel.Athena.Score;

namespace Scopos.BabelFish.DataActors.Tournaments {
    public class SumMethod : TournamentMergeMethod {

        public SumMethod( TournamentMerger tournamentMerger ) : base( tournamentMerger ) { 
        
        }

        public override void Merge( ResultEvent re ) {

            EventScore mergedEventScore = new EventScore();

            foreach (var subMatch in TournamentMerger.MatchesToMerge) {
                if (re.ResultCOFScores.TryGetValue( subMatch.GetParentId(), out EventScore eventScore )) {
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

            re.ResultCOFScores[ TournamentMerger.Tournament.GetParentId() ] = mergedEventScore;
        }
    }
}
