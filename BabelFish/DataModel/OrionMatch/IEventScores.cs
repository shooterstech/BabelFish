using System;
using System.Collections.Generic;
using System.Text;
using Shot = Scopos.BabelFish.DataModel.Athena.Shot.Shot;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    public interface IEventScores : IParticipant {

        Dictionary<string, EventScore> EventScores { get; }

        Dictionary<string, Shot> Shots { get; }

        /// <summary>
        /// Returns a copy of the Shots dictionary, but with the key being the Singular's EventName.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, Shot> GetShotsByEventName();

        /// <summary>
        /// Returns the last shot fired from the Shots Property, which only includes competition shots, if no shots have been fired, will return NULL
        /// </summary>
        /// <returns></returns>
        Shot? GetLastCompetitionShot();

        /// <summary>
        /// Returns the last shot fired, to include both competition and sighting shots.
        /// </summary>
        Shot? LastShot { get; set; }
    }
}
