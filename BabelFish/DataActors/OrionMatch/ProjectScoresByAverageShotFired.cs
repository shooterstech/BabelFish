using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {
    public class ProjectScoresByAverageShotFired : ProjectorOfScores {

        public ProjectScoresByAverageShotFired( CourseOfFire courseOfFire  ) :base( courseOfFire ) { 
        }

        /// <inheritdoc/>
        /// <param name="projection"></param>
        public override void ProjectEventScores( IEventScoreProjection projection ) {

            //The top level event should (better be) the only Event Type == EVENT.
            var topLevelEvent = EventComposite.GrowEventTree( this.CourseOfFire );
            var stageStyleEvents = topLevelEvent.GetEvents( false, false, true, false, false, false );

            //note, this is not safe
            var topLevelEventScore = projection.EventScores[topLevelEvent.EventName];


            foreach (var stageEvent in stageStyleEvents) {
                //note, this is also not safe
                var es = projection.EventScores[stageEvent.EventName];

                //project the scores, currently by some fake method
                es.Projected = new DataModel.Athena.Score();
                es.Projected.X = es.Score.X + 1;
                es.Projected.I = es.Score.I + 1;
                es.Projected.D = es.Score.D + 1.1f;
            }

            //Make the bold and wildly incorrect assumption that the topLevelEvent is the sum of the stages

            //Need to instantiate .Projected as it is by default null
            topLevelEventScore.Projected = new DataModel.Athena.Score();

            foreach (var stageEvent in stageStyleEvents) {
                //note, still not safe
                var es = projection.EventScores[stageEvent.EventName];

                topLevelEventScore.Projected.X = es.Projected.X;
                topLevelEventScore.Projected.I = es.Projected.I;
                topLevelEventScore.Projected.D = es.Projected.D;
            }
        }
    }
}
