using System;
using System.Collections.Generic;
using System.Text;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Runtime.Internal.Transform;
using Scopos.BabelFish.DataModel.AthenaLogin;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Score = Scopos.BabelFish.DataModel.Athena.Score;

namespace Scopos.BabelFish.DataActors.OrionMatch {
    public class ProjectScoresByAverageShotFired : ProjectorOfScores {

        public ProjectScoresByAverageShotFired( CourseOfFire courseOfFire ) : base( courseOfFire ) {
        }

        public override string ProjectionMadeBy {
            get {
                return "BabelFish ProjectScoresByAverageShotFired";
            }
        }

        //Projection and AvgShots exists as class variable to make recussion easier. They need to be cleared with each call to ProjectEventScores()
        private IEventScoreProjection Projection { get; set; }

        //first key is stage style setname, second key is avgType [(I)INT, (D)DEC, (X)INNER] value is avg.
        private Dictionary<string, Dictionary<string, float>> AvgShots = new Dictionary<string, Dictionary<string, float>>();

        /// <inheritdoc/>
        /// <param name="projection"></param>
        public override void ProjectEventScores( IEventScoreProjection projection ) {

            this.Projection = projection;
            this.AvgShots.Clear();

            var stageEvents = this.TopLevelEvent.GetEvents( false, false, true, false, false, false );

            EventScore topLevelEventScore;
            //If the event name doesn't exist in EventScores, which shouldn't ever happen, there isn't much we can do, so purposefully not setting a .Projection Score
            if (projection.EventScores.TryGetValue( this.TopLevelEvent.EventName, out topLevelEventScore )) {

                EventScore es;

                //I could probably just use singulars and get the complete avg shot fired and use that instead, might be easier...
                foreach (var stageEvent in stageEvents) {
                    this.Projection.EventScores.TryGetValue( stageEvent.EventName, out es );

                    //Every EventScore *should* have a StageStyleDef, if it does not, we have no option but to skip it

                    //get singulars for stage I am in, then count those and that is how many shots to take total.
                    var singulars = stageEvent.GetEvents( false, false, false, false, false, true );
                    var shotsFired = es.NumShotsFired;
                    //we want to always project shots if we have ANY remaining.
                    var shotsRemaining = singulars.Count - shotsFired;
                    var avgIntThisStage = 0.0f;
                    var avgDecThisStage = 0.0f;
                    var avgXPerShot = 0.0f;
                    if (shotsFired > 0 && singulars.Count > 0) { //if there are shots fired in this stage, and there should be, we should use those 
                        avgIntThisStage = (float)es.Score.I / (float)shotsFired;
                        avgDecThisStage = es.Score.D / shotsFired;
                        avgXPerShot = (float)es.Score.X / (float)shotsFired;

                        if (!string.IsNullOrEmpty( es.StageStyleDef )) {
                            //add this tot he avgShots dict, which gets avgd at the end for stages we know nothing about.
                            //NOTE that if there are two stages with the same stage style, only the first gets added. Which has room for improvement.
                            if (!AvgShots.ContainsKey( es.StageStyleDef )) {
                                AvgShots.Add( es.StageStyleDef, new Dictionary<string, float>() );
                                AvgShots[es.StageStyleDef].Add( "I", avgIntThisStage );
                                AvgShots[es.StageStyleDef].Add( "D", avgDecThisStage );
                                AvgShots[es.StageStyleDef].Add( "X", avgXPerShot );
                            }
                        }
                    } else {
                        continue;
                    }

                    //project the scores
                    es.Projected = new DataModel.Athena.Score();
                    es.Projected.I = es.Score.I + (int)(avgIntThisStage * shotsRemaining);
                    es.Projected.D = es.Score.D + (avgDecThisStage * shotsRemaining);
                    es.Projected.D = (float)Math.Round( es.Projected.D, 1 );
                    es.Projected.S = es.Projected.D;
                    es.Projected.X = (int)(es.Score.X + (avgXPerShot * shotsRemaining));

                }

                foreach (var stageEvent in stageEvents) { // loop through the stage styles again, this time hitting anything we dont have any shots for.

                    this.Projection.EventScores.TryGetValue( stageEvent.EventName, out es );
                    var singulars = stageEvent.GetEvents( false, false, false, false, false, true );
                    var shotsFired = es.NumShotsFired;
                    if (shotsFired > 0) {
                        continue;
                    }

                    //we want to always project shots if we have ANY remaining.
                    var shotsRemaining = singulars.Count;
                    var avgIntThisStage = 0.0f;
                    var avgDecThisStage = 0.0f;
                    var avgXPerShot = 0.0f;

                    if (AvgShots.Count > 0) {
                        Dictionary<string, float> defaulting;
                        if (!string.IsNullOrEmpty( es.StageStyleDef ) && AvgShots.TryGetValue( es.StageStyleDef, out defaulting )) {
                            //If we have scores for the stage style, use that as our default 
                            ;
                        } else {
                            //If we donn't, use the overall average shot fired
                            defaulting = GetAverageShot();
                        }

                        avgIntThisStage = defaulting["I"];
                        avgDecThisStage = defaulting["D"];
                        avgXPerShot = defaulting["X"];
                    }

                    //project the scores
                    es.Projected = new DataModel.Athena.Score();
                    es.Projected.I = es.Score.I + (int)(avgIntThisStage * shotsRemaining);
                    es.Projected.D = es.Score.D + (avgDecThisStage * shotsRemaining);
                    es.Projected.D = (float)Math.Round( es.Projected.D, 1 );
                    es.Projected.S = es.Projected.D;
                    es.Projected.X = (int)(es.Score.X + (avgXPerShot * shotsRemaining));
                }

                //Make the bold and wildly incorrect assumption that the topLevelEvent is the sum of the stages

                //Need to instantiate .Projected as it is by default null
                topLevelEventScore.Projected = this.ProjectScore( this.TopLevelEvent, 0 );

            }
        }

