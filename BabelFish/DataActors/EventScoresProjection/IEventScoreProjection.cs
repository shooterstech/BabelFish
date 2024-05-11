using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.EventScoresProjection {
    public interface IEventScoreProjection {

        Participant Participant { get; }

        //TODO: Come up with a better name.
        List<IEventScoreProjection> GetTeamMembersAsIEventScoreProjection();

        Dictionary<string, EventScore> EventScores { get; }

        /// <summary>
        /// Project the scores in the .EventScores dictionary, using the
        /// passed in ProjectorOfScores.
        /// </summary>
        /// <param name="ps"></param>
        void ProjectScores( ProjectorOfScores ps );
    }
}
