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

            EventScore topLevelEventScore;
            projection.EventScores.TryGetValue(topLevelEvent.EventName, out topLevelEventScore);
            int shotsInEvent = 0;

            foreach (var stageEvent in stageStyleEvents) {
                EventScore es;
                projection.EventScores.TryGetValue(stageEvent.EventName, out es);
                //get singulars for stage I am in, then count those and that is how many shots to take total.
                var singulars = stageEvent.GetEvents(false, false, false, false, false, true);
                var shotsFired = es.NumShotsFired;
                var shotsRemaining = singulars.Count - shotsFired;
                var avgShotThisStage = es.Score.D / shotsFired;

                //project the scores
                es.Projected = new DataModel.Athena.Score();
                es.Projected.X = (int)(es.Score.X + ((es.Score.X / shotsFired) * shotsRemaining));
                es.Projected.I = (int)(es.Score.I + (int)(avgShotThisStage * shotsRemaining));
                es.Projected.D = (1.0f) * (es.Score.D + (avgShotThisStage * shotsRemaining));

                shotsInEvent += singulars.Count;
            }

            //Make the bold and wildly incorrect assumption that the topLevelEvent is the sum of the stages

            //Need to instantiate .Projected as it is by default null
            topLevelEventScore.Projected = new DataModel.Athena.Score();

            foreach (var stageEvent in stageStyleEvents) {
                EventScore es;
                projection.EventScores.TryGetValue(stageEvent.EventName, out es);


                topLevelEventScore.Projected.X = es.Projected.X;
                topLevelEventScore.Projected.I = es.Projected.I;
                topLevelEventScore.Projected.D = es.Projected.D;
            }
        }
    }
}
