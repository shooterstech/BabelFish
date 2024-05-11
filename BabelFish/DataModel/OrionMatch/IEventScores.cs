using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    public interface IEventScores : IParticipant {

        //TODO: Come up with a better name.
        List<IEventScores> GetTeamMembersAsIEventScores();

        Dictionary<string, EventScore> EventScores { get; }
    }
}
