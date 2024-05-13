using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataActors.OrionMatch {

    /// <summary>
    /// Abstract base class, for classes that try to predict the final scores a participant will 
    /// finish with, based on the scores they already shot.
    /// 
    /// Each concrete class that implements ProjectorOfScores will have its own algorithm for
    /// making the predicted scores.
    /// </summary>
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
