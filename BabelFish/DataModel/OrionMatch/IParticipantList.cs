using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    public interface IParticipantList {

        /// <summary>
        /// List of IParticipants. Known concrete classes are MatchParticipant, SquaddingAssignment, or ResultEvent.
        /// </summary>
        public List<IParticipant> Items { get; set; }
    }
}
