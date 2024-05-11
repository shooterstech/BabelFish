using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataActors.EventScoresProjection {
    public abstract class ProjectorOfScores {

        public ProjectorOfScores( CourseOfFire courseOfFire ) {
            this.CourseOfFire = courseOfFire;
        }

        public CourseOfFire CourseOfFire { get; private set; }

        /// <summary>
        /// Calculates projected scores for the passed in IEventScoreProjection. Stores
        /// the projected scores in the EventScore's .Projected Score instance.
        /// </summary>
        /// <param name="projection"></param>
        public abstract void ProjectEventScores( IEventScoreProjection projection );
    }
}
