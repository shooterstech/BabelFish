using System;
using System.Collections.Generic;
using System.Text;
using Amazon.Runtime.Internal.Transform;
using Scopos.BabelFish.DataModel.AthenaLogin;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {
    public class ProjectScoresByAverageShotFired : ProjectorOfScores {

        public ProjectScoresByAverageShotFired( CourseOfFire courseOfFire  ) :base( courseOfFire ) { 
        }

        public override string ProjectionMadeBy {
            get {
                return "BabelFish ProjectScoresByAverageShotFired";
            }
        }

        /// <inheritdoc/>
        /// <param name="projection"></param>
        public override void ProjectEventScores( IEventScoreProjection projection ) {

            //The top level event should (better be) the only Event Type == EVENT.
            var topLevelEvent = EventComposite.GrowEventTree( this.CourseOfFire );
            var stageStyleEvents = topLevelEvent.GetEvents( false, false, true, false, false, false );

            EventScore topLevelEventScore;
            projection.EventScores.TryGetValue(topLevelEvent.EventName, out topLevelEventScore);

            EventScore es;
            //first key is eventName, second key is avgType [(I)INT, (D)DEC, (X)INNER] value is avg.
            Dictionary<string, Dictionary<string, float>> avgShots = new Dictionary<string, Dictionary<string, float>>();

            //I could probably just use singulars and get the complete avg shot fired and use that instead, might be easier...
            foreach (var stageEvent in stageStyleEvents)
            {
                projection.EventScores.TryGetValue(stageEvent.EventName, out es);
                //get singulars for stage I am in, then count those and that is how many shots to take total.
                var singulars = stageEvent.GetEvents(false, false, false, false, false, true);
                var shotsFired = es.NumShotsFired;
                //we want to always project shots if we have ANY remaining.
                var shotsRemaining = singulars.Count - shotsFired;
                var avgIntThisStage = 0.0f;
                var avgDecThisStage = 0.0f;
                var avgXPerShot = 0.0f;
                if (shotsFired > 0 && singulars.Count > 0)
                { //if there are shots fired in this stage, and there should be, we should use those 
                    avgIntThisStage = (float)es.Score.I / (float)shotsFired;
                    avgDecThisStage = es.Score.D / shotsFired;
                    avgXPerShot = (float)es.Score.X / (float)shotsFired;

                    //add this tot he avgShots dict, which gets avgd at the end for stages we know nothing about.
                    avgShots.Add(stageEvent.EventName, new Dictionary<string, float>());
                    avgShots[stageEvent.EventName].Add("I", avgIntThisStage);
                    avgShots[stageEvent.EventName].Add("D", avgDecThisStage);
                    avgShots[stageEvent.EventName].Add("X", avgXPerShot);
                }
                else
                {
                    continue;
                }

                //project the scores
                es.Projected = new DataModel.Athena.Score();
                es.Projected.I = es.Score.I + (int)(avgIntThisStage * shotsRemaining);
                es.Projected.D = es.Score.D + (avgDecThisStage * shotsRemaining);
                es.Projected.D = (float)Math.Round(es.Projected.D, 1);
                es.Projected.X = (int)(es.Score.X + (avgXPerShot * shotsRemaining));
            }

            foreach (var stageEvent in stageStyleEvents)
            { // loop through the stage styles again, this time hitting anything we dont have any shots for.

                projection.EventScores.TryGetValue(stageEvent.EventName, out es);
                var singulars = stageEvent.GetEvents(false, false, false, false, false, true);
                var shotsFired = es.NumShotsFired;
                if(shotsFired > 0)
                {
                    continue;
                }

                //we want to always project shots if we have ANY remaining.
                var shotsRemaining = singulars.Count;
                var avgIntThisStage = 0.0f;
                var avgDecThisStage = 0.0f;
                var avgXPerShot = 0.0f;

                if (avgShots.Count > 0 )
                {
                    //this step should only be attempted IF there are any avg shots
                    var defaulting = GetAverageShot(avgShots);
                    avgIntThisStage = defaulting["I"];
                    avgDecThisStage = defaulting["D"];
                    avgXPerShot = defaulting["X"];
                    //add this to the avgShots dict for this stage.
                    avgShots.Add(stageEvent.EventName, new Dictionary<string, float>());
                    avgShots[stageEvent.EventName].Add("I", avgIntThisStage);
                    avgShots[stageEvent.EventName].Add("D", avgDecThisStage);
                    avgShots[stageEvent.EventName].Add("X", avgXPerShot);
                }

                //project the scores
                es.Projected = new DataModel.Athena.Score();
                es.Projected.I = es.Score.I + (int)(avgIntThisStage * shotsRemaining);
                es.Projected.D = es.Score.D + (avgDecThisStage * shotsRemaining);
                es.Projected.D = (float)Math.Round(es.Projected.D, 1);
                es.Projected.X = (int)(es.Score.X + (avgXPerShot * shotsRemaining));
            }

            //Make the bold and wildly incorrect assumption that the topLevelEvent is the sum of the stages

            //Need to instantiate .Projected as it is by default null
            topLevelEventScore.Projected = new DataModel.Athena.Score();

            foreach (var stageEvent in stageStyleEvents)
            {
                projection.EventScores.TryGetValue(stageEvent.EventName, out es);
                //Add all the stage projected scores together, this will be the top level event, hopefully.
                topLevelEventScore.Projected.I += es.Projected.I;
                topLevelEventScore.Projected.D += es.Projected.D;
                topLevelEventScore.Projected.X += es.Projected.X;
            }
            topLevelEventScore.Projected.D = (float)Math.Round(topLevelEventScore.Projected.D, 1);
        }

        private Dictionary<string, float> GetAverageShot(Dictionary<string,Dictionary<string,float>> allAvgs )
        {
            var avgInt = 0.0f;
            var avgDec = 0.0f;
            var avgX = 0.0f;
            foreach (var @event in allAvgs)
            {
                avgInt += @event.Value["I"];
                avgDec += @event.Value["D"];
                avgX += @event.Value["X"];
            }
            Dictionary<string, float> bigAvgs = new Dictionary<string, float>
            {
                {"I", avgInt/(float)allAvgs.Count},
                {"D", avgDec/(float)allAvgs.Count},
                {"X", avgX/(float)allAvgs.Count},
            };
            return bigAvgs;
        }
    }
}
