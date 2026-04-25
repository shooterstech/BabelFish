using Shot = Scopos.BabelFish.DataModel.Athena.Shot.Shot;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    public interface IEventScores : IParticipant {

        /// <summary>
        /// Compiled <see cref="EventScore"/> fired by the participant within a Result COF, organized by event name.
        /// <para>Key is the EventName (as defined by the COURSE OF FIRE).</para>
        /// <para>Value is the compiled EventScore.</para>
        /// </summary>
        Dictionary<string, EventScore> EventScores { get; }

        /// <summary>
        /// Set of <see cref="Shot">Shots</see> fired by the participant within a Result COF.
        /// <para>Key is the sequence number of the shot represented as a string.</para>
        /// <para>Value is the Shot.</para>
        /// </summary>
        Dictionary<string, Shot> Shots { get; }

        /// <summary>
        /// Scores from Merged matches (e.g. tournaments, e.g. match groups)
        /// Key is (not sure yet) $"{MatchID}: {EventName}"
        /// </summary>
        Dictionary<string, EventScore> ResultCofScores { get; }

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
        /// <para>Value should only be set when shooting on ESTs. On paper, its not practically possible to know what the last shot was.</para>
        /// </summary>
        Shot? LastShot { get; set; }

        /// <summary>
        /// The UTC time this IEventScore was last updated.
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Boolean indicating if this Participant is shooting out of competition for this Event (aka shooting for score only).
        /// If true, their scores will be listed, but not ranked.
        /// </summary>
        bool OutOfCompetition { get; set; }

        /// <summary>
        /// Returns the Status of the top level Event (Event Type Event).
        /// </summary>
        /// <returns></returns>
        ResultStatus GetStatus();

        /// <summary>
        /// Returns a boolean indicating if the participant is currently competing
        /// (same as Status == INTERMEDIATE) or they are recently done. Recently done
        /// is determined by the age of the last shot.
        /// </summary>
        /// <returns></returns>
        bool CurrentlyCompetingOrRecentlyDone();
    }
}
