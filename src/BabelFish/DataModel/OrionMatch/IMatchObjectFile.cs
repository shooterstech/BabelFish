using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataModel.OrionMatch {



    /// <summary>
    /// Represents a top level match object that is intended to be saved to file (persistance). There are three general types of match objects that are saved to file.
    /// <list type="bullet">
    /// <item>Configuration files that describe how a match is setup. For example <see cref="CourseOfFireStructure"/> or <see cref="MatchParticipant"/></item>
    /// <item>Raw score data files. For example the files <see cref="ShotMapper"/> produces.</item>
    /// <item>Compiled result files. For example <see cref="ResultList"/> or <see cref="ResultCOF"/>.</item>
    /// </list>
    /// </summary>
    public interface IMatchObjectFile : ISaveToFile, ICheckSum {

        /// <summary>
        /// The unique identifier for the match that this object is a part of.
        /// </summary>
        /// <remarks>To maintain backward compatibility, the property uses capitol I and capitol D.</remarks>
        public MatchID MatchID { get; }
    }
}
