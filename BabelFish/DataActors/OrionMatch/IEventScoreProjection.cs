using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {


    public interface IEventScoreProjection : IEventScores {

        /// <summary>
        /// Project the scores in the .EventScores dictionary, using the
        /// passed in ProjectorOfScores.
        /// </summary>
        /// <param name="ps"></param>
        void ProjectScores( ProjectorOfScores ps );
    }
}