        private Score ProjectScore( EventComposite eventComposite, int depth ) {

            EventScore eventScore;
            Score projectedScore = new Score();
            if (this.Projection.EventScores.TryGetValue( eventComposite.EventName, out eventScore )) {

                //See if we already have a projected score for this event.
                if (eventScore.Projected != null)
                    return eventScore.Projected;

                //Check if we need to stop the recussion
                if (depth < 2 && eventComposite.HasChildren) {
                    projectedScore.S = 0; //Set S to 0, b/c the defautl is NaN. 

                    //To project the score for any stage, sum the projected scores of it's children
                    foreach (var childEvent in eventComposite.Children) {
                        var childProjectedScore = ProjectScore( childEvent, depth + 1 );
                        projectedScore.I += childProjectedScore.I;
                        projectedScore.X += childProjectedScore.X;
                        projectedScore.D += childProjectedScore.D;

                        if (eventComposite.Calculation == "SUM") {
                            projectedScore.S += childProjectedScore.D;
                        } else {
                            //SUM(i,d) ... of which yes, we really ought to be parsing it, but i ain't got tiem for that
                            if (childEvent.Equals( eventComposite.Children[0] ) )
                                projectedScore.S += childProjectedScore.I;
                            else
                                projectedScore.S += childProjectedScore.D;
                        }
                    }

                    projectedScore.D = (float)Math.Round( projectedScore.D, 1 );
                    projectedScore.S = (float)Math.Round( projectedScore.S, 1 );

                    eventScore.Projected = projectedScore;

                    return projectedScore;
                }
            }


            var defaulting = GetAverageShot();
            var numOfShots = eventComposite.GetAllSingulars().Count;

            Score score = new Score();
            projectedScore.I = (int)(defaulting["I"] * numOfShots);
            projectedScore.D = (defaulting["D"] * numOfShots);
            projectedScore.D = (float)Math.Round( projectedScore.D, 1 );
            projectedScore.S = projectedScore.D;
            projectedScore.X = (int)(defaulting["X"] * numOfShots);

            return projectedScore;
        }

        private Dictionary<string, float> GetAverageShot() {
            var avgInt = 0.0f;
            var avgDec = 0.0f;
            var avgX = 0.0f;
            foreach (var stageStyle in AvgShots) {
                avgInt += stageStyle.Value["I"];
                avgDec += stageStyle.Value["D"];
                avgX += stageStyle.Value["X"];
            }
            Dictionary<string, float> bigAvgs;
            if (AvgShots.Count > 0) {

                bigAvgs = new Dictionary<string, float> {
                    {"I", avgInt/(float)AvgShots.Count},
                    {"D", avgDec/(float)AvgShots.Count},
                    {"X", avgX/(float)AvgShots.Count}
                };

            } else {
                bigAvgs = new Dictionary<string, float> {
                    {"I", 0},
                    {"D", 0},
                    {"X", 0}
                };
            }
            return bigAvgs;
        }
    }
}
