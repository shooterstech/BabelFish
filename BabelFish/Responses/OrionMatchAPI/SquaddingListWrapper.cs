using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {

    /// <summary>
    /// Helper class that creates the added structure in the data model needed for Deserialzing a SquaddingList object from json.
    /// </summary>
    public class SquaddingListWrapper : BaseClass {
        public SquaddingList SquaddingList = new SquaddingList();

        public override string ToString() {
            return $"Squadding List for {SquaddingList.MatchName}: {SquaddingList.EventName}";
        }
    }
}
