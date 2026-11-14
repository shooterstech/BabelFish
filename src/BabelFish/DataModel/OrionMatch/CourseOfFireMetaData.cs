using System.Collections.Concurrent;

namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Contains data about the course of fire was competed. Such as the scoring systems in use.
    /// </summary>
    public class CourseOfFireMetaData {

        /// <summary>
        /// Lists the types of scoring systems used in the course of fire when it was competed.
        /// The key is the type of scoring system (e.g. EST), and the value is the name of the scoring system used (e.g. Athena).
        /// </summary>
        public ScoringTechnologyList ScoringTechnology { get; set; } = new ScoringTechnologyList();
    }

    /// <summary>
    /// IN use by both <see cref="CourseOfFireMetaData"/> and <see cref="ResultListMetadata"/> to list the scoring technologies used in the match.
    /// Defining this as a separate class to avoid code duplication and to make it clear that the list of scoring technologies is used in multiple places in the data model.
    /// </summary>
    public class ScoringTechnologyList : ConcurrentDictionary<ScoringSystem, ConcurrentBag<string>> {
    }
}
