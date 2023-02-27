using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {

    /// <summary>
    /// Helper class that creates the added structure in the data model needed for Deserialzing a MatchParticipantList object from json.
    /// </summary>
    public class MatchParticipantListWrapper : BaseClass {

        public MatchParticipantListWrapper() {

        }

        public MatchParticipantList MatchParticipantList = new MatchParticipantList();

        public override string ToString() {
            return $"Match Participant List for {MatchParticipantList.MatchName}";
        }
    }
}
