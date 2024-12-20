﻿using System;
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
        /// Returns the last shot fired, if no shots have been fired, will return NULL
        /// </summary>
        /// <returns></returns>
        Shot? GetLastShot();
    }
}
