using System;
using System.Collections.Generic;
using System.Text;
using Shot = Scopos.BabelFish.DataModel.Athena.Shot.Shot;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    public interface IEventScores : IParticipant {

        //TODO: Come up with a better name.
        List<IEventScores> GetTeamMembersAsIEventScores();

        Dictionary<string, EventScore> EventScores { get; }

        Dictionary<string, Shot> Shots { get; }
    }
}
