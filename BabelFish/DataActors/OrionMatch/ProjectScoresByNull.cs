using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataActors.OrionMatch {

    /// <summary>
    /// Null operator for projecting scores, meaning it implements the abstract class
    /// ProjectorOfScores, but doesn't actually project anything. In fact, it sets all
    /// the .Projected scores to null.
    /// </summary>
    public class ProjectScoresByNull : ProjectorOfScores {

        public ProjectScoresByNull( CourseOfFire courseOfFire ) : base( courseOfFire ) {
        }

        public override string ProjectionMadeBy {
            get {
                return "BabelFish ProjectScoresByNull";
            }
        }

        public override void ProjectEventScores( IEventScoreProjection projection ) {
            
            foreach( var eventScore in projection.EventScores.Values) {
                eventScore.Projected = null;
            }
        }
    }
}
