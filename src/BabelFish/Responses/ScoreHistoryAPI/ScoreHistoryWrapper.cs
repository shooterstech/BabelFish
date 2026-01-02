using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.DataModel;

namespace Scopos.BabelFish.Responses.ScoreHistoryAPI {

    /// <summary>
    /// Helper class that creates the added structure in the data model needed for Deserialzing a Match object from json.
    /// </summary>
    public class ScoreHistoryWrapper  : BaseClass {

        public ScoreHistoryList ScoreHistoryList { get; set; }
    }
}
