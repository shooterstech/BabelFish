using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.OrionMatch {

    /// <summary>
    /// Implements an interface that may be used to project (predict) the scores a participant
    /// could finish with, based on the scores they already shot.
    /// </summary>
    public interface IEventScoreProjection : IEventScores {

        /// <summary>
        /// Project the scores in the .EventScores dictionary, using the
        /// passed in ProjectorOfScores.
        /// </summary>
        /// <param name="ps"></param>
        void ProjectScores( ProjectorOfScores ps );

        //TODO: Come up with a better name.
        List<IEventScoreProjection> GetTeamMembersAsIEventScoreProjection();

        void SetTeamMembersFromIEventScoreProjection(  List<IEventScoreProjection> teamMembers );
    }
}
